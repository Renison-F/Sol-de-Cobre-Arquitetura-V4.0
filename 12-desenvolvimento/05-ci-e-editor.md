# CI, editor e validação de conteúdo

Editor tools a implementar: catálogo de IDs duplicados, validação de pré-condições de quests, relatório de assets sem origem/licença, gizmos de alcance de laço/mascote, preview de câmera por aspect ratio, auditoria de colliders, máscaras de dano, orçamento por cena e checklist de checkpoint alcançável. Não inventar engine própria ou editor de mundo paralelo.

Pipeline no repositório real: content lint → testes do domínio → EditMode → PlayMode críticos → build Android interno → smoke em device → iOS/TestFlight em runner macOS → relatório de aprovação. Segredos e jobs de assinatura só em branch protegida. PR de fork sem credencial de publicação. Lockfiles e versão do editor são parte do artefato.

O pacote inclui `tools/validate_project.py` para verificar catálogos, IDs, referências de arte e presença de documentos; `tools/reindex.py` gera snapshot offline. São ferramentas documentais, não verificadores de cenas Unity ainda inexistentes. `serve.py` é servidor local de documentação e nunca deve ser usado como serviço de licenças.

Testes de código de domínio focam invariantes: concluir objetivo uma vez, não avançar sem pré-condição, restaurar checkpoint completo, persistência não destruir backup, assinatura não aceitar alteração e acesso de instalação A não afetar B. Testes que apenas confirmam que um método chama outro não substituem esses comportamentos.


## Automação incluída na V4

ProjectBootstrap gera cena/prefab/input de um laboratório e um APK Development desse laboratório, quando executado no Editor real. tools/run_unity.py compõe a chamada local. tools/validate_production.py confere contratos de produção e grafo de assemblies; tools/test_model_delivery.py cobre o validador de handoff. Consulte `12-desenvolvimento/08-bootstrap-local-v4.md`. O validador documental não virou compilador de C#.
