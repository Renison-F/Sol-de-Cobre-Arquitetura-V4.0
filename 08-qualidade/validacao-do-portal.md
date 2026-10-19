# Validação do portal V3

O relatório vigente está em `08-qualidade/RELATORIO-V3.md`. Há testes de servidor e integração de DOM reproduzíveis, sem alegação de renderização em browser nesta versão. A fonte original V2 não é evidência de layout da V3.

Roteiro visual de homologação: abrir desktop 1600 px e mobile 390 px; todas as áreas; tema claro/escuro; galeria e lightbox; leitura de documentos longos e tabelas; busca, navegação por teclado e foco de modais; árvore grande e filtros; ausência de rolagem horizontal indevida; imagens sem distorção e cabeçalhos sem colisão. Em servidor, criar/editar Markdown e atualizar árvore. Em file://, abrir sem rede e confirmar snapshot.

Testes de segurança do servidor estão em `tools/test_docs_server.py`; não expor servidor local na internet.
