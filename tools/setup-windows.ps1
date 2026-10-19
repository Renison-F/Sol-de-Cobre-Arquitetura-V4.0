<#
.SYNOPSIS
  Prepara o ambiente local (Windows) do Sol de Cobre: dependências não-GUI, clone do
  repo de arquitetura e cópia do kit _Game para o projeto Unity. NÃO instala o Unity
  (instalação licenciada via Unity Hub é manual — ver START-AQUI-WINDOWS.md).

.EXEMPLO
  # 1) Crie o projeto URP no Unity Hub em C:\Projetos\SolDeCobre\Game
  # 2) Rode:
  powershell -ExecutionPolicy Bypass -File tools\setup-windows.ps1 `
     -ArchDir "C:\Projetos\SolDeCobre\Arquitetura" `
     -GameDir "C:\Projetos\SolDeCobre\Game" -InstallDeps
#>
[CmdletBinding()]
param(
  [string]$RepoUrl = "https://github.com/Renison-F/Sol-de-Cobre-Arquitetura-V4.0.git",
  [string]$ArchDir = "C:\Projetos\SolDeCobre\Arquitetura",
  [string]$GameDir  = "C:\Projetos\SolDeCobre\Game",
  [switch]$InstallDeps,
  [string]$UnityEditor = ""   # opcional: caminho do Unity.exe 6000.3.24f1 para rodar EditMode
)

$ErrorActionPreference = "Stop"
function Step($m) { Write-Host "`n==> $m" -ForegroundColor Cyan }
function Ok($m)   { Write-Host "    [ok] $m" -ForegroundColor Green }
function Warn($m) { Write-Host "    [!]  $m" -ForegroundColor Yellow }
function Have($c) { $null -ne (Get-Command $c -ErrorAction SilentlyContinue) }

Step "Verificando dependências (git, git-lfs, dotnet, python)"
$deps = @(
  @{ cmd="git";    winget="Git.Git";               nome="Git" },
  @{ cmd="git-lfs";winget="GitHub.GitLFS";         nome="Git LFS" },
  @{ cmd="dotnet"; winget="Microsoft.DotNet.SDK.8";nome=".NET SDK 8 (testes locais)" },
  @{ cmd="python"; winget="Python.Python.3.12";    nome="Python 3 (validadores)" }
)
foreach ($d in $deps) {
  if (Have $d.cmd) { Ok "$($d.nome) presente" }
  elseif ($InstallDeps -and (Have "winget")) {
    Warn "Instalando $($d.nome) via winget…"
    winget install --id $($d.winget) -e --accept-source-agreements --accept-package-agreements
  } else {
    Warn "$($d.nome) ausente. Instale, ou rode com -InstallDeps (precisa de winget). winget id: $($d.winget)"
  }
}

Step "Clonando/atualizando o repositório de arquitetura em $ArchDir"
if (Test-Path (Join-Path $ArchDir ".git")) {
  git -C $ArchDir pull --ff-only
  Ok "Repo atualizado"
} else {
  New-Item -ItemType Directory -Force -Path (Split-Path $ArchDir) | Out-Null
  git clone $RepoUrl $ArchDir
  Ok "Repo clonado"
}
if (Have "git-lfs") { git -C $ArchDir lfs install | Out-Null; Ok "Git LFS inicializado" }

$kit = Join-Path $ArchDir "unity-project-template"
if (-not (Test-Path (Join-Path $kit "Assets\_Game"))) { throw "Kit _Game não encontrado em $kit." }

Step "Validando a camada Domain (engine-free) com .NET, se disponível"
if (Have "dotnet") {
  $verify = Join-Path $env:TEMP "sc-verify"
  New-Item -ItemType Directory -Force -Path $verify | Out-Null
  $csproj = @"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <LangVersion>latest</LangVersion>
    <EnableDefaultCompileItems>false</EnableDefaultCompileItems>
    <Nullable>disable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include="$kit\Assets\_Game\Domain\**\*.cs" />
    <Compile Include="$kit\Assets\_Game\Tests\EditMode\**\*.cs" />
  </ItemGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.11.1" />
    <PackageReference Include="NUnit" Version="3.14.0" />
    <PackageReference Include="NUnit3TestAdapter" Version="4.6.0" />
  </ItemGroup>
</Project>
"@
  Set-Content -Path (Join-Path $verify "sc-verify.csproj") -Value $csproj -Encoding UTF8
  Push-Location $verify
  try { dotnet test --nologo; Ok "Testes de Domain executados (deve passar 95/95)" }
  finally { Pop-Location }
} else { Warn "Sem dotnet: pule — os mesmos testes rodam no Unity (Test Runner → EditMode)." }

Step "Copiando o kit _Game e a config Git para o projeto Unity em $GameDir"
if (-not (Test-Path (Join-Path $GameDir "Assets"))) {
  Warn "O projeto Unity ainda não existe em $GameDir."
  Warn "Crie-o primeiro no Unity Hub (Universal 3D / URP, baseline 6000.3.24f1) e rode de novo."
} else {
  $dstGame = Join-Path $GameDir "Assets\_Game"
  if (Test-Path $dstGame) {
    $backup = Join-Path $GameDir ("_Game_backup_" + (Get-Date -Format "yyyyMMdd_HHmmss"))
    Warn "Já existe Assets\_Game — movendo para backup fora de Assets: $backup (migrar por diff, sem duplicar)."
    Move-Item $dstGame $backup
  }
  Copy-Item (Join-Path $kit "Assets\_Game") (Join-Path $GameDir "Assets\") -Recurse
  foreach ($f in @(".gitignore", ".gitattributes")) {
    $src = Join-Path $kit $f
    if (Test-Path $src) { Copy-Item $src (Join-Path $GameDir $f) -Force }
  }
  Ok "Kit _Game e config Git copiados"
}

if ($UnityEditor -ne "" -and (Test-Path $UnityEditor) -and (Test-Path (Join-Path $GameDir "Assets"))) {
  Step "Rodando EditMode no Unity em modo batch (via tools/run_unity.py)"
  if (Have "python") {
    python (Join-Path $ArchDir "tools\run_unity.py") --editor "$UnityEditor" --project "$GameDir" --task editmode
  } else { Warn "Sem python: rode os testes pelo Test Runner do Editor." }
}

Step "Próximos passos manuais (Unity — licenciado/GUI)"
Write-Host @"
  1. Unity Hub: instalar Unity 6000.3.24f1 + módulo Android (e iOS se for buildar no Mac).
  2. Se ainda não criou: novo projeto Universal 3D / URP em $GameDir.
  3. Package Manager: adicionar com.unity.inputsystem e com.unity.test-framework
     (ver $kit\Packages\README.md). Player → Active Input Handling = Input System.
  4. Esperar compilar. Test Runner → EditMode: devem passar os 95 testes.
  5. Menu Sol de Cobre → Bootstrap → Create movement laboratory (andar/saltar/câmera).
  6. Menu Sol de Cobre → Bootstrap → Create Estação slice (graybox do confronto).
  7. Montar o canvas da HUD: $kit\Assets\_Game\UI\HUD-CANVAS.md
  Guia completo: START-AQUI-WINDOWS.md
"@ -ForegroundColor Gray
Ok "Ambiente preparado (parte automatizável)."
