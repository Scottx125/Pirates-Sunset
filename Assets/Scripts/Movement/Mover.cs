using System;
using UnityEngine;

namespace PirateGame.Movement{
    public class Mover : MonoBehaviour, IUpdateDamagedModifier
    {
        // States of control for sails.
        private float _sailStateModifier;
        private int _sailState = 0;

        // State change delay.
        private float _sailStateChangeDelay;
        private float _sailStateTimeSinceChanged;

        // Movement attributes.
        private float _maxSpeed, _minSpeed;
        private float _maxTurnSpeed, _minTurnSpeed;
        private float _maxAccelerationRate, _accelerationEasingFactor, _minAcceleration;
        private float _maxDecelerationRate, _decelerationEasingFactor, _minDeceleration;
        private float _hullDamageModifier = 1f;
        private float _sailDamageModifier = 1f;
        private float _crewDamageModifier = 1f;
        private float _currentSpeed;
        private float _targetSpeed;
        private bool _leftTurn, _rightTurn;

        public void Setup(MoverDataStruct moverDataStruct)
        {
            // Allow the player to fire immediately.
            _sailStateTimeSinceChanged = 1f;

            // Setup of ship variables.
            _sailStateChangeDelay = moverDataStruct.GetSailStateChangeDelay;
            _maxSpeed = moverDataStruct.GetMaxSpeed;
            _minSpeed = moverDataStruct.GetMinSpeed;
            _maxTurnSpeed = moverDataStruct.GetMaxTurnSpeed; 
            _minTurnSpeed = moverDataStruct.GetMinTurnSpeed;
            _maxAccelerationRate = moverDataStruct.GetMaxAccelerationRate; 
            _accelerationEasingFactor = moverDataStruct.GetAccelerationEasingFactor; 
            _minAcceleration = moverDataStruct.GetMinAccelration;
            _maxDecelerationRate = moverDataStruct.GetMaxDecelerationRate;
            _decelerationEasingFactor = moverDataStruct.GetDecelerationEasingFactor; 
            _minDeceleration = moverDataStruct.GetMinDeceleration;
        }

        // Increase/decrease ship sail state.
        public void SailStateIncrease(){
            if (_sailState < getSailStateEnumLength() - 1 && _sailStateTimeSinceChanged >= _sailStateChangeDelay){
                _sailState++;
                _sailStateModifier = getSailStateEnumValue();
                _sailStateTimeSinceChanged = 0f;
            }
        }
        public void SailStateDecrease(){
            if (_sailState > 0 && _sailStateTimeSinceChanged >= _sailStateChangeDelay){
                _sailState--;
                _sailStateModifier = getSailStateEnumValue();
                _sailStateTimeSinceChanged = 0f;
            }
        }

        // Turning enabled/disabled methods.
        public void LeftTurnEnable(){
            _leftTurn = true;
        }
        public void LeftTurnDisable(){
            _leftTurn = false;
        }
        public void RightTurnEnable(){
            _rightTurn = true;
        }
        public void RightTurnDisable(){
            _rightTurn = false;
        }

        // Update the modifiers for ship variables such as speed based on whatever health pool is damaged.
        public void UpdateHullDamageModifier(float modifier)
        {
            _hullDamageModifier = modifier;
        }
        public void UpdateSailDamageModifier(float modifier)
        {
            _sailDamageModifier = modifier;
        }
        public void UpdateCrewDamageModifier(float modifier)
        {
            _crewDamageModifier = modifier;
        }

        private void FixedUpdate(){
            SailStateTimer();

            CalculateRotation();
            CalculateMovement();
        }

        // Timer controling the delay between changing sail state.
        private void SailStateTimer()
        {
            if (_sailStateTimeSinceChanged >= _sailStateChangeDelay / _crewDamageModifier){
                _sailStateTimeSinceChanged = 1f;
                return;
            }
            _sailStateTimeSinceChanged += Time.deltaTime;
        }

        private void CalculateRotation()
        {
            float leftSpeed = -(MinValue(_maxTurnSpeed * _crewDamageModifier, _minTurnSpeed));
            float rightSpeed = (MinValue(_maxTurnSpeed * _crewDamageModifier, _minTurnSpeed));
            float turnDirection = 0;

            if (_leftTurn == true){
                turnDirection = leftSpeed;
            }
            if (_rightTurn == true){
                turnDirection = rightSpeed;
            }

            transform.Rotate(new Vector3(0f,turnDirection,0f));
        }

        private void CalculateMovement()
        {
            _targetSpeed = (MinValue(_maxSpeed * _sailDamageModifier, _minSpeed)) * _sailStateModifier;
            float difference = Mathf.Abs(_currentSpeed - _targetSpeed);
            // Calculates movement based off of acceleration/deceleration rate.
            if (_currentSpeed < _targetSpeed && difference > 0.01f)
            {
                _currentSpeed += AccelerationCalc(difference, _maxAccelerationRate * _sailDamageModifier, _accelerationEasingFactor, _minAcceleration);
            }
            else
            if (_currentSpeed > _targetSpeed && difference > 0.01f){
                _currentSpeed -= AccelerationCalc(difference, _maxDecelerationRate * _hullDamageModifier, _decelerationEasingFactor, _minDeceleration);
            }
            // Clamp speed to ensure no engative or overspeed.
            _currentSpeed = Mathf.Clamp(_currentSpeed, 0f, _maxSpeed * _sailDamageModifier);

            // Apply Movement.
            transform.position += transform.forward * _currentSpeed * Time.deltaTime;

           // Debug.Log(_currentSpeed);
        }

        private float AccelerationCalc(float difference, float rate, float easing, float min)
        {
            return MinValue(rate * (difference * easing), min) * Time.deltaTime;
        }

        // Retrieves the sail enum value based on the int state of _sailState.
        private float getSailStateEnumValue(){
            var value = (SpeedModifierEnum)_sailState;
            return (float)value / (Enum.GetValues(typeof(SpeedModifierEnum)).Length - 1);
        }

        private int getSailStateEnumLength(){
            return Enum.GetValues(typeof(SpeedModifierEnum)).Length;
        }
        
        private float MinValue(float min, float current){
            return Mathf.Max(min, current);
        }

    }
}
