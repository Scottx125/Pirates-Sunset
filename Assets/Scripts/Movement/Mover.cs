using System;
using UnityEngine;
using PirateGame.Health;

namespace PirateGame.Movement{
    public class Mover : MonoBehaviour, IOnDamaged
    {
        // States of control for sails.
        private float _sailStateModifier;
        private int _sailState = 0;

        // State change delay.
        [SerializeField]
        private float _sailStateChangeDelay = 1f;
        private float _sailStateTimeSinceChanged;

        // Movement attributes.
        [SerializeField]
        private float _maxSpeed = 10f, _minSpeed = 1f;
        [SerializeField]
        private float _turnSpeed = 5f, _minTurnSpeed = 0.5f;
        [SerializeField]
        private float _initialAccelerationRate = 3.5f, _accelerationEasingFactor = 0.5f, _minimumAcceleration = 0.1f;
        [SerializeField]
        private float _initialDecelerationRate = 3.5f, _decelerationEasingFactor = 0.5f, _minimumDeceleration = 0.1f;
        private float _hullDamageModifier = 1f;
        private float _sailDamageModifier = 1f;
        private float _crewDamageModifier = 1f;
        private float _currentSpeed;
        private float _targetSpeed;
        private bool _leftTurn, _rightTurn;


        // Sail struct, names and float modifier values stored in a struct and held in an array.

            // Use this to get sail states/speed. Remove the struct and remove array also.
            // Set up a case system where you cast the enum to a value and then iterate through those values based on an int state.
        private void Awake()
        {
            Setup();
        }

        private void Setup()
        {
            // Allow the player to fire immediately.
            _sailStateTimeSinceChanged = 1f;
        }

        private void FixedUpdate(){
            SailStateTimer();

            CalculateRotation();
            CalculateMovement();
        }

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
            float leftSpeed = -(MinValue(_turnSpeed * _crewDamageModifier, _minTurnSpeed));
            float rightSpeed = (MinValue(_turnSpeed * _crewDamageModifier, _minTurnSpeed));
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
                _currentSpeed += AccelerationCalc(difference, _initialAccelerationRate * _sailDamageModifier, _accelerationEasingFactor, _minimumAcceleration);
            }
            else
            if (_currentSpeed > _targetSpeed && difference > 0.01f){
                _currentSpeed -= AccelerationCalc(difference, _initialDecelerationRate * _hullDamageModifier, _decelerationEasingFactor, _minimumDeceleration);
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

        public void OnHullDamage(float modifier)
        {
            _hullDamageModifier = modifier;
            Debug.Log(modifier);
        }

        public void OnSailDamage(float modifier)
        {
            _sailDamageModifier = modifier;
        }

        public void OnCrewDamage(float modifier)
        {
            _crewDamageModifier = modifier;
        }
    }
}
