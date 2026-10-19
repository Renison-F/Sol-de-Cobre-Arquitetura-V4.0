using System;
using UnityEngine;
using SolDeCobre.Gameplay;
namespace SolDeCobre.World
{
    public enum EnemyFamily { Patrol, Hover, Dive, Turret }

    // Inimigo comum dirigido por EnemyThreatCycle (ameaça Prepare/Active/Recover + vida).
    // Agressão exige alvo em raio de 8 m e linha de visão. Dano só na janela Active.
    // Derrota publica o ID uma vez. Movimento das famílias simples (patrol/hover) aqui;
    // dive/turret entram parados e recebem movimento próprio ao serem produzidos.
    public sealed class CommonEnemy : MonoBehaviour
    {
        [SerializeField] string enemyId = "ENM-01";
        [SerializeField] EnemyFamily family = EnemyFamily.Hover;
        [SerializeField] Transform target;
        [SerializeField] LayerMask lineOfSightBlockers = ~0;
        [SerializeField] int integrity = 1;
        [SerializeField] float warning = 1f, active = 0.5f, recover = 1.5f;
        [SerializeField] float aggroRadius = 8f, moveSpeed = 2f;
        [SerializeField] float patrolHalfWidth = 3f, hoverAmplitude = 0.5f, hoverSpeed = 2f;

        EnemyThreatCycle cycle;
        Vector3 origin;
        int patrolDir = 1;

        public event Action<string> Defeated;
        public string EnemyId => enemyId;
        public bool DealsDamageNow => cycle != null && cycle.DealsDamageNow;
        public float DamageAmount => 1f;

        void Awake()
        {
            cycle = new EnemyThreatCycle(integrity, warning, active, recover);
            origin = transform.position;
            cycle.Activate();
        }

        public void Freeze(bool value) => cycle.Freeze(value);
        public void SetAssist(bool value) => cycle.SetAssist(value);
        public void ResetToNeutral() => cycle.ResetNeutral(); // morte do herói reseta a seção

        // Resposta válida do herói (laço/impacto no ponto fraco).
        public void TakeValidHit()
        {
            if (cycle.TakeValidHit() && cycle.TryPublishDefeat())
                Defeated?.Invoke(enemyId); // desativa dano e devolve ao pool após o efeito (view)
        }

        void Update()
        {
            float dt = Time.deltaTime;
            cycle.SetAggro(TargetInSight());
            cycle.Tick(dt);
            Move(dt);
        }

        bool TargetInSight()
        {
            if (target == null) return false;
            Vector3 to = target.position - transform.position;
            if (to.sqrMagnitude > aggroRadius * aggroRadius) return false;
            // Linha de visão: nada bloqueando entre inimigo e alvo.
            return !Physics.Raycast(transform.position, to.normalized, to.magnitude, lineOfSightBlockers,
                QueryTriggerInteraction.Ignore);
        }

        void Move(float dt)
        {
            switch (family)
            {
                case EnemyFamily.Patrol: // vai e volta no segmento; não cai da plataforma
                    var p = transform.position;
                    p.x += patrolDir * moveSpeed * dt;
                    if (p.x > origin.x + patrolHalfWidth) { p.x = origin.x + patrolHalfWidth; patrolDir = -1; }
                    else if (p.x < origin.x - patrolHalfWidth) { p.x = origin.x - patrolHalfWidth; patrolDir = 1; }
                    transform.position = p;
                    break;
                case EnemyFamily.Hover: // flutua em faixa limitada e volta à origem
                    var h = origin;
                    h.y = origin.y + Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude;
                    transform.position = h;
                    break;
                // Dive/Turret: entram parados; trajetória de mergulho e resfriamento de torre
                // recebem movimento próprio na produção da variante (ver BESTIARIO.md).
            }
        }
    }
}
