# General Para-Raios — identidade de chefe

ID `BOSS-07`. Primeiro encontro: LVL-07 · Serra dos Relâmpagos.

## Função e visual

Personagem de confronto com silhueta própria. Arte-mestra `09-assets/personagens/general-para-raios.png`. O turnaround conceitual mostra volume e acessórios; modelador deve produzir vistas técnicas alinhadas antes de rig. Propriedades de dano e interação pertencem à ficha `05-game-design/bosses/BOSS-07.md`.

## Dramaturgia

Oponente protege a infraestrutura que a equipe precisa recuperar. Objetivo de vitória: desviar três descargas às hastes e desconectar as bobinas do General. Entidade mecânica/guardião; desativação expõe manutenção e falha do sistema, sem sofrimento humano. Introdução curta exibe instrumento de ameaça e ponto de resposta antes de fechar arena.

## Rig e animações

Clipes: dormant, intro, idle, locomotion/reposition, telegraph_A, attack_A, recover_A, telegraph_B, attack_B, recover_B, exposed, phase_transition, defeated. Componentes repetidos (anéis, segmentos, bobinas, drones) têm pivôs nomeados e submáquinas independentes controladas pelo estado principal. Nenhum AnimationEvent concede vitória sozinho. Suprimir hitbox ao cancelar ou descarregar.

Budget específico V4: 25000/12500/6250 triângulos em LOD0/1/2, até 3 slots materiais e 52 ossos deformadores. Metas iniciais a validar; fonte única e exceções em `09-arte-audio/modelagem/general-para-raios.md`.

## Aceite

Frente/lado/costas consistentes com mecanismo; área de ameaça e fraqueza legíveis por forma, som e contraste; todos ataques/recuperações da ficha têm clipe e fallback por timer; desativação permite save e saída sem travar controle.


## Pacote técnico de modelagem V4

A especificação individual de geometria, rig, sockets, animações e aceite está em `09-arte-audio/modelagem/general-para-raios.md`. Seus budgets específicos substituem os valores genéricos acima. Fonte numérica única: `11-contratos/modelagem.json`.
