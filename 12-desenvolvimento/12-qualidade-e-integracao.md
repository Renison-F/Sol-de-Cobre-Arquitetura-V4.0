# Plano de testes e evidência de integração

| Camada | Teste | Prova arquivada |
|---|---|---|
| Catálogos | IDs, refs, ciclos, budgets e resumo CSV | Saída dos validadores + commit |
| Domínio | Flags idempotentes, snapshot isolado, sessão e prazo | XML EditMode no Unity; testes Python de segurança são outra suíte |
| Motor/input | Buffer/coyote, salto curto/longo, gamepad desconectado, multitouch e foco | PlayMode e vídeo no aparelho |
| Mundo | Alvo some, prefab ausente, loading cancelado, checkpoint após boss | Cena de regressão e log legível |
| Save | Corrupção, versão futura, backup, falta de espaço e concorrência | Fixtures versionadas e relatórios sem dados privados |
| Arte | Deformação, LOD, textura, Avatar, sockets e bounds | Manifesto de entrega e captura da câmera real |
| Performance | Pico de cena/boss, água/transparência, aquecimento e retomada | Captura CPU/GPU/memória com modelo do aparelho e build |
| Segurança | Prova de loja, assinatura, vínculo e revogação | Sandbox real e matriz da seção Segurança comercial |

## Primeira execução local

Rodar os quatro testes EditMode incluídos depois da compilação: idempotência, isolamento do snapshot, transição inválida e janela de licença. Eles não cobrem a verificação RSA inteira nem provam o comportamento dos MonoBehaviours. Acrescentar PlayMode para motor/câmera e teste em aparelhos para touch/IL2CPP. Não chamar de 100% validado porque os catálogos ou o domínio passaram.

Na nuvem sem Unity, os checks possíveis são Python/documentos, integridade de arquivos e grafo de assemblies. Registrar explicitamente a ausência de compilação C#/execução de cena. Quando existir Editor no runner, CI executa na ordem: lint → EditMode → PlayMode → build de desenvolvimento → smoke em aparelho. Release/assinatura usam pipeline separado; o BuildLabAndroid entregue gera apenas demonstração interna.

Capturas devem incluir hash de commit, versão do Editor/pacotes, aparelho, SO, perfil de qualidade, duração e cena. Medir no cenário caro inteiro, não só a malha isolada. Não aprovar Android a partir da RTX do computador de desenvolvimento. Para iOS, reservar macOS/Xcode e aparelho antes de prometer data de loja.
