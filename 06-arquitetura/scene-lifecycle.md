# Ciclo de vida e carregamento de cenas

**Boot → MainMenu → Hub/Map → LevelLoading → InLevel → Pause → Results → Map.** Loading prepara pools, texto e áudio sem bloquear thread principal. Ao entrar, serviços resolvem flags, checkpoint e hero selecionado.

**Fallback:** falha de asset direciona ao menu com mensagem compreensível e mantém save válido. SceneTransition só avança quando dependências reportam readiness; UI exibe progresso real ou indeterminado, nunca inventado.
