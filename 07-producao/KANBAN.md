# Kanban de implementação V4

157 tarefas · estimativa-base 7,712 horas-pessoa · reserva inicial de planejamento 30% (10,026 h). Estimativas preliminares para equipe com experiência; não equivalem a prazo ou orçamento. Ajustar após o slice, especialmente qualidade de arte/animação e QA mobile. Não somar trabalho paralelo como semanas de uma única pessoa.

A V4 detalha a sequência em `12-desenvolvimento/09-ordem-de-implementacao.md` e o aceite 3D em `09-arte-audio/modelagem/INDICE.md`, mantendo a estimativa-base. Ferramentas de referência fornecidas precisam de homologação no jogo antes de concluir DEV-02/03/25.

Todas as tarefas abaixo estão em **Backlog de produto**: a entrega de documentação e concept art não marca jogo como implementado. Colunas operacionais propostas: Backlog → Ready → Doing (limite 1 por pessoa) → Review → Device QA → Done. Bloqueio é rótulo com causa e responsável, não etapa esquecida.

| ID | Marco | Tarefa | Função | h | Depende de | Aceite |
|---|---|---|---|---|---|---|
| DEV-01 | M0 | Repositório, LFS e projeto URP baseline | Tech Lead | 12 | — | Projeto abre, versões e locks commitados |
| DEV-02 | M0 | Assemblies, CompositionRoot e fronteiras | Gameplay | 20 | DEV-01 | Domain sem UnityEngine; build compila |
| DEV-03 | M1 | Input System touch/teclado/gamepad | Gameplay | 32 | DEV-02 | Multitouch e cancelamento de foco funcionam |
| DEV-04 | M1 | Motor Mara com salto variável e coyote | Gameplay | 40 | DEV-03 | Gaps do slice passam em 30/60 FPS |
| DEV-05 | M1 | Câmera, volumes e safe framing | Gameplay | 24 | DEV-04 | Pousos visíveis nos ratios alvo |
| DEV-06 | M1 | Laço, alvo e sockets | Gameplay | 48 | DEV-04 | Alcance, cancelamento e balanço sem softlock |
| DEV-07 | M1 | Mascote Faísca e canais | Gameplay | 32 | DEV-06 | Entrar, retornar e falhar com segurança |
| DEV-08 | M1 | DTO schema3 e três slots | Gameplay | 24 | DEV-02 | Schemas e catálogos validados |
| DEV-09 | M2 | Migração, backup e I/O atômico nativo | Gameplay | 40 | DEV-08 | Crash/disco cheio preservam progresso |
| DEV-10 | M1 | Quest state e idempotência | Gameplay | 32 | DEV-02 | Eventos repetidos não duplicam prêmio |
| DEV-11 | M2 | Checkpoints e suspensão mobile | Gameplay | 32 | DEV-09, DEV-10 | Retoma mecanismos/posição em ponto seguro |
| DEV-12 | M2 | Diálogo, legenda e skip | Gameplay | 32 | DEV-10 | Mesmo estado com cena vista ou pulada |
| DEV-13 | M2 | Pool e famílias de inimigos | Gameplay | 48 | DEV-04 | 19 variantes configuráveis sem copiar IA |
| DEV-14 | M2 | Sistema de chefes por configuração | Gameplay | 40 | DEV-13 | Hitboxes canceladas e vitória única |
| DEV-15 | M3 | Zé, troca segura e impacto | Gameplay | 56 | DEV-04, DEV-06 | Troca não penetra nem perde objetivo |
| DEV-16 | M3 | Aterramento e mecanismos elétricos | Gameplay | 32 | DEV-15 | Sinal de ameaça acessível e cancelável |
| DEV-17 | M3 | Trovico e alvos aéreos | Gameplay | 40 | DEV-07 | Alcance/LOS/retorno e interrupção |
| DEV-18 | M3 | Água, pressão e válvulas | Gameplay | 40 | DEV-10 | Puzzle recupera estado local sem perder canal |
| DEV-19 | M3 | Trem e cenário em movimento relativo | Gameplay | 40 | DEV-05, DEV-13 | Sem drift de física ou câmera |
| DEV-20 | M4 | Lentes e gravidade por volume | Gameplay | 40 | DEV-05, DEV-10 | Controle preservado e restauração correta |
| DEV-21 | M2 | Menus, slots, pausa e Atlas | UI | 56 | DEV-08, DEV-12 | 12 telas e caminhos de erro navegáveis |
| DEV-22 | M2 | Opções e assistência persistidas | UI | 32 | DEV-21 | Canhoto, mix, legenda, flash e remap |
| DEV-23 | M3 | Áudio por prioridade e stems | Audio Dev | 40 | DEV-12 | Aviso audível e transição sem corte |
| DEV-24 | M3 | Catálogo de assets e carga aditiva | Gameplay | 40 | DEV-02 | Unload sem referência perdida ou pico OOM |
| DEV-25 | M2 | Ferramentas Editor de IDs e checkpoints | Tools | 32 | DEV-10, DEV-24 | Cena inválida bloqueia build de conteúdo |
| SEC-DEV-01 | M0 | Gateway, projeto sandbox e cofre/KMS | Backend | 40 | DEV-01 | Sem segredo em repo/cliente; TLS e papéis |
| SEC-DEV-02 | M1 | Principal de instalação e posse de chave | Backend | 48 | SEC-DEV-01 | Instalação A não redefine chave de B |
| SEC-DEV-03 | M2 | Android bridge e verificação Play Integrity | Platform | 64 | SEC-DEV-02 | Compra/app/certificado/hash/tempo verificados |
| SEC-DEV-04 | M2 | iOS bridge AppTransaction + App Attest | Platform | 80 | SEC-DEV-02 | Cadeia, bundle, ambiente, contador e challenge |
| SEC-DEV-05 | M2 | Persistência de concessão e idempotência/outbox | Backend | 48 | SEC-DEV-02 | Crash/replay/corrida em duas instâncias |
| SEC-DEV-06 | M2 | Verificador Unity interoperável e storage | Platform | 40 | SEC-DEV-03, SEC-DEV-04 | Vetores Python/C# e IL2CPP no aparelho |
| SEC-DEV-07 | M2 | Fluxos ativar/renovar/restaurar e UX offline | UI | 40 | DEV-21, SEC-DEV-06 | Cache e erro preservam campanha |
| SEC-DEV-08 | M5 | Manifesto assinado e atualização de conteúdo | Backend | 32 | DEV-24, SEC-DEV-01 | Bundle adulterado não é promovido |
| SEC-DEV-09 | M5 | Operação, métricas e recovery drill | SRE | 40 | SEC-DEV-05 | Restore banco, cota e fornecedor indisponível |
| SEC-DEV-10 | M6 | Pentest backend e binário de release | Security | 80 | SEC-DEV-06, SEC-DEV-09 | Achados corrigidos/retestados; gate registrado |
| LVL-01-GRAY | M2 | Greybox e checkpoints — Estação Pôr do Sol | Level Design | 48 | DEV-14 | Seis seções, rota principal e opcionais recuperáveis |
| LVL-01-BOSS | M2 | Confronto Fiscal de Trilhos | Gameplay | 40 | LVL-01-GRAY, DEV-14 | Dois ataques específicos e fases da ficha |
| LVL-01-ART | M2 | Kit e lighting — Estação Pôr do Sol | Environment | 100 | LVL-01-GRAY | Kit modular, materiais, LOD e budget no aparelho |
| LVL-01-NAR | M2 | Diálogo e encenação EP-01 | Narrative | 24 | LVL-01-GRAY, DEV-12 | Falas/flags e skip sem contradição |
| LVL-01-QA | M2 | QA completo — Estação Pôr do Sol | QA | 24 | LVL-01-BOSS, LVL-01-ART, LVL-01-NAR | 30/60 FPS, morte, save, replay e assistência |
| LVL-02-GRAY | M3 | Greybox e checkpoints — Cânion dos Mil Ecos | Level Design | 48 | LVL-01-GRAY | Seis seções, rota principal e opcionais recuperáveis |
| LVL-02-BOSS | M3 | Confronto Cascavel de Silício | Gameplay | 40 | LVL-02-GRAY, DEV-14 | Dois ataques específicos e fases da ficha |
| LVL-02-ART | M5 | Kit e lighting — Cânion dos Mil Ecos | Environment | 100 | LVL-02-GRAY | Kit modular, materiais, LOD e budget no aparelho |
| LVL-02-NAR | M3 | Diálogo e encenação EP-02 | Narrative | 24 | LVL-02-GRAY, DEV-12 | Falas/flags e skip sem contradição |
| LVL-02-QA | M5 | QA completo — Cânion dos Mil Ecos | QA | 24 | LVL-02-BOSS, LVL-02-ART, LVL-02-NAR | 30/60 FPS, morte, save, replay e assistência |
| LVL-03-GRAY | M3 | Greybox e checkpoints — Minas de Ferrugem | Level Design | 48 | LVL-02-GRAY | Seis seções, rota principal e opcionais recuperáveis |
| LVL-03-BOSS | M3 | Confronto Britador Colosso | Gameplay | 40 | LVL-03-GRAY, DEV-14 | Dois ataques específicos e fases da ficha |
| LVL-03-ART | M5 | Kit e lighting — Minas de Ferrugem | Environment | 100 | LVL-03-GRAY | Kit modular, materiais, LOD e budget no aparelho |
| LVL-03-NAR | M3 | Diálogo e encenação EP-03 | Narrative | 24 | LVL-03-GRAY, DEV-12 | Falas/flags e skip sem contradição |
| LVL-03-QA | M5 | QA completo — Minas de Ferrugem | QA | 24 | LVL-03-BOSS, LVL-03-ART, LVL-03-NAR | 30/60 FPS, morte, save, replay e assistência |
| LVL-04-GRAY | M3 | Greybox e checkpoints — Cidade Fantasma | Level Design | 48 | LVL-03-GRAY | Seis seções, rota principal e opcionais recuperáveis |
| LVL-04-BOSS | M3 | Confronto Maquinista Sem Rosto | Gameplay | 40 | LVL-04-GRAY, DEV-14 | Dois ataques específicos e fases da ficha |
| LVL-04-ART | M5 | Kit e lighting — Cidade Fantasma | Environment | 100 | LVL-04-GRAY | Kit modular, materiais, LOD e budget no aparelho |
| LVL-04-NAR | M3 | Diálogo e encenação EP-04 | Narrative | 24 | LVL-04-GRAY, DEV-12 | Falas/flags e skip sem contradição |
| LVL-04-QA | M5 | QA completo — Cidade Fantasma | QA | 24 | LVL-04-BOSS, LVL-04-ART, LVL-04-NAR | 30/60 FPS, morte, save, replay e assistência |
| LVL-05-GRAY | M4 | Greybox e checkpoints — O Expresso de Cobre | Level Design | 48 | LVL-04-GRAY | Seis seções, rota principal e opcionais recuperáveis |
| LVL-05-BOSS | M4 | Confronto Condutor Relâmpago | Gameplay | 40 | LVL-05-GRAY, DEV-14 | Dois ataques específicos e fases da ficha |
| LVL-05-ART | M5 | Kit e lighting — O Expresso de Cobre | Environment | 100 | LVL-05-GRAY | Kit modular, materiais, LOD e budget no aparelho |
| LVL-05-NAR | M4 | Diálogo e encenação EP-05 | Narrative | 24 | LVL-05-GRAY, DEV-12 | Falas/flags e skip sem contradição |
| LVL-05-QA | M5 | QA completo — O Expresso de Cobre | QA | 24 | LVL-05-BOSS, LVL-05-ART, LVL-05-NAR | 30/60 FPS, morte, save, replay e assistência |
| LVL-06-GRAY | M4 | Greybox e checkpoints — Oásis das Vozes | Level Design | 48 | LVL-05-GRAY | Seis seções, rota principal e opcionais recuperáveis |
| LVL-06-BOSS | M4 | Confronto Guardião dos Canais | Gameplay | 40 | LVL-06-GRAY, DEV-14 | Dois ataques específicos e fases da ficha |
| LVL-06-ART | M5 | Kit e lighting — Oásis das Vozes | Environment | 100 | LVL-06-GRAY | Kit modular, materiais, LOD e budget no aparelho |
| LVL-06-NAR | M4 | Diálogo e encenação EP-06 | Narrative | 24 | LVL-06-GRAY, DEV-12 | Falas/flags e skip sem contradição |
| LVL-06-QA | M5 | QA completo — Oásis das Vozes | QA | 24 | LVL-06-BOSS, LVL-06-ART, LVL-06-NAR | 30/60 FPS, morte, save, replay e assistência |
| LVL-07-GRAY | M4 | Greybox e checkpoints — Serra dos Relâmpagos | Level Design | 48 | LVL-06-GRAY | Seis seções, rota principal e opcionais recuperáveis |
| LVL-07-BOSS | M4 | Confronto General Para-Raios | Gameplay | 40 | LVL-07-GRAY, DEV-14 | Dois ataques específicos e fases da ficha |
| LVL-07-ART | M5 | Kit e lighting — Serra dos Relâmpagos | Environment | 100 | LVL-07-GRAY | Kit modular, materiais, LOD e budget no aparelho |
| LVL-07-NAR | M4 | Diálogo e encenação EP-07 | Narrative | 24 | LVL-07-GRAY, DEV-12 | Falas/flags e skip sem contradição |
| LVL-07-QA | M5 | QA completo — Serra dos Relâmpagos | QA | 24 | LVL-07-BOSS, LVL-07-ART, LVL-07-NAR | 30/60 FPS, morte, save, replay e assistência |
| LVL-08-GRAY | M4 | Greybox e checkpoints — Observatório Perdido | Level Design | 48 | LVL-07-GRAY | Seis seções, rota principal e opcionais recuperáveis |
| LVL-08-BOSS | M4 | Confronto Oráculo de Latão | Gameplay | 40 | LVL-08-GRAY, DEV-14 | Dois ataques específicos e fases da ficha |
| LVL-08-ART | M5 | Kit e lighting — Observatório Perdido | Environment | 100 | LVL-08-GRAY | Kit modular, materiais, LOD e budget no aparelho |
| LVL-08-NAR | M4 | Diálogo e encenação EP-08 | Narrative | 24 | LVL-08-GRAY, DEV-12 | Falas/flags e skip sem contradição |
| LVL-08-QA | M5 | QA completo — Observatório Perdido | QA | 24 | LVL-08-BOSS, LVL-08-ART, LVL-08-NAR | 30/60 FPS, morte, save, replay e assistência |
| LVL-09-GRAY | M4 | Greybox e checkpoints — Cidadela Ferrugem | Level Design | 48 | LVL-08-GRAY | Seis seções, rota principal e opcionais recuperáveis |
| LVL-09-BOSS | M4 | Confronto Madame Engrenagem | Gameplay | 40 | LVL-09-GRAY, DEV-14 | Dois ataques específicos e fases da ficha |
| LVL-09-ART | M5 | Kit e lighting — Cidadela Ferrugem | Environment | 100 | LVL-09-GRAY | Kit modular, materiais, LOD e budget no aparelho |
| LVL-09-NAR | M4 | Diálogo e encenação EP-09 | Narrative | 24 | LVL-09-GRAY, DEV-12 | Falas/flags e skip sem contradição |
| LVL-09-QA | M5 | QA completo — Cidadela Ferrugem | QA | 24 | LVL-09-BOSS, LVL-09-ART, LVL-09-NAR | 30/60 FPS, morte, save, replay e assistência |
| LVL-10-GRAY | M4 | Greybox e checkpoints — Coração da Tempestade | Level Design | 48 | LVL-09-GRAY | Seis seções, rota principal e opcionais recuperáveis |
| LVL-10-BOSS | M4 | Confronto Barão Ferrugem | Gameplay | 40 | LVL-10-GRAY, DEV-14 | Dois ataques específicos e fases da ficha |
| LVL-10-ART | M5 | Kit e lighting — Coração da Tempestade | Environment | 100 | LVL-10-GRAY | Kit modular, materiais, LOD e budget no aparelho |
| LVL-10-NAR | M4 | Diálogo e encenação EP-10 | Narrative | 24 | LVL-10-GRAY, DEV-12 | Falas/flags e skip sem contradição |
| LVL-10-QA | M5 | QA completo — Coração da Tempestade | QA | 24 | LVL-10-BOSS, LVL-10-ART, LVL-10-NAR | 30/60 FPS, morte, save, replay e assistência |
| CHR-01-MODEL | M2 | Malha, UV, texturas e LOD — Mara Vento | 3D Artist | 64 | DEV-01 | Prancha resolvida em vistas técnicas e budget |
| CHR-01-RIG | M2 | Rig, clips e prefab — Mara Vento | Animator | 56 | CHR-01-MODEL | Clips da ficha, sockets, collider, import e device QA |
| CHR-02-MODEL | M5 | Malha, UV, texturas e LOD — Zé Trovão | 3D Artist | 64 | DEV-01 | Prancha resolvida em vistas técnicas e budget |
| CHR-02-RIG | M5 | Rig, clips e prefab — Zé Trovão | Animator | 56 | CHR-02-MODEL | Clips da ficha, sockets, collider, import e device QA |
| CHR-03-MODEL | M2 | Malha, UV, texturas e LOD — Faísca | 3D Artist | 64 | DEV-01 | Prancha resolvida em vistas técnicas e budget |
| CHR-03-RIG | M2 | Rig, clips e prefab — Faísca | Animator | 56 | CHR-03-MODEL | Clips da ficha, sockets, collider, import e device QA |
| CHR-04-MODEL | M5 | Malha, UV, texturas e LOD — Trovico | 3D Artist | 64 | DEV-01 | Prancha resolvida em vistas técnicas e budget |
| CHR-04-RIG | M5 | Rig, clips e prefab — Trovico | Animator | 56 | CHR-04-MODEL | Clips da ficha, sockets, collider, import e device QA |
| CHR-05-MODEL | M5 | Malha, UV, texturas e LOD — Luna Fagulha | 3D Artist | 64 | DEV-01 | Prancha resolvida em vistas técnicas e budget |
| CHR-05-RIG | M5 | Rig, clips e prefab — Luna Fagulha | Animator | 56 | CHR-05-MODEL | Clips da ficha, sockets, collider, import e device QA |
| CHR-06-MODEL | M5 | Malha, UV, texturas e LOD — Isadora Veredas | 3D Artist | 64 | DEV-01 | Prancha resolvida em vistas técnicas e budget |
| CHR-06-RIG | M5 | Rig, clips e prefab — Isadora Veredas | Animator | 56 | CHR-06-MODEL | Clips da ficha, sockets, collider, import e device QA |
| CHR-07-MODEL | M5 | Malha, UV, texturas e LOD — Barão Ferrugem | 3D Artist | 64 | DEV-01 | Prancha resolvida em vistas técnicas e budget |
| CHR-07-RIG | M5 | Rig, clips e prefab — Barão Ferrugem | Animator | 56 | CHR-07-MODEL | Clips da ficha, sockets, collider, import e device QA |
| CHR-08-MODEL | M2 | Malha, UV, texturas e LOD — Fiscal de Trilhos | 3D Artist | 80 | DEV-01 | Prancha resolvida em vistas técnicas e budget |
| CHR-08-RIG | M2 | Rig, clips e prefab — Fiscal de Trilhos | Animator | 64 | CHR-08-MODEL | Clips da ficha, sockets, collider, import e device QA |
| CHR-09-MODEL | M5 | Malha, UV, texturas e LOD — Cascavel de Silício | 3D Artist | 80 | DEV-01 | Prancha resolvida em vistas técnicas e budget |
| CHR-09-RIG | M5 | Rig, clips e prefab — Cascavel de Silício | Animator | 64 | CHR-09-MODEL | Clips da ficha, sockets, collider, import e device QA |
| CHR-10-MODEL | M5 | Malha, UV, texturas e LOD — Britador Colosso | 3D Artist | 80 | DEV-01 | Prancha resolvida em vistas técnicas e budget |
| CHR-10-RIG | M5 | Rig, clips e prefab — Britador Colosso | Animator | 64 | CHR-10-MODEL | Clips da ficha, sockets, collider, import e device QA |
| CHR-11-MODEL | M5 | Malha, UV, texturas e LOD — Maquinista Sem Rosto | 3D Artist | 80 | DEV-01 | Prancha resolvida em vistas técnicas e budget |
| CHR-11-RIG | M5 | Rig, clips e prefab — Maquinista Sem Rosto | Animator | 64 | CHR-11-MODEL | Clips da ficha, sockets, collider, import e device QA |
| CHR-12-MODEL | M5 | Malha, UV, texturas e LOD — Condutor Relâmpago | 3D Artist | 80 | DEV-01 | Prancha resolvida em vistas técnicas e budget |
| CHR-12-RIG | M5 | Rig, clips e prefab — Condutor Relâmpago | Animator | 64 | CHR-12-MODEL | Clips da ficha, sockets, collider, import e device QA |
| CHR-13-MODEL | M5 | Malha, UV, texturas e LOD — Guardião dos Canais | 3D Artist | 80 | DEV-01 | Prancha resolvida em vistas técnicas e budget |
| CHR-13-RIG | M5 | Rig, clips e prefab — Guardião dos Canais | Animator | 64 | CHR-13-MODEL | Clips da ficha, sockets, collider, import e device QA |
| CHR-14-MODEL | M5 | Malha, UV, texturas e LOD — General Para-Raios | 3D Artist | 80 | DEV-01 | Prancha resolvida em vistas técnicas e budget |
| CHR-14-RIG | M5 | Rig, clips e prefab — General Para-Raios | Animator | 64 | CHR-14-MODEL | Clips da ficha, sockets, collider, import e device QA |
| CHR-15-MODEL | M5 | Malha, UV, texturas e LOD — Oráculo de Latão | 3D Artist | 80 | DEV-01 | Prancha resolvida em vistas técnicas e budget |
| CHR-15-RIG | M5 | Rig, clips e prefab — Oráculo de Latão | Animator | 64 | CHR-15-MODEL | Clips da ficha, sockets, collider, import e device QA |
| CHR-16-MODEL | M5 | Malha, UV, texturas e LOD — Madame Engrenagem | 3D Artist | 80 | DEV-01 | Prancha resolvida em vistas técnicas e budget |
| CHR-16-RIG | M5 | Rig, clips e prefab — Madame Engrenagem | Animator | 64 | CHR-16-MODEL | Clips da ficha, sockets, collider, import e device QA |
| ENM-01-ASSET | M5 | Malha/rig/prefab da variante ENM-01 | 3D Artist | 32 | DEV-13 | Visual da célula correta e comportamento da família |
| ENM-02-ASSET | M5 | Malha/rig/prefab da variante ENM-02 | 3D Artist | 32 | DEV-13 | Visual da célula correta e comportamento da família |
| ENM-03-ASSET | M5 | Malha/rig/prefab da variante ENM-03 | 3D Artist | 32 | DEV-13 | Visual da célula correta e comportamento da família |
| ENM-04-ASSET | M5 | Malha/rig/prefab da variante ENM-04 | 3D Artist | 32 | DEV-13 | Visual da célula correta e comportamento da família |
| ENM-05-ASSET | M5 | Malha/rig/prefab da variante ENM-05 | 3D Artist | 32 | DEV-13 | Visual da célula correta e comportamento da família |
| ENM-06-ASSET | M5 | Malha/rig/prefab da variante ENM-06 | 3D Artist | 32 | DEV-13 | Visual da célula correta e comportamento da família |
| ENM-07-ASSET | M5 | Malha/rig/prefab da variante ENM-07 | 3D Artist | 32 | DEV-13 | Visual da célula correta e comportamento da família |
| ENM-08-ASSET | M5 | Malha/rig/prefab da variante ENM-08 | 3D Artist | 32 | DEV-13 | Visual da célula correta e comportamento da família |
| ENM-09-ASSET | M5 | Malha/rig/prefab da variante ENM-09 | 3D Artist | 32 | DEV-13 | Visual da célula correta e comportamento da família |
| ENM-10-ASSET | M5 | Malha/rig/prefab da variante ENM-10 | 3D Artist | 32 | DEV-13 | Visual da célula correta e comportamento da família |
| ENM-11-ASSET | M5 | Malha/rig/prefab da variante ENM-11 | 3D Artist | 32 | DEV-13 | Visual da célula correta e comportamento da família |
| ENM-12-ASSET | M5 | Malha/rig/prefab da variante ENM-12 | 3D Artist | 32 | DEV-13 | Visual da célula correta e comportamento da família |
| ENM-13-ASSET | M5 | Malha/rig/prefab da variante ENM-13 | 3D Artist | 32 | DEV-13 | Visual da célula correta e comportamento da família |
| ENM-14-ASSET | M5 | Malha/rig/prefab da variante ENM-14 | 3D Artist | 32 | DEV-13 | Visual da célula correta e comportamento da família |
| ENM-15-ASSET | M5 | Malha/rig/prefab da variante ENM-15 | 3D Artist | 32 | DEV-13 | Visual da célula correta e comportamento da família |
| ENM-16-ASSET | M5 | Malha/rig/prefab da variante ENM-16 | 3D Artist | 32 | DEV-13 | Visual da célula correta e comportamento da família |
| ENM-17-ASSET | M5 | Malha/rig/prefab da variante ENM-17 | 3D Artist | 32 | DEV-13 | Visual da célula correta e comportamento da família |
| ENM-18-ASSET | M5 | Malha/rig/prefab da variante ENM-18 | 3D Artist | 32 | DEV-13 | Visual da célula correta e comportamento da família |
| ENM-19-ASSET | M5 | Malha/rig/prefab da variante ENM-19 | 3D Artist | 32 | DEV-13 | Visual da célula correta e comportamento da família |
| SIDE-01 | M4 | Implementar e testar opcional S01 | Level Design | 16 | LVL-01-GRAY, DEV-10 | Aceitar/adiar/replay sem bloquear final |
| SIDE-02 | M4 | Implementar e testar opcional S02 | Level Design | 16 | LVL-02-GRAY, DEV-10 | Aceitar/adiar/replay sem bloquear final |
| SIDE-03 | M4 | Implementar e testar opcional S03 | Level Design | 16 | LVL-03-GRAY, DEV-10 | Aceitar/adiar/replay sem bloquear final |
| SIDE-04 | M4 | Implementar e testar opcional S04 | Level Design | 16 | LVL-04-GRAY, DEV-10 | Aceitar/adiar/replay sem bloquear final |
| SIDE-05 | M4 | Implementar e testar opcional S05 | Level Design | 16 | LVL-05-GRAY, DEV-10 | Aceitar/adiar/replay sem bloquear final |
| SIDE-06 | M4 | Implementar e testar opcional S06 | Level Design | 16 | LVL-06-GRAY, DEV-10 | Aceitar/adiar/replay sem bloquear final |
| SIDE-07 | M4 | Implementar e testar opcional S07 | Level Design | 16 | LVL-07-GRAY, DEV-10 | Aceitar/adiar/replay sem bloquear final |
| SIDE-08 | M4 | Implementar e testar opcional S08 | Level Design | 16 | LVL-08-GRAY, DEV-10 | Aceitar/adiar/replay sem bloquear final |
| PROD-01 | M2 | Revisão técnica de pranchas e escala | Art Lead | 40 | DEV-04 | Turnarounds técnicos consistentes para modelagem |
| PROD-02 | M5 | Máquina da Seca: modelo modular e animação | 3D Artist | 160 | DEV-14 | Anéis, válvulas e três estados seguros |
| PROD-03 | M5 | Três famílias de figurantes e crowd | 3D Artist | 96 | DEV-24 | Rig compartilhável, 3 variantes e teto de crowd |
| PROD-04 | M5 | Composição e produção de trilha/stems | Composer | 200 | LVL-01-GRAY | Motivos originais e atribuições |
| PROD-05 | M5 | SFX, vocalizações e mix | Sound | 120 | DEV-23 | Avisos e interações consistentes no aparelho |
| PROD-06 | M5 | Texto final PT-BR e tabela de localização | Narrative | 64 | LVL-10-NAR | IDs únicos e teste de expansão de texto |
| QA-01 | M2 | Testes externos do slice | QA | 32 | LVL-01-QA | Observação e ajustes priorizados |
| QA-02 | M6 | Campanha completa e migração de saves | QA | 80 | LVL-10-QA, DEV-09 | Início ao fim, upgrade e recovery |
| QA-03 | M6 | Matriz de dispositivos e thermal soak | QA | 80 | LVL-10-ART | Evidência por aparelho e perfil |
| QA-04 | M6 | Acessibilidade e controles por usuários | QA | 48 | DEV-22 | Problemas críticos corrigidos |
| REL-01 | M2 | CI de build, símbolos e assinatura | Build | 48 | DEV-01 | Release reproduzível sem debug/mocks |
| REL-02 | M6 | Direitos de assets, privacidade e loja | Product | 48 | PROD-06 | Vendedor/contato reais e documentação revisada |
| REL-03 | M6 | Candidato Android/iOS, restore e rollout | Release | 48 | QA-02, QA-03, QA-04, SEC-DEV-10, REL-02 | Gates assinados e monitoramento ativo |

Done exige revisão e evidência real, sem TODO funcional, ausência de softlock e testes de erro adequados. Operação contínua, suporte, custos de loja/nuvem e marketing não estão convertidos em horas finitas de implementação; precisam de orçamento separado do proprietário.
