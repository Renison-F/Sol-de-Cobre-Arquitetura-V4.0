# Começar o desenvolvimento — V4

## Resultado desta entrega

Portal offline e explorador local ampliados; Game Bible com escopo e regras; elenco e bestiário com concept arts; núcleo criptográfico executável; referências C# e contratos de serviço; backlog rastreável. A documentação permite iniciar implementação sem esperar a invenção de cada sistema básico. Não há jogo Unity compilado ou garantia de publicação imediata.

## Entrada recomendada da V4

Começar por `12-desenvolvimento/08-bootstrap-local-v4.md`, depois seguir `12-desenvolvimento/09-ordem-de-implementacao.md`. A modelagem individual está em `09-arte-audio/modelagem/INDICE.md`. Para passar o projeto ao Claude/Codex, usar `12-desenvolvimento/13-handoff-claude-codex.md`.

## Ordem prática

1. Abrir `index.html`, visitar **Preparação para desenvolvimento**, **Segurança comercial**, **Personagens** e **Galeria**.
2. Ler `05-game-design/GDD.md`, `01-universo/08-canon-v3.md` e `10-seguranca/00-SEGURANCA.md`.
3. Criar repositório privado do jogo, Git LFS para fontes pesadas e projeto URP no Unity **6000.3.24f1** como baseline verificável da linha 6.3 LTS. A versão foi encontrada em página oficial; ainda não foi compilada neste ambiente. Avaliar correções posteriores de segurança e regressões antes do primeiro build de produção.
4. Usar módulos Android/iOS da instalação oficial e pacotes compatíveis resolvidos pelo template; commitar `manifest.json`, lockfile e `ProjectVersion.txt` gerados pelo Editor. Não adivinhar versões de pacotes a partir do número do editor.
5. Migrar as referências V4 sem duplicar scripts antigos, compilar, executar EditMode e gerar o laboratório de movimento. Depois implementar o slice da Estação com blockout. Resolver primeiro movimento, câmera, laço, checkpoints e touch.
6. Em paralelo de disciplinas dentro da equipe, provisionar sandbox de loja e backend de licença, implementar bridges oficiais e provar interoperabilidade do envelope; não inserir resultado de compra fake no release.
7. Produzir Mara/Faísca/Fiscal e kit da Estação após validar escala e controle. Testar build de loja no aparelho físico.
8. Passar gate do slice e então produzir os nove capítulos restantes pelo backlog.

## Decisões tomadas

Single-player, dois heróis alternáveis, mascotes contextuais, compra integral do app, ativação inicial online, licença de 30+7 dias, sem nuvem/ranking/anúncio/IAP, PT-BR inicial, final único com variações opcionais. Qualquer mudança tem ADR e impacto no backlog.

## Entradas externas do proprietário

Contas de desenvolvedor e verificação das lojas, identidade comercial e contato de suporte, domínio e conta de nuvem, projeto Google/Apple, certificados e segredos, aparelhos de teste e orçamento da equipe. São insumos reais que não podem ser fabricados dentro de um ZIP. Podem ser providenciados durante o bootstrap e não impedem prototipar movimento e fase local.

## Critério de prontidão

Pronto para **iniciar desenvolvimento**: escopo, contratos, referência visual, roteiro e sequência de execução definidos. Pronto para **publicar**: todas as tarefas de produto implementadas e gates reais aprovados. A matriz de cobertura distingue ambos, sem esconder trabalho de produção em “sem pendências”.

Fonte da versão: [Unity 6000.3.24f1](https://unity.com/releases/editor/whats-new/6000.3.24f1), consultada em 19/10/2026.
