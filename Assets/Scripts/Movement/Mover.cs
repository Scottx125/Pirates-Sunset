using UnityEngine;

namespace PirateGame.Movement{
    [RequireComponent(typeof(Rigidbody))]
    public class Mover : MonoBehaviour
    {
        // States of control.
        private float sailState;

        // State change delay.
        [SerializeField]
        private float sailStateChangeDelay = 1f;
        private float sailStateTimeSinceChanged;

        // Movement attributes.
        [SerializeField]
        private float maxSpeed;
        [SerializeField]
        private float maxTurnSpeed;
        [SerializeField]
        private float acceleration;
        Vector3 velocity, desiredVelocity;

        // Other Scripts/Componenets
        Rigidbody rb;

        private void Awake()
        {
            Setup();
        }

        private void Setup()
        {
            // Allow the player to fire immediately.
            sailStateTimeSinceChanged = 1f;

            if (rb == null) rb = GetComponent<Rigidbody>();
        }

        public void FixedUpdate(){
            SailStateTimer();

            CalculateMovement();
        }

        private void SailStateTimer()
        {
            if (sailStateTimeSinceChanged >= sailStateChangeDelay){
                sailStateTimeSinceChanged = 1f;
                return;
            }
            sailStateTimeSinceChanged += Time.deltaTime;
        }

        private void CalculateMovement()
        {
            
            // Set desired speed based on sailstate and input.
            float sailSpeedImpact = maxSpeed * sailState;
            desiredVelocity = new Vector3(0f,0f, sailSpeedImpact);

            // Velocity = current rb velocity.
            velocity = rb.velocity;

            // Rate of acceleration.
            float maxSpeedChange = (acceleration + sailState) * Time.deltaTime;

            // Moves the velocity values towards the desiredVelocity at the max speed change.
		    velocity.x =
			    Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
		    velocity.z =
			    Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

            // Set the current rb velocity to the new velocity.
            rb.velocity = velocity;
            Debug.Log(rb.velocity);
        }

        public void SailStateIncrease(){
            if (sailState < 1f && sailStateTimeSinceChanged >= sailStateChangeDelay){
                sailState += .25f;
                sailStateTimeSinceChanged = 0f; 
            }
        }
        public void SailStateDecrease(){
            if (sailState > 0f && sailStateTimeSinceChanged >= sailStateChangeDelay){
                sailState -= .25f;
                sailStateTimeSinceChanged = 0f; 
            }
        }
    }
}
