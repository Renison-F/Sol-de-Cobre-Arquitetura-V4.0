# Começar localmente com o kit V4

## O que existe aqui

Há quatro assemblies: Domain (sem UnityEngine), Runtime (adaptação Unity/Input System), Editor (automação) e testes EditMode. O gerador ProjectBootstrap cria um laboratório de movimento com uma cápsula de Mara, configuração HeroDefinition, Input Actions, prefab, chão/degraus, luz e câmera. É um teste técnico de andar/saltar; não contém laço, Faísca, Fiscal, touch, save integrado, compra nem os 8–12 minutos do slice.

O ambiente desta entrega não possui Unity/Blender. O código foi organizado e revisado; compilação C#, execução do gerador, testes EditMode e APK ainda precisam ocorrer na instalação real. Não há .unity/.prefab/.blend/FBX final no ZIP; esses arquivos serão criados pelas ferramentas apropriadas. O relatório diferencia verificações executadas e não executadas.

## Instalação inicial no Windows

1. Instalar Unity pelo Hub com a baseline existente **6000.3.24f1**, suporte Android e ferramentas correspondentes. Não é afirmação de que essa seja a última correção. Ao escolher outra versão, homologar regressões e atualizar ADR, runner e ProjectVersion juntos.
2. Criar projeto **Universal 3D / URP** numa pasta do jogo, por exemplo C:/Projetos/SolDeCobre/Game. O portal documental pode ficar em Docs. Não colocar o jogo dentro do servidor público do XAMPP: Unity não depende de PHP/Apache para rodar.
3. Resolver URP pelo template. Instalar Input System e Unity Test Framework pelo Package Manager nas versões compatíveis apresentadas pelo Editor. Em Active Input Handling, habilitar o Input System e reiniciar se solicitado.
4. Copiar a pasta _Game de unity-project-template/Assets para Game/Assets. Se estiver atualizando a referência V3, retirar a antiga _Game para um backup fora de Assets antes de copiar: manter as duas cria classes duplicadas. Não apagar scripts próprios já implementados; migrar por diff.
Copiar também os arquivos de configuração Git do diretório unity-project-template para a raiz do projeto Game, revisar se já existirem e instalar Git LFS antes de adicionar fontes pesadas. Não ignorar Assets/_Generated nem os .meta: o laboratório gerado e seus GUIDs podem ser versionados para a equipe.

5. Esperar compilação. Se houver erro, resolver antes de gerar cena. Versionar Assets e seus .meta, ProjectSettings, manifest e packages-lock reais. O pacote não fornece lockfile imaginado.
6. Salvar cenas abertas. Executar menu **Sol de Cobre → Bootstrap → Create movement laboratory**. A pasta Assets/_Generated/SC_Lab_V4 precisa não existir. O comando não sobrescreve uma geração anterior; se uma execução falhar, preservar a pasta parcial para diagnóstico antes de uma nova tentativa.
7. Abrir MovementLab.unity da pasta gerada. Play: A/D ou setas para andar, espaço para saltar, gamepad analógico/d-pad e botão sul. Testar salto curto/longo, quina, câmera e perda de foco. Nenhum teste de touch é automaticamente aprovado pelo laboratório.

## Automação pelo terminal

Fechar o Editor que estiver usando esse projeto antes de executar outra instância. Exemplo a partir da raiz deste pacote, em PowerShell:

```powershell
python tools/run_unity.py --editor "C:/Program Files/Unity/Hub/Editor/6000.3.24f1/Editor/Unity.exe" --project "C:/Projetos/SolDeCobre/Game" --task lab --dry-run
python tools/run_unity.py --editor "C:/Program Files/Unity/Hub/Editor/6000.3.24f1/Editor/Unity.exe" --project "C:/Projetos/SolDeCobre/Game" --task lab
python tools/run_unity.py --editor "C:/Program Files/Unity/Hub/Editor/6000.3.24f1/Editor/Unity.exe" --project "C:/Projetos/SolDeCobre/Game" --task editmode
python tools/run_unity.py --editor "C:/Program Files/Unity/Hub/Editor/6000.3.24f1/Editor/Unity.exe" --project "C:/Projetos/SolDeCobre/Game" --task lab-android
```

Logs e XML de testes vão para Builds/Logs no projeto Unity. --dry-run apenas imprime os argumentos e valida os caminhos/baseline; não compila nem simula sucesso do Unity. O runner não instala, ativa licença, baixa SDK ou informa credenciais. Para o APK, o alvo Android precisa estar disponível; a compilação é Development e contém só o laboratório. Não é candidato comercial. Builds existentes são preservados até arquivá-los conscientemente. Testar no aparelho físico via fluxo local de QA, sem publicar nas lojas.

Para iOS, Unity exporta o projeto Xcode; a etapa local de compilação/assinatura usa macOS/Xcode. Um runner macOS pode automatizar essa parte. O runner entregue nesta versão cobre laboratório Android e EditMode, sem alegar geração de IPA.
