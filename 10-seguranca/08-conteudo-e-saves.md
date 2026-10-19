# Conteúdo, assets e saves

## Distribuição

Manifesto de conteúdo assinado por chave distinta da licença contém versão de catálogo, versão mínima de cliente, ID estável, tamanho, SHA-256 e endereço allowlisted de cada bundle. Cliente valida assinatura antes de confiar nos hashes. Baixar em temporário, impor tamanho e quota, validar hash e só então fazer promoção atômica. Não executar código baixado; atualização da lógica passa pelo mecanismo autorizado da loja.

URLs temporárias para conteúdo remoto reduzem hotlink e expõem apenas aquele objeto por poucos minutos. Não esconder segredo estático no aplicativo. Criptografia de assets, se adotada, dificulta extração casual, mas a chave e o conteúdo precisam estar acessíveis à execução legítima e podem ser extraídos por atacante privilegiado. Não comercializar isso como garantia antipirataria.

## Save

Formato versionado com `schema_version`, slot, revisão, capítulo/checkpoint, herói, quests, coletáveis, opções e `written_at`. Limite 1 MiB por slot no projeto alvo. Validar IDs pelo catálogo, números finitos e intervalos, não confiar em tipo de objeto serializado do usuário. Nunca desserializar tipos arbitrários ou usar BinaryFormatter.

Escrita: construir snapshot imutável, validar, escrever `.tmp`, flush, mover atomicamente, manter backup anterior; em falha recuperar a versão íntegra mais recente. Checksum detecta corrupção acidental; HMAC local só dificulta edição casual se a chave também está no aparelho. Não confundir checksum com autenticidade comercial. Licença fica fora do save.

Migração usa cópia, valida schema novo, só promove após sucesso; versão futura desconhecida oferece preservação/exportação e orientação, nunca reset silencioso. Repetir evento não duplica recompensa. Um checkpoint restaura posição segura e estado de mecanismos; não salva objeto volátil de física nem referências de cena.

## Nuvem futura

Fora do escopo de lançamento V3. Se incluída, isolamento por principal, revisão para conflito, limite de payload, recuperação de exclusão e escolha explícita entre progresso local/remoto. Não aceitar save enviado como prova de propriedade ou de compra. Não haverá ranking baseado em estado offline editável.
