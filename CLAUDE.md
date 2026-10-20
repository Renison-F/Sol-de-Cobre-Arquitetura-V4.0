# CLAUDE.md — Sol de Cobre (contexto para o Claude Code)

Jogo mobile **2.5D plataforma**, **Unity 6000.3.24f1 / URP**, C#, Android/iOS, single-player,
dois heróis (Mara/Zé), compra integral do app, ativação online + licença 30+7 dias. Este repo
é a **arquitetura viva + kit de código**; o projeto Unity jogável é montado à parte (ver
`START-AQUI-WINDOWS.md`).

## Onde está o quê

- `unity-project-template/Assets/_Game/` — **o kit de código** que se copia para `Game/Assets/`.
  Quatro assemblies: `SolDeCobre.Domain` (engine-free), `SolDeCobre.Runtime`, `SolDeCobre.UI`,
  `SolDeCobre.Editor`, + `SolDeCobre.Tests.EditMode`.
- `06-arquitetura/`, `05-game-design/`, `10-seguranca/`, `11-contratos/` — normas/contratos.
- `12-desenvolvimento/` — ordem de implementação, handoff e a resposta por incremento
  (`15-incrementos-a-d-headless.md`, cobre A→G + integração).
- `tools/` — validadores (`validate_project.py`, `test_model_delivery.py`) e `run_unity.py`
  (batch: lab / editmode / lab-android).

## Estado atual

Incrementos **A→G + integração** implementados e com lógica testada. **Domain = 95 testes
EditMode, engine-free**, verificável sem Unity. Runtime/UI/Editor compilam no Unity. Branch de
trabalho: `feat/incrementos-a-d-headless` (PR #1). Falta: arte/áudio 3D, canvas da HUD,
balanceamento do slice, adaptadores comerciais reais.

## Como verificar

- **No Unity:** Test Runner → EditMode (95 testes de Domain). `run_unity.py --task editmode`
  roda em batch.
- **Sem Unity (rápido):** compile a camada Domain com .NET e rode os testes — ver o harness
  que `tools/setup-windows.ps1` gera (inclui `Domain/**` + `Tests/EditMode/**`, net8.0, NUnit).
- **Portal/dados:** `python tools/validate_project.py`; `python -m py_compile serve.py tools/reindex.py`;
  `node --check dashboard/js/app.js`.

## Convenções (não negociáveis)

- **Migrar por diff**: nunca duplicar `_Game` antigo e novo em `Assets`.
- **Sem segredos no cliente/repo/logs**; chave privada nunca entra no cliente.
- **Sem prova de compra falsa**: adaptadores comerciais ausentes **falham** explicitamente,
  nunca devolvem sucesso fictício. Não marcar backlog produtivo como pronto por haver só texto
  ou C# não compilado no Unity.
- **Não adivinhar versões de pacotes**: `manifest.json`/lock/`ProjectVersion.txt` são gerados
  pelo Editor e versionados reais.
- Mudanças **pequenas e rastreáveis**, com critério de aceite e evidência (testes).
- Toda lógica testável vai para `Domain` (engine-free) com teste EditMode; MonoBehaviours ficam
  finos, só ligando o domínio à cena.

## Padrões de código

C# compacto no estilo do kit existente, comentários curtos em PT-BR explicando invariantes.
`Domain` só usa `System.*` (sem `UnityEngine`). Novos sistemas: regra pura + adaptador runtime.
