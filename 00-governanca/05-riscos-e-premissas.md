# Registro de riscos de produto

| ID | Risco | Gravidade | Mitigação |
|---|---|---|---|
| R1 | Rig de personagens ultrapassa orçamento mobile | Alta | Silhuetas simplificadas, LODs e perfilado |
| R2 | Controle touch ocupa tela | Alta | UI adaptativa, opção de remapeamento |
| R3 | Cada fase cria sistema irreutilizável | Alta | Puzzle primitives e sequenciamento por dados |
| R4 | História cria softlocks no save | Alta | Event bus tipado, flags idempotentes e validação |
| R5 | Aventura 10 fases ultrapassa orçamento | Crítica | Vertical slice antes da produção em massa |
| R6 | Confundir concept art com asset 3D | Média | Pipeline 2D → modelagem → rig → Unity com aprovação |
| R7 | IP e licenças inconsistentes | Alta | Registro de fonte/licença por recurso |

Premissas: jogo single-player offline; story-driven; sem microtransações no MVP; equipe pequena e pipeline terceirizável.
