# Condutor Relâmpago — identidade de chefe

ID `BOSS-05`. Primeiro encontro: LVL-05 · O Expresso de Cobre.

## Função e visual

Personagem de confronto com silhueta própria. Arte-mestra `09-assets/personagens/condutor-relampago.png`. O turnaround conceitual mostra volume e acessórios; modelador deve produzir vistas técnicas alinhadas antes de rig. Propriedades de dano e interação pertencem à ficha `05-game-design/bosses/BOSS-05.md`.

## Dramaturgia

Oponente protege a infraestrutura que a equipe precisa recuperar. Objetivo de vitória: aterrar dois ímãs e puxar o freio de emergência após três curtos controlados. Entidade mecânica/guardião; desativação expõe manutenção e falha do sistema, sem sofrimento humano. Introdução curta exibe instrumento de ameaça e ponto de resposta antes de fechar arena.

## Rig e animações

Clipes: dormant, intro, idle, locomotion/reposition, telegraph_A, attack_A, recover_A, telegraph_B, attack_B, recover_B, exposed, phase_transition, defeated. Componentes repetidos (anéis, segmentos, bobinas, drones) têm pivôs nomeados e submáquinas independentes controladas pelo estado principal. Nenhum AnimationEvent concede vitória sozinho. Suprimir hitbox ao cancelar ou descarregar.

Budget específico V4: 22000/11000/5500 triângulos em LOD0/1/2, até 3 slots materiais e 48 ossos deformadores. Metas iniciais a validar; fonte única e exceções em `09-arte-audio/modelagem/condutor-relampago.md`.

## Aceite

Frente/lado/costas consistentes com mecanismo; área de ameaça e fraqueza legíveis por forma, som e contraste; todos ataques/recuperações da ficha têm clipe e fallback por timer; desativação permite save e saída sem travar controle.


## Pacote técnico de modelagem V4

A especificação individual de geometria, rig, sockets, animações e aceite está em `09-arte-audio/modelagem/condutor-relampago.md`. Seus budgets específicos substituem os valores genéricos acima. Fonte numérica única: `11-contratos/modelagem.json`.
