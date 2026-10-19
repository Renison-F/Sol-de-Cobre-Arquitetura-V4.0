# Fiscal de Trilhos — identidade de chefe

ID `BOSS-01`. Primeiro encontro: LVL-01 · Estação Pôr do Sol.

## Função e visual

Personagem de confronto com silhueta própria. Arte-mestra `09-assets/personagens/fiscal-de-trilhos.png`. O turnaround conceitual mostra volume e acessórios; modelador deve produzir vistas técnicas alinhadas antes de rig. Propriedades de dano e interação pertencem à ficha `05-game-design/bosses/BOSS-01.md`.

## Dramaturgia

Oponente protege a infraestrutura que a equipe precisa recuperar. Objetivo de vitória: prender o carimbo em três blocos de carga e puxar a trava dorsal. Entidade mecânica/guardião; desativação expõe manutenção e falha do sistema, sem sofrimento humano. Introdução curta exibe instrumento de ameaça e ponto de resposta antes de fechar arena.

## Rig e animações

Clipes: dormant, intro, idle, locomotion/reposition, telegraph_A, attack_A, recover_A, telegraph_B, attack_B, recover_B, exposed, phase_transition, defeated. Componentes repetidos (anéis, segmentos, bobinas, drones) têm pivôs nomeados e submáquinas independentes controladas pelo estado principal. Nenhum AnimationEvent concede vitória sozinho. Suprimir hitbox ao cancelar ou descarregar.

Budget específico V4: 20000/10000/5000 triângulos em LOD0/1/2, até 3 slots materiais e 40 ossos deformadores. Metas iniciais a validar; fonte única e exceções em `09-arte-audio/modelagem/fiscal-de-trilhos.md`.

## Aceite

Frente/lado/costas consistentes com mecanismo; área de ameaça e fraqueza legíveis por forma, som e contraste; todos ataques/recuperações da ficha têm clipe e fallback por timer; desativação permite save e saída sem travar controle.


## Pacote técnico de modelagem V4

A especificação individual de geometria, rig, sockets, animações e aceite está em `09-arte-audio/modelagem/fiscal-de-trilhos.md`. Seus budgets específicos substituem os valores genéricos acima. Fonte numérica única: `11-contratos/modelagem.json`.
