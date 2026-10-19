# Validação V4.0 — kit de desenvolvimento e produção 3D

Data: 19/10/2026. Escopo: organização de código, ferramentas de laboratório, contratos e documentação de produção. As 22 imagens da V3.3 são preservadas sem alteração; a galeria continua com 21 entradas únicas e o mapa mantém dez fases alinhadas.

## Verificações executadas

| Frente | Resultado observado |
|---|---|
| Conteúdo | 10 níveis, 60 sequências, 16 identidades, 19 inimigos, 22 PNGs, 157 tarefas e 51 linhas de diálogo; referências/IDs/dependências validados |
| Modelagem | 17 especificações principais e metas de 22 pacotes secundários (19 inimigos/3 famílias NPC), com rigs, LODs, sockets, animações e budgets; catálogos e resumo CSV principal consistentes |
| Assemblies | Quatro asmdefs, referências sem ciclo; Domain sem dependência textual de Unity/QuestDefinition |
| Handoff 3D | 12 testes Python passaram: caminhos/escape por symlink, fonte ausente, LOD duplicado, limites de triângulos/skin/textura, nomes obrigatórios e aceite sem provas |
| Runner | Sintaxe Python verificada; dry-run com caminhos contendo espaços preserva argumentos e não cria Builds; baseline divergente rejeitada |
| Portal | 22 áreas passaram no teste DOM, incluindo novos destinos, Markdown, busca, tema, lightbox, leitura ao vivo/offline e menu |
| Galeria e mapa | 21 caminhos únicos, sem cartão redundante da dupla; dez centros na curva e nomes completos |
| Servidor documental | 15 testes passaram, inclusive proteção de caminhos e atualização real do disco |
| Sintaxe | JavaScript do portal passou em node --check; novas ferramentas Python compiladas em py_compile |

Os bytes sintéticos usados nos testes de handoff verificam apenas o contrato/arquivos. Não são modelos FBX/Blender de teste de importação. O validador não inspeciona geometria; esse limite aparece também no seu resultado e no guia de homologação.

## Ainda não executado neste ambiente

Unity, Unity Hub, Blender e compilador .NET/C# não estavam instalados. Não houve compilação dos novos C#, resolução de pacotes Unity, execução de ProjectBootstrap, geração real de cena/prefab, execução dos quatro testes EditMode fornecidos, build APK/IPA ou medição em aparelho. O teste DOM usa jsdom e não equivale a inspeção gráfica de layout. Não houve produção de malha/rig/animação nem execução de ferramentas Blender.

O código C# de licença foi movido para Domain sem alteração de algoritmo; a suíte Python de segurança não foi repetida nesta revisão. A alteração funcional de QuestState é a troca da dependência de ScriptableObject por lista de flags, com ponte QuestRules no Runtime. Essa ponte e o input/gerador precisam de homologação no Editor real.

O laboratório é uma cápsula de movimento com teclado/gamepad, câmera e degraus. Não implementa touch, laço, mascotes, chefe, save integrado, gate comercial nem o slice de 8–12 minutos. O save de referência continua schema1; o contrato schema3 permanece tarefa DEV-08/09. Não há lockfile/projeto Unity completo fabricado no ZIP.

## Reproduzir

Na raiz do pacote, executar:

```bash
python tools/validate_project.py
python tools/validate_production.py
python tools/test_model_delivery.py -v
python tools/test_docs_server.py -v
node --check dashboard/js/app.js
```

Para DOM: instalar as dependências opcionais com npm ci --prefix tools/qa e executar node tools/test_portal_dom.cjs. Para Unity: seguir `12-desenvolvimento/08-bootstrap-local-v4.md`, compilar, gerar laboratório e executar EditMode. Registrar Editor/locks, logs/XML, commit e aparelho antes de marcar incremento produtivo como concluído.
