using System;
using UnityEngine;
using PirateGame.Health;

namespace PirateGame.Movement{
    public class Mover : MonoBehaviour, IObserverShipMovement
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
        private float _maxSpeed = 10f;
        [SerializeField]
        private float _turnSpeed = 5f;
        [SerializeField]
        private float _initialAccelerationRate = 3.5f, _accelerationEasingFactor = 0.5f, _minimumAcceleration = 0.1f;
        [SerializeField]
        private float _initialDecelerationRate = 3.5f, _decelerationEasingFactor = 0.5f, _minimumDeceleration = 0.1f;
        private float _hullModifier = 1f;
        private float _sailModifier = 1f;
        private float _crewModifier = 1f;
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

            // Setup Observers.
            gameObject.GetComponent<Hull>().AddHealthObserver(this);
            gameObject.GetComponent<Sail>().AddHealthObserver(this);
            gameObject.GetComponent<Crew>().AddHealthObserver(this);
        }

        private void FixedUpdate(){
            SailStateTimer();

            CalculateRotation();
            CalculateMovement();
        }

        private void SailStateTimer()
        {
            if (_sailStateTimeSinceChanged >= _sailStateChangeDelay / _crewModifier){
                _sailStateTimeSinceChanged = 1f;
                return;
            }
            _sailStateTimeSinceChanged += Time.deltaTime;
        }

        private void CalculateRotation()
        {
            float leftSpeed = -_turnSpeed * _crewModifier;
            float rightSpeed = _turnSpeed * _crewModifier;
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
            _targetSpeed = (_maxSpeed * _sailModifier) * _sailStateModifier;
            float difference = Mathf.Abs(_currentSpeed - _targetSpeed);
            // Calculates movement based off of acceleration/deceleration rate.
            if (_currentSpeed < _targetSpeed && difference > 0.01f)
            {
                _currentSpeed += AccelerationCalc(difference, _initialAccelerationRate * _sailModifier, _accelerationEasingFactor, _minimumAcceleration);
            }
            else
            if (_currentSpeed > _targetSpeed && difference > 0.01f){
                _currentSpeed -= AccelerationCalc(difference, _initialDecelerationRate * _hullModifier, _decelerationEasingFactor, _minimumDeceleration);
            }
            // Clamp speed to ensure no engative or overspeed.
            _currentSpeed = Mathf.Clamp(_currentSpeed, 0f, _maxSpeed * _sailModifier);

            // Apply Movement.
            transform.position += transform.forward * _currentSpeed * Time.deltaTime;

            Debug.Log(_currentSpeed);
        }

        private float AccelerationCalc(float difference, float rate, float easing, float min)
        {
            return min + rate * (difference * easing) * Time.deltaTime;
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

        public void OnDamageNotify(float healthPercentage, AttackTypeEnum type)
        {
            if (healthPercentage < .25f){
                return;
            }
            switch (type){
                case AttackTypeEnum.Round_Shot:
                    _hullModifier = healthPercentage;
                break;
                case AttackTypeEnum.Chain_Shot:
                    _sailModifier = healthPercentage;
                break;
                case AttackTypeEnum.Grape_Shot:
                    _crewModifier = healthPercentage;
                break;           
            }
        }
    }
}
