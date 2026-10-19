# Briefing de implementação para Claude ou Codex

Use o texto abaixo ao abrir o repositório real. O contexto do projeto deve estar versionado no repositório; um link de conversa sozinho não é contrato técnico.

```text
Você vai implementar Sol de Cobre a partir da documentação V4.0, preservando o cânone e as pranchas atuais, incluindo Isadora Veredas. Antes de editar, leia AGENTS.md se existir, README, 12-desenvolvimento/00-COMECE-AQUI.md, 08-bootstrap-local-v4.md, 09-ordem-de-implementacao.md, 10-modulos-e-contratos.md e 00-governanca/07-matriz-de-cobertura.md. Inspecione o repositório real e informe a diferença entre o kit de referência e código de jogo já existente.

Comece pelo incremento A e prove compilação no Unity 6000.3.24f1 ou registre concretamente qual runtime falta. Preserve manifest/lock/ProjectVersion reais. Migre referências por diff: não duplique _Game antigo e novo em Assets. Use os asmdefs fornecidos. QuestState.Ready recebe flags; QuestRules adapta QuestDefinition.

Gere o laboratório com ProjectBootstrap.CreateLab, abra a cena e valide andar/saltar/câmera/input de teclado e gamepad. Ele não implementa o slice nem touch. Depois avance B/C: touch, laço, Faísca, estados e cancelamento. Faça mudanças pequenas e rastreáveis com arquivos, critérios de aceite, comandos e resultados de teste. Não trocar o projeto por outro protótipo isolado nem reescrever documentação para chamar stub de implementação pronta.

Save legado é schema1; o contrato alvo é schema3. Implementar DTO, migração, slots e recuperação com fixtures, sem apagar save ao falhar. Licença e backend seguem 10-seguranca; sem provas de compra falsas no release. Não colocar segredos no cliente, no repositório ou em logs.

Para 3D, consumir 11-contratos/modelagem.json e fichas de 09-arte-audio/modelagem. Produzir primeiro Mara/Faísca/Fiscal em blockout. Não confundir PNG com FBX nem declarar rigs/modelos finais sem arquivos e testes. Scenes/prefabs podem ser gerados por Editor scripts; FBX/Blender podem usar automação quando a ferramenta existe, com revisão humana de forma/deformação.

Não publicar, assinar release nem enviar mensagens externas sem instrução do proprietário. Não deixar uma execução bloqueada sem diagnóstico: informe ferramenta/requisito ausente, arquivo afetado e comando local para retomar. Entregue o incremento com estado real, não promessa de jogo completo. Não marque backlog produtivo concluído por haver apenas texto ou C# não compilado.
```

## Resposta esperada por incremento

Arquivos alterados e motivo; comportamento que já funciona; testes executados e evidências; limitações concretas; tarefa seguinte e pré-condição. Não estimar “concluído” usando porcentagem arbitrária de arquivos. Quando o Editor não estiver disponível, continuar nas partes independentes e fornecer o passo exato para homologação local.
