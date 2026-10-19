# Revisão de nomenclatura V3.3 — 19/10/2026

Escopo: Isadora Veredas, título da prancha e referências de produção. Os 21 outros PNGs foram preservados. A edição mantém cinco quadros de detalhes legendados. A nova prancha foi inspecionada e validada como PNG horizontal. Os testes DOM do portal e o validador de conteúdo passaram após o novo nome e caminhos; não foram repetidos testes do núcleo de licença.

---

# Revisão visual V3.2 — 19/10/2026

Escopo: padronização das dez pranchas solicitadas e retirada do cartão redundante da dupla na galeria. A revisão inclui ainda legendas em todos os quadros de detalhes das 17 pranchas individuais: sete pranchas anteriores receberam esse ajuste, além das dez refeitas. O alinhamento das dez fases do mapa permanece vigente.

Os 15 testes do servidor documental passaram nesta revisão. Os testes DOM passaram nas 22 áreas do portal. A galeria contém 21 entradas com caminhos únicos e sem o cartão `ze-e-trovico.png`. Foram verificados Markdown, busca, tema, lightbox, leitura ao vivo/offline e lógica do menu móvel, além das dez fases alinhadas à trilha e nomes completos.

As imagens desta revisão foram geradas pela ferramenta nativa, inspecionadas individualmente e validadas como PNG horizontal. Os prompts finais e referências de identidade/direção constam no documento de procedência V3.2.

Não houve validação da página inteira em navegador gráfico. A revisão não modifica o núcleo de licenciamento e não repete seus testes. Os registros abaixo pertencem às revisões anteriores.

---

# Revisão V3.1 — 19/10/2026

Escopo desta revisão: quatro pranchas de personagem e alinhamento do mapa. As quatro novas imagens foram inspecionadas e validadas como PNG 1536 × 1024. Somente os quatro arquivos solicitados de personagens tiveram pixels alterados.

As 22 áreas do portal passaram pelos testes de interação DOM, incluindo busca, tema, lightbox e leitura ao vivo/offline. O teste do mapa confirmou dez marcadores exatamente sobre os endpoints da curva, igualdade entre as duas camadas de trilha e nomes completos sem truncamento. O SVG resultante foi renderizado e inspecionado visualmente; a composição da página inteira não foi validada em navegador gráfico nesta revisão.

A revisão preserva o núcleo de licença e o restante do escopo do jogo. Os resultados abaixo registram a validação da V3 anterior; os testes de licenciamento não foram repetidos, pois o núcleo não foi alterado.

---

# Relatório de validação V3 — 19/10/2026

## Resultados observados

| Área | Verificação | Resultado |
|---|---|---|
| Núcleo comercial | 25 testes unittest de assinatura, claims, vínculos, prazo, replay e concorrência | 25/25 passaram em Python 3.12 e cryptography 50.0.2 |
| Servidor documental | 15 testes de API/estático, travessia, symlink, Host, Origin, HEAD, escrita e atualização real | 15/15 passaram |
| Portal | Integração DOM das 22 áreas, destinos de documento/arte, Markdown, busca, tema, lightbox e menu | Passou sem erro de execução observado |
| Ao vivo / offline | API lê alteração real; portal abre catálogo e galeria por file:// | Passou nos testes de DOM e servidor |
| Conteúdo | 10 níveis, 60 sequências, 16 identidades, 19 variantes, 22 imagens, 157 tarefas e 51 linhas de diálogo catalogadas | IDs/referências íntegros; dependências sem ciclo |
| Sintaxe | Python compilado; JavaScript verificado com node --check | Passou |
| Contratos | JSON lido; YAML OpenAPI interpretado e 22 referências locais resolvidas | Passou em estrutura; não equivale a validação formal integral de OpenAPI |
| Artes | 16 saídas novas inspecionadas visualmente, seis originais preservadas | Pranchas incluídas e associadas por ID |

## Limites da validação

Os testes do portal foram executados em jsdom; verificam estrutura e interações, não renderização de pixels. O navegador Chromium não estava instalado e o download do binário de teste não ficou disponível. Não houve captura de layout nem auditoria visual em browser desktop/celular da V3. O CSS e estrutura da V2 foram preservados, com ajustes limitados de apresentação para as novas pranchas. Teste de DOM do menu em breakpoint móvel não prova layout responsivo.

Não houve compilação C#/Unity/IL2CPP, integração Play/App Store, KMS/infra de produção, pentest externo, teste de desempenho ou jogabilidade em celular. Os PNGs não contêm malhas, rigs, animações ou áudio final. Nenhum teste do núcleo usa compra real; StoreResult de teste é explícito e interno, sem endpoint HTTP de sucesso simulado.

## Reproduzir

Núcleo: dentro de `security-reference`, instale requirements em venv e execute `python -m unittest discover -s tests -v`. Servidor: na raiz execute `python tools/test_docs_server.py -v`. Conteúdo: `python tools/validate_project.py`. Portal DOM: instale dependências opcionais de QA com `npm ci --prefix tools/qa` e execute `node tools/test_portal_dom.cjs`.

Os relatórios registram o estado do pacote e devem ser repetidos após alterações relevantes. Segurança de produção exige a matriz de integração de `10-seguranca/11-testes-e-fontes.md`; não se infere de contagem de testes. O inventário de arquivos e hashes é entregue na raiz do ZIP.
