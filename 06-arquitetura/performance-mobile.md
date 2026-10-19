# Orçamento técnico para dispositivos móveis

**Metas preliminares para medição, não garantias:** 30 FPS base, 60 FPS opcional; profiler por fase; p95 de frame time apropriado à meta (33,3 ms ou 16,7 ms); budget de memória definido após seleção de dispositivos mínimos.

- URP com redução de efeitos em perfil básico, iluminação baked sempre que viável.
- Occlusion/culling, LOD de personagem, GPU instancing e atlases onde fizer sentido.
- Object pooling para FX, projéteis e pequenos inimigos.
- Sem alocações por frame no hot path; medir física e scripts no Profiler.
- Texturas importadas por perfil Android/iOS e ASTC quando suportado.
- Validar input latency e consumo térmico após sessões longas.
