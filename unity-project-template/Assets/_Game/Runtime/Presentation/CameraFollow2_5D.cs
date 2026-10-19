using UnityEngine;
namespace SolDeCobre.Presentation
{
    public sealed class CameraFollow2_5D : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] Vector3 offset = new Vector3(0,1,-11);
        [SerializeField] float smoothTime = .19f;
        Vector3 velocity;
        void LateUpdate()
        {
            if(target == null) return;
            Vector3 desired = new Vector3(target.position.x,target.position.y,0)+offset;
            transform.position=Vector3.SmoothDamp(transform.position,desired,ref velocity,smoothTime);
        }
    }
}
