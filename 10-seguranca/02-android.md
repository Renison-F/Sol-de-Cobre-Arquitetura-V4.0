# Android — licença do aplicativo e Play Integrity

## Compra premium

A V3 vende o aplicativo, não um produto de compra interna. Não chamar Play Billing `purchases.products` para fingir validar a compra do app inteiro. O backend usa o veredito `accountDetails.appLicensingVerdict` da Play Integrity na política da distribuição Google Play. `LICENSED` indica que a conta instalou ou comprou o app pela loja; app premium deve estar de fato configurado como pago. O serviço não recebe e-mail nem identidade estável da conta Google a partir desse sinal.

## Requisição protegida

Bridge nativa prepara o provedor em segundo plano. Para ativar/renovar, calcula hash estável da operação, desafio, instalação, impressão da chave pública e hash da prova relevante. Envia o token opaco ao backend, que pede decodificação/verificação ao serviço oficial e compara o `requestHash`. Não confiar em JSON decodificado no aparelho. Não registrar tokens ou dados sensíveis no hash em texto aberto.

Validar no backend: pacote exato da configuração; request hash igual ao recalculado; timestamp dentro de 120 s com tolerância operacional explicitada; `PLAY_RECOGNIZED`; certificado SHA-256 da **chave de assinatura de distribuição**, não apenas upload key; versão pertencente ao conjunto permitido; licença `LICENSED`. Tratar rotação de certificado e versões em rollout por conjuntos com validade, sem aceitar certificados enviados livremente pelo cliente.

A API padrão já oferece proteção contra replay do token; o desafio de aplicação acrescenta vínculo de operação e idempotência no domínio. Separar esse controle de mecanismos de nonce da API clássica. Não implementar o exemplo Standard com parâmetros da Classic.

## Identidade da instalação

Na matrícula, a chave pública efêmera persistente da instalação entra no digest e é registrada somente após prova válida. Nas renovações, assinatura da requisição demonstra posse dessa chave. O ID local é aleatório e não representa uma conta Google. Nunca permitir sobrescrever uma instalação existente com outra chave sem fluxo de reativação. Limites por aparelho e contas compartilhadas não devem ser inventados a partir de `LICENSED`.

## Tratamento de erros

| Resposta | Conduta |
|---|---|
| Timeout / cota / falha transitória | Backoff com jitter; usar cache assinado existente dentro de prazo |
| `UNEVALUATED` | Não classificar como pirataria; tentar reparação e suporte; não emitir licença premium sem prova |
| `UNLICENSED` | Mostrar loja e restauração com conta correta; confirmar estado online antes de revogar |
| App/certificado não reconhecido | Negar operação de backend; orientar instalação oficial; guardar save |
| Dispositivo sem integridade forte | Avaliar política e risco; não exigir selo mais restritivo por padrão |
| Emulador de QA | Ambiente sandbox segregado, nunca exceção secreta na produção |

Prova de integridade é sinal adicional, não garantia de que todo código executado seja inviolável. Não fazer atestação por frame. Configurar cota e observar distribuição dos vereditos antes de aplicar enforcement. Versão do plugin e Android SDK devem ser travados e testados no repositório Unity real conforme documentação vigente.

## Aceite do adaptador

Instalação Play interna reconhecida; pacote adulterado negado; certificado errado negado; mudança de conta; atualização gradual; reinstalação; app legítimo sem rede; erro de cota; recuperação de chave perdida. Registrar somente status sanitizado e correlation ID. Evidências de loja não estão no ZIP porque dependem de projeto e credenciais do proprietário.
