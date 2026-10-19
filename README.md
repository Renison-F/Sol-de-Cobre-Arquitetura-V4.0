# Sol de Cobre — Arquitetura Viva V4.0

Expansão do pacote V2 enviado: Game Bible, roteiro, segurança comercial, arte do elenco e guia de implementação, com o portal existente preservado e ampliado.

## Atualização 4.0

Kit de produção: 17 fichas individuais de modelagem, metas para os 19 inimigos e três famílias de figurantes, budgets em JSON/CSV, guia Blender → FBX → Unity e contrato de entrega com validador. Referências C# reorganizadas em quatro assemblies; ponte do Input System, gerador de laboratório de movimento/cena/prefab e testes EditMode. Guia de bootstrap local, ordem de implementação e briefing para Claude/Codex. As 22 imagens permanecem idênticas à V3.3.

No portal, começar em **Iniciar desenvolvimento** e **Pipeline de arte**. Ler `12-desenvolvimento/08-bootstrap-local-v4.md` e `09-arte-audio/modelagem/INDICE.md`. O laboratório ainda precisa ser gerado/compilado no seu Unity e não equivale ao slice completo.

## Atualização 3.3

Personagem renomeada para **Isadora Veredas** na prancha, no portal, nas fichas, nos catálogos e nas referências de produção. A relação de mãe de Mara permanece no cânone. A revisão altera o nome e o título da prancha; as cinco legendas de detalhes seguem o padrão V3.2. Prompt e procedência em `09-arte-audio/prompts-e-proveniencia-v33.md`.

## Atualização 3.2

Dez pranchas restantes da lista solicitada foram uniformizadas com Mara e os demais personagens da revisão anterior, incluindo Luna, os chefes e a Máquina da Seca. A galeria tem 21 entradas únicas após retirar o cartão redundante da dupla. Todas as 17 pranchas individuais com quadros de detalhes passam a usar legendas em português nesses quadros, incluindo as sete pranchas anteriores. Os prompts desta revisão estão em `09-arte-audio/prompts-e-proveniencia-v32.md`.

## Atualização 3.1

As pranchas de Mara, Zé, Faísca e Trovico foram refeitas no padrão horizontal dos personagens novos. As dez fases do mapa agora estão centradas na mesma trilha, com nomes completos e navegação lateral em telas pequenas.

## Abrir

Extraia o ZIP inteiro e abra `index.html`. Funciona offline com o snapshot documental incorporado. Para leitura dos arquivos reais e atualização da árvore:

```bash
python serve.py
```

Abra `http://127.0.0.1:8765/`. Servidor somente local e somente leitura. Não é serviço comercial do jogo. Após editar os documentos, atualize o snapshot com `python tools/reindex.py`.

## Começar

No portal, abra **Iniciar desenvolvimento** e **Segurança comercial**. O guia `12-desenvolvimento/00-COMECE-AQUI.md` define bootstrap, decisões, slice e insumos externos. O Kanban tem tarefas, horas, dependências e critérios de aceite.

## Conteúdo

- Campanha de 10 capítulos com 60 sequências específicas, objetivos, checkpoints, falas, revelações e encerramento.
- 7 personagens centrais e 9 chefes adicionais, 19 variantes de inimigos comuns e 3 famílias de figurantes.
- 22 arquivos conceituais no acervo e 21 entradas únicas na galeria; dez pranchas refeitas e legendas padronizadas nas 17 pranchas individuais, com fichas, animações requeridas, budgets e pipeline de modelagem.
- Segurança: modelo de ameaças, licença premium, Play Integrity, AppTransaction/App Attest, prazo offline, chaves, backend, build, conteúdo, privacidade, incidentes e testes.
- Núcleo criptográfico Python executável; interfaces e verificador SC1 em C#; OpenAPI, SQL, JSON schemas, catálogos e diálogo CSV.

## Estado real

Esta entrega prepara o desenvolvimento. Não é jogo executável, projeto Unity completo, modelo 3D rigado, trilha final ou backend comercial implantado. Python foi testado conforme relatório; o C# não foi compilado em Unity/IL2CPP neste ambiente. Adaptadores oficiais, contas/credenciais do proprietário, modelos e testes em dispositivos são trabalho concreto do backlog.

Nenhum jogo local é impossível de crackear. A arquitetura dificulta adulteração e protege decisões do servidor; não promete impedir todo patch no aparelho. Primeira ativação requer rede. A política V3 usa licença de 30 dias + 7 dias de tolerância, renovada oportunisticamente; essa escolha altera a premissa totalmente offline da V2.

## Verificar

```bash
python tools/validate_project.py
python -m py_compile serve.py tools/reindex.py
node --check dashboard/js/app.js
```

Para o núcleo de segurança, entre em `security-reference`, instale `requirements.txt` em um ambiente virtual e execute `python -m unittest discover -s tests -v`. `08-qualidade/RELATORIO-V4.md` registra os checks desta revisão; `08-qualidade/RELATORIO-V3.md` preserva o histórico anterior.

Versão 4.0 · 19/10/2026. Bibliotecas/fontes do portal mantêm licenças em `dashboard/vendor`. Produção comercial exige registrar origem e direitos dos recursos finais usados.
