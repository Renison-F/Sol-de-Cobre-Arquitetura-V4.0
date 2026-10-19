using UnityEngine;
namespace SolDeCobre.Gameplay
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerMotor2_5D : MonoBehaviour
    {
        [SerializeField] HeroDefinition hero;
        [SerializeField] float coyoteTime=0.12f,jumpBuffer=0.15f,acceleration=30f,deceleration=40f;
        CharacterController controller;
        float verticalSpeed,horizontal,velocityX,lastGround=-100f,jumpAt=-100f,planeZ;
        bool jumpReleased;
        void Awake() { controller=GetComponent<CharacterController>();planeZ=transform.position.z; }
        public void SetHorizontal(float axis) { horizontal=Mathf.Clamp(axis,-1f,1f); }
        public void QueueJump() { jumpAt=Time.time;jumpReleased=false; }
        public void ReleaseJump() { jumpReleased=true; }
        public void ClearInput() { horizontal=0f;jumpAt=-100f;jumpReleased=false; }
        void OnDisable() { ClearInput(); }
        void OnApplicationFocus(bool focus) { if(!focus) ClearInput(); }
        void Update()
        {
            if(hero==null || Time.deltaTime<=0f) return;
            float dt=Time.deltaTime;
            if(controller.isGrounded && verticalSpeed<=0f) { lastGround=Time.time;verticalSpeed=-1f; }
            if(Time.time-jumpAt<=jumpBuffer && Time.time-lastGround<=coyoteTime)
            {
                verticalSpeed=Mathf.Sqrt(hero.jumpHeight*-2f*hero.gravity);
                jumpAt=-100f;lastGround=-100f;
            }
            if(jumpReleased && verticalSpeed>0f) { verticalSpeed*=0.5f;jumpReleased=false; }
            velocityX=Mathf.MoveTowards(velocityX,horizontal*hero.moveSpeed,
                (Mathf.Abs(horizontal)>0.01f?acceleration:deceleration)*dt);
            verticalSpeed=Mathf.Max(verticalSpeed+hero.gravity*dt,-40f);
            Vector3 motion=new Vector3(velocityX,verticalSpeed,(planeZ-transform.position.z)/dt);
            var collision=controller.Move(motion*dt);
            if((collision&CollisionFlags.Above)!=0 && verticalSpeed>0f) verticalSpeed=0f;
        }
    }
}
