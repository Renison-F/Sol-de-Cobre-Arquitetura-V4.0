# iOS — AppTransaction, App Attest e restauração

## Responsabilidades diferentes

`AppTransaction` contém informação assinada pela App Store sobre a compra do aplicativo. `App Attest` estabelece confiança em uma instância que acessa o servidor. Atestação não prova pagamento; prova de compra não substitui vínculo de requisição. A V3 combina os controles quando disponíveis e mantém os verificadores separados.

## Compra e ambiente

Bridge StoreKit obtém os dados assinados da compra do app. Backend verifica a assinatura e cadeia pelas bibliotecas oficiais adequadas ao formato, bundle ID, identidade do aplicativo e ambiente de distribuição. Não aceitar JWS apenas decodificado nem confiar em `environment` do corpo HTTP. Sandbox/TestFlight e produção têm políticas separadas. Não enviar segredo de App Store Connect ou chave privada ao Unity.

Para sistemas sem API de compra escolhida, implementar caminho oficialmente suportado de validação e sua matriz de testes antes de anunciar compatibilidade. A versão mínima alvo do projeto é iOS 16, como escolha de escopo inicial; a integração nativa ainda precisa ser homologada em dispositivos e toolchain atuais. Não inserir um retorno `true` em plataformas não suportadas.

## Matrícula App Attest

1. Verificar suporte, gerar chave via serviço nativo e pedir desafio aleatório ao servidor.
2. Obter objeto de atestação vinculado a `clientDataHash`. Servidor valida CBOR, cadeia até a raiz Apple apropriada, nonce, identificador de aplicação, chave, contador inicial e ambiente segundo o procedimento oficial.
3. Guardar chave pública verificada e associação à instalação. Não reutilizar dados de sandbox na produção.
4. Nas operações seguintes, validar assertion com corpo da requisição e desafio, assinatura, RP ID, contador crescente e challenge da operação. Atualizar contador de forma atômica com controle de concorrência.
5. Reinstalação ou perda da chave exige matrícula nova e restauração de compra, preservando o save recuperável.

O protocolo deve serializar asserts por chave no cliente; no backend, comparação de contador não pode ser um read-modify-write sem transação. Pedidos concorrentes que chegam fora de ordem retornam erro recuperável com novo desafio.

## Suporte indisponível ou erro

Falha transitória usa licença assinada existente até seu limite. Dispositivo sem suporte a App Attest pode receber licença de campanha após validação autêntica de compra, marcada como confiança reduzida no backend; serviços sensíveis permanecem limitados e não há ranking na V3. Uma assinatura inválida não recebe esse fallback. Essa distinção precisa de testes negativos.

## Restaurar

Ação explícita de restaurar atualiza dados da loja pelo fluxo oficial e pede licença nova para esta instalação. Não cobrar novamente automaticamente. Compra premium iOS e Android são independentes; transferência entre lojas e nuvem não fazem parte do escopo inicial. Não usar conta própria obrigatória para uma campanha individual. Suporte recebe somente código de diagnóstico, nunca token ou recibo cru.

## Aceite

App comprado, instalação de teste, ambiente incorreto, certificado inválido, assertion repetida, contador fora de ordem, chave perdida, suporte indisponível e upgrade do sistema. Nenhuma declaração de certificação de segurança é feita antes desses ensaios.
