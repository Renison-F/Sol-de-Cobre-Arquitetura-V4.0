# Arquitetura deste portal de documentação

## Componentes
`index.html` (shell sem CSS/JS inline), `dashboard/css/` (tokens, layout, responsividade), `dashboard/js/` (navegação, renderizadores, explorador, diagramas), `dashboard/data/catalog.js` (snapshot offline de documentos/arquivos) e `serve.py` (servidor local opcional com API segura).

## Dois modos
- **Offline / arquivo:** snapshot embutido em JavaScript funciona com `file://`, sem fetch ou internet. Mudanças externas exigem `python tools/reindex.py`.
- **Ao vivo:** `python serve.py`, abre URL local e habilita refresh real da árvore e leitura de Markdown/código/imagem diretamente do diretório. API de leitura apenas, bloqueio de travessia e arquivos ocultos.

## Garantias
Sem editor de arquivos, sem upload remoto, sem serviços de terceiros; pesquisas indexadas por caminho e conteúdo; botões e navegação por hashes funcionais. Não confundir leitura ao vivo com monitoramento instantâneo automático.
