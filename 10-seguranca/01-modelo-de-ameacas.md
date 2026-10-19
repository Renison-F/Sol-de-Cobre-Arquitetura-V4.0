# Modelo de ameaças e controles

## Ativos e adversários

Ativos: receita, licença, chaves de assinatura e publicação, integridade do catálogo, progresso, disponibilidade e confiança do comprador. Adversários: usuário casual copiando pacote; revendedor que modifica IL2CPP e assets; automação que repete provas; atacante de backend; dependência comprometida; operador com privilégio excessivo. Celular desbloqueado está fora da confiança de hardware garantida.

| ID | Ameaça e fronteira | Controle verificável | Risco residual |
|---|---|---|---|
| SEC-01 | APK reempacotado entra no serviço | Validar pacote, certificado, versão reconhecida e licença na resposta oficial | Cliente pode ser modificado para executar conteúdo local |
| SEC-02 | Compra ou licença falsificada | Loja verificada no servidor; assinatura fixa e allowlist de chaves públicas | Fraude na conta da loja segue políticas da plataforma |
| SEC-03 | Replay ou corrida de ativação | Desafio de uso único 120 s, vínculo da operação e consumo atômico | Relay em tempo real exige detecção complementar |
| SEC-04 | Licença copiada para outro aparelho | Identificador + chave por instalação; renovação autenticada | Aparelho comprometido pode extrair estado e patchar cliente |
| SEC-05 | Relógio volta para prolongar licença | Âncora de tempo confiável, monotonicidade dentro da sessão, reconexão quando necessário | Relógio e âncora locais são contornáveis por atacante privilegiado |
| SEC-06 | API de outro jogador / IDOR | Derivar instalação do principal autenticado, nunca do ID enviado | Falha de autorização exige teste de integração dedicado |
| SEC-07 | Prova inválida contamina revogação | Eventos de loja autenticados, idempotentes e correlacionados | Atrasos de eventos e consulta offline |
| SEC-08 | Roubo de chave de distribuição | KMS/HSM, assinatura separada, MFA resistente a phishing, revisão de release | Comprometimento do operador autorizado |
| SEC-09 | Bundle/CDN adulterado | Manifesto assinado, hashes SHA-256 por arquivo e versão mínima | Hash sem assinatura não prova autoria; extração local segue possível |
| SEC-10 | Destruição ou corrupção do save | Escrita atômica, backup anterior, migração transacional, limites de tamanho | Usuário pode editar progresso individual offline |
| SEC-11 | Dependência ou CI comprometido | Versões fixas, lockfiles, SBOM, revisão de update, runner efêmero | Auditoria não elimina vulnerabilidade desconhecida |
| SEC-12 | DDoS / custo de atestação | Rate limit por instalação e rede, limites de corpo, timeout, circuit breaker | Ataque distribuído exige operação e orçamento |
| SEC-13 | Suporte vira bypass universal | Ferramenta com RBAC, justificativa, concessão limitada e auditoria | Engenharia social; revisão operacional |
| SEC-14 | Backup de chave de aparelho clonado | Chave não exportável quando possível; reativação após reinstall | Nem todo hardware dá mesmas garantias |
| SEC-15 | Portal lê arquivos externos | Confinamento de caminhos em API e rotas estáticas; bloquear symlinks e Host externo | Servidor não deve ser exposto em rede pública |

## Política de decisão

Compra não comprovada impede emissão nova; sinais inconclusivos pedem nova tentativa. Root/jailbreak, emulador ou ausência de integridade forte isoladamente não autorizam banimento ou remoção de progresso. Provas inválidas não geram licença de tolerância nova. Negar operação de backend não implica apagar campanha. Nenhum controle destrutivo, coleta invasiva, detecção que encerra ferramentas do usuário ou mecanismo de vigilância faz parte do projeto.

## Critério para início e publicação

O modelo permite iniciar a implementação do slice. Publicação exige exercitar cada fronteira com respostas reais de sandbox, binários assinados, revisão independente e evidências anexadas ao release. Aprovação documental não deve aparecer como teste executado.
