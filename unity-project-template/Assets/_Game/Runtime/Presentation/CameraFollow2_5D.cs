using UnityEngine;
namespace SolDeCobre.Presentation
{
    // Uma câmera principal acompanha o plano XY; Z serve à composição (camera-2-5d.md).
    // Dead zone horizontal, look-ahead no sentido do movimento e amortecimento.
    // A projeção (perspectiva FOV 35° ou orto) e a distância calibrada ficam no
    // componente Camera, definidos pelo gerador/cena após validar proporções.
    public sealed class CameraFollow2_5D : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] Vector3 offset = new Vector3(0, 1, -11);
        [SerializeField] float smoothTime = .18f;      // amortecimento 0,18 s
        [SerializeField] float lookAhead = 2f;         // look-ahead horizontal 2 m
        [SerializeField] float deadZoneWidth = 1.2f;   // dead zone 1,2 m (largura total)
        [SerializeField] float lookAheadSmooth = .35f;

        Vector3 velocity;
        float focusX, lookAheadX, lookAheadVel, lastTargetX;
        bool initialized;

        public void SetTarget(Transform value)
        {
            target = value;
            initialized = false;
        }

        void LateUpdate()
        {
            if (target == null) return;
            float dt = Time.deltaTime;
            if (dt <= 0f) return;
            float tx = target.position.x;
            if (!initialized)
            {
                focusX = tx; lastTargetX = tx; lookAheadX = 0f; initialized = true;
            }

            // Dead zone: só desloca o foco quando o alvo sai da banda central.
            float half = deadZoneWidth * .5f;
            float delta = tx - focusX;
            if (delta > half) focusX = tx - half;
            else if (delta < -half) focusX = tx + half;

            // Look-ahead no sentido do movimento recente do alvo, suavizado.
            float moved = tx - lastTargetX;
            lastTargetX = tx;
            float desiredLookAhead = Mathf.Abs(moved) > 0.0001f ? Mathf.Sign(moved) * lookAhead : lookAheadX;
            lookAheadX = Mathf.SmoothDamp(lookAheadX, desiredLookAhead, ref lookAheadVel, lookAheadSmooth);

            Vector3 desired = new Vector3(focusX + lookAheadX, target.position.y, 0f) + offset;
            transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);
        }
    }
}
