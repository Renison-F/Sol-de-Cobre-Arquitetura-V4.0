# Ordem de implementação e critério de cada incremento

O pacote mantém as 157 tarefas estimadas. Os incrementos abaixo organizam esse backlog; não são horas extras somadas novamente. A duração depende de equipe, experiência, assets externos e disponibilidade de aparelhos. Não esperar modelar todo o elenco para começar gameplay.

| Incremento | IDs principais | Produção concreta | Como aceitar |
|---|---|---|---|
| A — bootstrap | DEV-01, DEV-02 | Projeto URP, locks, assemblies e laboratório gerado | Editor compila e EditMode executa com XML; nenhum segredo no cliente |
| B — sensação | DEV-03, DEV-04, DEV-05 | Input touch/teclado/gamepad, movimento e câmera | Pessoa joga com os três métodos; foco/pausa não deixam comando preso |
| C — interação | DEV-06, DEV-07 | Laço, argolas, Faísca e túnel | Cancelamento e alvo fora de alcance não travam; mascote retorna ao dono |
| D — persistência | DEV-08, DEV-09, DEV-10, DEV-11 | DTO schema3, três slots, migração e checkpoints | Falha de disco/corrupção não apaga save; restaurar repõe mundo e objetivos |
| E — encontro | DEV-13, DEV-14 e LVL-01 | Duas famílias simples e Fiscal configurado | Três ciclos de contra-ataque; vitória e saída idempotentes |
| F — apresentação | PROD-01 e tarefas de arte do trio | Mara/Faísca/Fiscal, trecho da Estação, HUD/áudio | Aceite dos assets no aparelho e leitura da ameaça em perfil baixo |
| G — acesso comercial | SEC-DEV e REL-01 | Prova real de loja sandbox, licença e renovação | Build de teste verifica instalação, prazo e recuperação sem bypass de release |
| H — slice externo | QA-01, LVL-01-QA | Trecho de 8–12 minutos com começo/fim/save | Sessão observada e gate técnico da V3; relatório de falhas e ajustes |

A Estação completa do roteiro tem duração planejada maior que o trecho de slice. Recortar poço, uma sequência de laço, túnel de Faísca e encontro condensado com o Fiscal, preservando objetivos de teste. Não reduzir todo o roteiro de 22 minutos sem uma decisão de design registrada. O laboratório gerado do incremento A não satisfaz H.

Depois de H: Zé e troca segura; Trovico; puzzles de pressão/energia; trem; campanha inteira em graybox; arte/áudio final; regressão e release candidate. Cada subsistema deve provar recuperação de checkpoint antes de produzir muitas fases dependentes. Uma melhoria da arte não pode quebrar um checkpoint estável.

## Definição de uma tarefa concluída

Código revisado, comportamento observável, erro tratado, teste apropriado, integração em cena e evidência de build/commit. Arte exige fonte + derivado + aceite de deformação e câmera. Documentação escrita ou código não compilado não equivalem a tarefa de jogo concluída. O trabalho desta V4 prepara e fornece referências; as tarefas produtivas continuam em backlog até homologação no Unity.
