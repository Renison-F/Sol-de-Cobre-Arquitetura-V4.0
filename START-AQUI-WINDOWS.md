# Começar no Windows — continuar o Sol de Cobre (com Unity)

Guia para preparar o PC e continuar o desenvolvimento, inclusive dirigindo o Unity pelo
Claude Code local. A lógica de jogo (A→G) já está implementada e testada; aqui você monta
o ambiente que faltava (Unity, projeto, HUD, arte).

> **Por que local?** O Unity precisa de instalação gráfica + licença + sua conta Unity.
> Isso é feito na sua máquina. O Claude Code **local** (no seu PC) consegue abrir/compilar
> o projeto e rodar o Unity em modo batch; a sessão na nuvem, não.

## 0. Pré-requisitos (uma vez)

- **Unity Hub** + **Unity 6000.3.24f1** com módulo **Android** (e iOS se for buildar no Mac).
  Baixe pela unity.com/download; ative a licença (Personal/Plus/Pro conforme sua conta).
- **Git** e **Git LFS**.
- **.NET SDK 8** (opcional, roda os 95 testes de Domain fora do Unity — rápido).
- **Python 3** (opcional, para os validadores do portal e o runner `run_unity.py`).
- **Claude Code** instalado no PC (se quiser continuar comigo localmente).

## 1. Pegar o repositório e preparar o kit

Abra o PowerShell e rode (ajuste os caminhos se quiser):

```powershell
# clona o repo de arquitetura (este) e prepara dependências não-GUI
git clone https://github.com/Renison-F/Sol-de-Cobre-Arquitetura-V4.0.git C:\Projetos\SolDeCobre\Arquitetura
cd C:\Projetos\SolDeCobre\Arquitetura
# O trabalho A→G + scripts estão na branch de feature até o PR #1 ser mergeado na main:
git checkout feat/incrementos-a-d-headless
powershell -ExecutionPolicy Bypass -File tools\setup-windows.ps1 -InstallDeps
```

> Depois que o **PR #1** for mergeado na `main`, o `git checkout` acima deixa de ser
> necessário (a `main` passa a ter tudo).

O script verifica/instala Git/LFS/.NET/Python (com `-InstallDeps` via winget), roda os testes
de Domain se houver .NET, e imprime os próximos passos. Ainda **não** copia o `_Game` porque
o projeto Unity não existe — faça o passo 2 e rode o script de novo.

## 2. Criar o projeto Unity (URP)

1. No **Unity Hub → New project → Universal 3D (URP)**, versão **6000.3.24f1**,
   local `C:\Projetos\SolDeCobre\Game`.
2. Abra o projeto uma vez para o Editor gerar `Packages/manifest.json`, `packages-lock.json`
   e `ProjectSettings/ProjectVersion.txt` **reais** (estes a gente versiona depois).

## 3. Trazer o kit de código e os pacotes

```powershell
cd C:\Projetos\SolDeCobre\Arquitetura
powershell -ExecutionPolicy Bypass -File tools\setup-windows.ps1 `
  -ArchDir "C:\Projetos\SolDeCobre\Arquitetura" -GameDir "C:\Projetos\SolDeCobre\Game"
```

Isso copia `unity-project-template\Assets\_Game` → `Game\Assets\_Game` e os arquivos
`.gitignore`/`.gitattributes`. (Se já existir um `_Game`, ele move para backup — **migrar por
diff**, nunca duplicar.)

No Editor, **Package Manager** → instalar **`com.unity.inputsystem`** e
**`com.unity.test-framework`** (ver `unity-project-template\Packages\README.md`).
Em **Edit → Project Settings → Player → Active Input Handling** escolha **Input System**.

## 4. Verificar

- **Window → General → Test Runner → EditMode → Run All**: devem passar **95 testes** de Domain.
- Menu **Sol de Cobre → Bootstrap → Create movement laboratory** → abra `MovementLab.unity`,
  dê Play: A/D/setas + espaço + gamepad.
- Menu **Sol de Cobre → Bootstrap → Create Estação slice** → graybox do confronto (laço no
  núcleo do Fiscal, inimigos, checkpoints, save).
- HUD de jogo: siga `unity-project-template\Assets\_Game\UI\HUD-CANVAS.md`.

Pelo terminal (opcional, sem abrir o Editor):

```powershell
python tools\run_unity.py --editor "C:\Program Files\Unity\Hub\Editor\6000.3.24f1\Editor\Unity.exe" `
  --project "C:\Projetos\SolDeCobre\Game" --task editmode
```

## 5. Continuar com o Claude Code local

Abra o Claude Code na pasta do **projeto Unity** (`C:\Projetos\SolDeCobre\Game`) ou na do
repo de arquitetura. O `CLAUDE.md` da raiz dá o contexto (camadas, comandos, convenções) e eu
posso, a partir daí, rodar EditMode/lab/slice via `run_unity.py`, gerar cenas por Editor script
e continuar os incrementos que dependem do Editor.

## O que ainda é produção sua (não automatizável)

- **Arte/áudio/animação 3D** (elenco, trecho, trilha) — Blender/Unity.
- **Canvas da HUD** e **balanceamento** do slice de 8–12 min.
- **Adaptadores comerciais reais** (loja/atestação/KMS/backend) — suas contas; os placeholders
  atuais falham de propósito até você plugar os reais.
