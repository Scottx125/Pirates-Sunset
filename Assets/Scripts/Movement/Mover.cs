using System.Collections.Generic;
using UnityEngine;

namespace PirateGame.Movement{
    [RequireComponent(typeof(Rigidbody))]
    public class Mover : MonoBehaviour
    {
        // States of control.
        private float sailStateModifier;
        private int sailState = 0;

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

        // Sail struct, names and float modifier values stored in a struct and held in an array.
        private struct SailStateStruct{
            private string sailStateName;
            private float sailStateSpeedModifier;

            public string getSailStateName => sailStateName;
            public float getSailStateSpeedModifier => sailStateSpeedModifier;

            public SailStateStruct(string sailStateEnum, float sailStateSpeedModifier){
                this.sailStateName = sailStateEnum;
                this.sailStateSpeedModifier = sailStateSpeedModifier;
            }
        }

        private SailStateStruct[] sailStateArray = new SailStateStruct[5]{
            new SailStateStruct("Reefed Sail", 0f),
            new SailStateStruct("Quater Sail", 0.25f),
            new SailStateStruct("Half Sail", 0.5f),
            new SailStateStruct("Three Quaters Sail", 0.75f),
            new SailStateStruct("Full Sail", 1f),
        };

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
            float sailSpeedImpact = maxSpeed * sailStateModifier;
            desiredVelocity = new Vector3(0f,0f, sailSpeedImpact);

            // Velocity = current rb velocity.
            velocity = rb.velocity;

            // Rate of acceleration.
            float maxSpeedChange = (acceleration + sailStateModifier) * Time.deltaTime;

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
            if (sailState < 4 && sailStateTimeSinceChanged >= sailStateChangeDelay){
                sailState++;
                sailStateModifier = sailStateArray[sailState].getSailStateSpeedModifier;
                sailStateTimeSinceChanged = 0f;
            }
        }
        public void SailStateDecrease(){
            if (sailState > 0 && sailStateTimeSinceChanged >= sailStateChangeDelay){
                sailState--;
                sailStateModifier = sailStateArray[sailState].getSailStateSpeedModifier;
                sailStateTimeSinceChanged = 0f;
            }
        }
    }
}
