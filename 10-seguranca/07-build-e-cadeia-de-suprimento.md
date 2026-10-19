# Build, assinatura e dependências

Release exige IL2CPP onde compatível, builds sem Development Build/Script Debugging, símbolos guardados em repositório privado de diagnóstico e arquivos de assinatura separados do código. IL2CPP aumenta custo de análise, mas não é DRM infalível. Managed stripping remove código não usado; níveis agressivos exigem testes de reflection, serialização e plugins. Ofuscador comercial é opção após medir compatibilidade e custo; não integra a V3 por nome ou licença presumida.

## Pipeline alvo

1. PR revisado: lint, testes do domínio, testes de conteúdo, dependências e segredos.
2. Runner efêmero constrói com engine e pacotes travados; sem reutilizar workspace não confiável de fork com segredo.
3. Produzir SBOM, hashes de artefatos, metadados de commit/engine/pacotes e relatório de testes.
4. Assinar em job protegido com identidade de curta duração; upload key separada da assinatura gerida pela Play; certificados e perfis Apple no cofre.
5. Distribuir primeiro para faixa interna/TestFlight. Testar instalação de loja e retorno de licença real.
6. Aprovação de release, rollout gradual, monitorar e pausar em regressão. Rollback de conteúdo usa versão ainda compatível com schema de save; não fazer downgrade destrutivo.

## Chaves distintas

Chave de upload Google, certificado de distribuição, chave Apple de API, licença KMS, conteúdo e TLS têm finalidades e ciclos diferentes. Rotação de um não muda silenciosamente os demais. A conta proprietária de loja usa MFA e recuperação guardada fora do repositório. Sem domínio, conta e projeto reais não é possível completar essa configuração no ZIP.

## Dependências

`security-reference/requirements.txt` fixa a versão usada na validação deste pacote. No repositório produtivo, gerar lockfile com hashes por plataforma, verificar advisories e reproduzir build antes de atualizar. Não afirmar que uma versão é livre de vulnerabilidades por ter passado nos testes funcionais. SDKs de loja e pacotes Unity serão fixados no projeto real, não inventados em `manifest.json` sem compilação.

## Gate objetivo

Nenhum release com segredo embutido, modo debug, mock de compra, endpoint de sandbox ativo na produção, adaptador que retorna sucesso por padrão ou vulnerabilidade crítica explorável aberta. Regressões de acesso do comprador interrompem rollout. Toda exceção tem responsável e validade; não fica como comentário esquecido no código.
