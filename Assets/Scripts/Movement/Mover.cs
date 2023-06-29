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
        private int _turnSpeedEasePoint;
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
            _turnSpeedEasePoint = moverDataStruct.GetTurnSpeedEasePoint;
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
            if (_sailState < getSailStateEnumLength() && _sailStateTimeSinceChanged >= _sailStateChangeDelay){
                _sailState++;
                _sailStateModifier = getSailStateEnumValue(_sailState);
                _sailStateTimeSinceChanged = 0f;
            }
        }
        public void SailStateDecrease(){
            if (_sailState > 0 && _sailStateTimeSinceChanged >= _sailStateChangeDelay){
                _sailState--;
                _sailStateModifier = getSailStateEnumValue(_sailState);
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
            float turnDirectionSpeed = 0;

            // Calculate the ease point based on the _turnSpeedEasePoint and then 
            // use that to determine the speed at which the turn speed will linearly ramp up or down from.
            // So that at low speeds the ship turns slower, but at a certain speed it will reach it's maximum turn rate.
            float easePointValue = _maxSpeed * getSailStateEnumValue(_turnSpeedEasePoint);
            float easePointDifference = _currentSpeed < easePointValue ? _currentSpeed / easePointValue : 1f;

            float turnSpeed = (MinValue((_maxTurnSpeed * easePointDifference) * _crewDamageModifier, _minTurnSpeed));

            if (_leftTurn == true){
                turnDirectionSpeed = -turnSpeed;
            }
            if (_rightTurn == true){
                turnDirectionSpeed = turnSpeed;
            }

            transform.Rotate(new Vector3(0f,turnDirectionSpeed,0f));
        }

        private void CalculateMovement()
        {
            _targetSpeed = (MinValue(_maxSpeed * _sailDamageModifier, _minSpeed)) * _sailStateModifier;
            float difference = Mathf.Abs(_currentSpeed - _targetSpeed);
            // Calculates movement based off of acceleration/deceleration rate.
            if (_currentSpeed < _targetSpeed)
            {
                _currentSpeed += AccelerationCalc(difference, _maxAccelerationRate * _sailDamageModifier, _accelerationEasingFactor, _minAcceleration);
            }
            else
            if (_currentSpeed > _targetSpeed){
                _currentSpeed -= AccelerationCalc(difference, _maxDecelerationRate * _hullDamageModifier, _decelerationEasingFactor, _minDeceleration);
            }
            // Clamp speed to ensure no engative or overspeed.
            _currentSpeed = Mathf.Clamp(_currentSpeed, 0f, _maxSpeed * _sailDamageModifier);

            // Apply Movement.
            transform.position += transform.forward * _currentSpeed * Time.deltaTime;
        }

        private float AccelerationCalc(float difference, float rate, float easing, float min)
        {
            return MinValue(rate * (difference * easing), min) * Time.deltaTime;
        }

        // Retrieves the sail enum value based on the int state of _sailState.
        private float getSailStateEnumValue(int speedMod){
            var value = (SpeedModifierEnum)speedMod;
            return (float)value / (Enum.GetValues(typeof(SpeedModifierEnum)).Length);
        }

        private int getSailStateEnumLength(){
            return Enum.GetValues(typeof(SpeedModifierEnum)).Length - 1;
        }
        
        private float MinValue(float a, float b){
            return Mathf.Max(a, b);
        }

    }
}
