using System;
using System.Collections.Generic;
using UnityEngine;

namespace PirateGame.Movement{
    public class Mover : MonoBehaviour
    {
        // States of control for sails.
        private float sailStateModifier;
        private int sailState = 0;

        // State change delay.
        [SerializeField]
        private float sailStateChangeDelay = 1f;
        private float sailStateTimeSinceChanged;

        // Movement attributes.
        [SerializeField]
        private float maxSpeed = 10f;
        [SerializeField]
        private float turnSpeed = 5f;
        [SerializeField]
        private float accelerationSpeed = 3.5f;
        [SerializeField]
        private float decelerationSpeed = 1f;
        private float currentSpeed;
        private float targetSpeed;
        Vector3 velocity, desiredVelocity;
        private bool leftTurn, rightTurn;

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

            CalculateRotation();
            CalculateMovement();
        }

        private void CalculateRotation()
        {
            float leftSpeed = -turnSpeed;
            float rightSpeed = turnSpeed;
            float turnDirection = 0;

            if (leftTurn == true){
                turnDirection = leftSpeed;
            }
            if (rightTurn == true){
                turnDirection = rightSpeed;
            }

            transform.Rotate(new Vector3(0f,turnDirection,0f));
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
            targetSpeed = maxSpeed * sailStateModifier;

            if (currentSpeed < targetSpeed){
                float accelerationFactor = 1f - currentSpeed / maxSpeed;
                float adjustedAccelerationSpeed = accelerationSpeed * Mathf.Abs(accelerationFactor);
                currentSpeed += (1f + adjustedAccelerationSpeed) * Time.deltaTime;
                
            }
            if (currentSpeed > targetSpeed){
                float decelerationFactor = currentSpeed / maxSpeed;
                float adjustedDecelerationSpeed = decelerationSpeed * Mathf.Abs(decelerationFactor);
                currentSpeed -= (1f + adjustedDecelerationSpeed) * Time.deltaTime;
            }
            Mathf.Clamp(currentSpeed, maxSpeed - maxSpeed, maxSpeed);
            transform.position += transform.forward * currentSpeed * Time.deltaTime;
            Debug.Log(currentSpeed);
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
        public void LeftTurnEnable(){
            leftTurn = true;
        }
        public void LeftTurnDisable(){
            leftTurn = false;
        }
        public void RightTurnEnable(){
            rightTurn = true;
        }
        public void RightTurnDisable(){
            rightTurn = false;
        }
        
    }
}
