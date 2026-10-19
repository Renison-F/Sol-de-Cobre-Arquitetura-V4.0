# Contrato de entrega de um personagem 3D

## Pacote por asset_id

Entregar uma pasta por identidade contendo Source (fonte Blender, controles e dependências), Runtime (FBX), Textures, Evidence (capturas e vídeo) e handoff.json na raiz. Fonte não entra em Assets; copiar apenas derivados homologados para o projeto do jogo. Não entregar só FBX sem textura/fonte nem só render sem malha. Registrar origem/autoria e licença dos recursos efetivamente incorporados.

O manifesto usa schema_version=1, asset_id do catálogo, rig_type, source_file, runtime_file, lods, deform_bones, max_influences, textures, sockets, clips, metrics_source, reviews e evidence_files. Caminhos relativos com barras /, sem saída da pasta. LODs listam level, triangles e material_slots. Texturas listam file, width e height. Contar triângulos após a triangulação de export e novamente no import; não contar quads como triângulos nem uma instância de Cascavel como o corpo inteiro.

Exemplo preenchível em `11-contratos/handoff.template.json`: valores null e reviews false são intencionais porque não existem modelos medidos nesta entrega. Não trocar por zero/true para passar no check. Copiar o template para a pasta do asset e preencher com medições reais.

## Comandos

Validar conteúdo do projeto: `python tools/validate_project.py` e `python tools/validate_production.py`.

Validar entrega do artista: `python tools/validate_model_delivery.py "C:/GameArt/CHR_MARA_VENTO/handoff.json"`.

Exigir também revisões de aceite declaradas: `python tools/validate_model_delivery.py "C:/GameArt/CHR_MARA_VENTO/handoff.json" --require-accepted`.

O validador verifica contrato, presença dos arquivos, caminhos e budgets declarados. Ele NÃO abre FBX/Blender, não mede geometria, não reconhece a identidade do rosto e não prova que as evidências são verdadeiras. O status de aceite é revisão humana com provas. A validação no Editor e no aparelho continua obrigatória.

## Aceite rastreável

| Revisão | Quem conduz | Prova necessária |
|---|---|---|
| technical_views | Direção de arte | Vistas alinhadas, medidas e ambiguidades resolvidas |
| deformation | Rig/animação | Vídeo das poses extremas e sockets em contato |
| unity_import | Programação/Tech Art | Log sem erro, materiais, Avatar/Generic e escala conferidos |
| lod_transition | Tech Art | Captura do ator na câmera com mudança de LOD |
| device_profile | QA | Aparelho, SO, build, cena, tempo de sessão e captura CPU/GPU/memória |
| origin_recorded | Produção | Autor, ferramenta e direitos dos recursos usados registrados |

Qualidade visual é aprovada por consistência com a concept e legibilidade em movimento. O check numérico não certifica beleza nem performance. Aprovar uma textura em um PC com GPU dedicada não libera a arena no Android de entrada. Material/rig compartilhado precisa de revisão depois de alterar um prefab base: uma alteração pode quebrar todas as variantes.

Não marcar a tarefa ART como concluída antes de o prefab funcionar em seu capítulo e estado de checkpoint. A Máquina da Seca inclui plataformas e resgate; Madame inclui encerramento humano e drones; o pacote de um não substitui o de outro.
