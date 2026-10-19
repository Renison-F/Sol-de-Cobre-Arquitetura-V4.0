# Arquitetura técnica V4 — fronteiras implementáveis

## Camadas

Domínio C# puro: estado de campanha, quests, contagem de objetivos, políticas de checkpoint e licença. Aplicação: casos de uso StartSession, LoadLevel, CompleteObjective, SaveCheckpoint, RestorePurchase e RenewLicense. Adaptadores Unity: motor, colisores, Input System, Animator, áudio, render, UI e assets. Infraestrutura: arquivo, secure storage, bridge nativa, HTTP e serviços de loja. Conteúdo serializado configura regras e nunca recebe segredos.

Dependência aponta para contratos do domínio. Presentation não altera flags diretamente; emite intenção. Infrastructure não chama Canvas. ScriptableObjects são definições imutáveis; estado mutável fica em instâncias de sessão, separado para não vazar entre cenas ou testes. Evitar Service Locator global e singleton para cada subsistema; CompositionRoot monta dependências.

## Módulos e ownership

| Módulo | Autoridade | Entradas | Saídas |
|---|---|---|---|
| Core | relógio, sessão, cancelamento | lifecycle | eventos tipados |
| Gameplay | movimento, dano, habilidade | input + mundo | resultado e apresentação |
| World | carregamento, checkpoints, mecanismos | LevelId | cena pronta e destinos seguros |
| Narrative | objetivos, diálogo e cutscene | eventos validados | flags idempotentes |
| Progression | snapshot, slots e migração | estado de domínio | save/recovery |
| Presentation | HUD, menus, câmera, animação, som | estado observável | intenções do usuário |
| Security | autorização comercial | prova nativa, token, relógio | modo de acesso |
| Infrastructure | disco, rede, loja e KMS remoto | contratos | resultados tipados |

## Fluxo de inicialização

Bootstrap carrega settings e registro de assets → valida versão de save sem sobrescrever → verifica licença cacheada → menu. Jogar usa gate da sessão → carrega nível/essenciais → resolve checkpoint e entidades → aplica snapshot → enquadra câmera → habilita input. Timeout cancela e volta ao menu preservando save. Renovação oportunista ocorre fora do loop de física.

## Assemblies alvo

`SolDeCobre.Domain` sem UnityEngine; `Application` referencia Domain; `UnityAdapters` referencia Application e Unity; `SecurityAdapters` referencia contratos e plugins homologados; `EditorTools` só Editor; testes EditMode para domínio e PlayMode para adaptadores. A V4 entrega quatro asmdefs reais: Domain, Runtime, Editor e testes EditMode. O alvo de Application/UnityAdapters/SecurityAdapters acima é evolução da divisão atual; não há assemblies vazios com esses nomes. QuestState recebe flags puras, com QuestRules como ponte do ScriptableObject no Runtime. A homologação local da separação continua na tarefa DEV-02. Consulte `12-desenvolvimento/10-modulos-e-contratos.md`.

## Eventos e persistência

Envelope local `event_id`, `type`, `entity_id`, `sequence`, `occurred_tick` e payload tipado. Idempotência de objetivo pelo par quest/flag; save tem revisão crescente. UI assina e desassina ao entrar/sair; evitar handlers acumulados. Nenhuma animação é fonte exclusiva de conclusão de missão. Falha de I/O retorna resultado de recuperação, não campanha nova silenciosa.

## Segurança

Compra e prova verificadas fora do cliente; licença assinada validada no cliente para continuidade local; backend rejeita operações não autenticadas independentemente de UI modificada. `security-reference` é núcleo executável de referência, não backend implantável. C# inclui verificador SC1 e política, mas falta compilação/interoperabilidade Unity.

## Tecnologia baseline

Unity 6.3 LTS 6000.3.24f1 para primeiro bootstrap, C#, URP 3D, Input System, cenas aditivas por capítulo, Addressables apenas se carga e dependências justificarem. Package/SDK locks saem do Editor e dos plugins oficiais em etapa rastreada; não há manifest falso prometendo build já validado. `12-desenvolvimento/00-COMECE-AQUI.md` traz fonte e procedimento.
