using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PirateGame.Moving{
    public class Movement : MonoBehaviour, IMobilityStateModifier
    {
        private float _maxSpeed, _minSpeed;
        private float _maxAccelerationRate, _accelerationEasingFactor, _minAcceleration;
        private float _maxDecelerationRate, _decelerationEasingFactor, _minDeceleration;
        private float _currentSpeed;
        private float _targetSpeed;
        private float _mobilityStateModifier;
        private ICurrentSpeed _sendCurrentSpeed;
        
        private float _mobilityDamageModifier = 1f;
        private float _structuralDamageModifier = 1f;

        public void SetMobilityDamageModifier(float modifier) => _mobilityDamageModifier = modifier;
        public void SetStructuralDamageModifier(float modifier) => _structuralDamageModifier = modifier;
        public void MobilityStateModifier(float modifier) => _mobilityStateModifier = modifier;

        public void Setup(MovementSO movementData, ICurrentSpeed currentSpeed = null)
        {
            // Setup of variables.
            _maxSpeed = movementData.GetMaxSpeed;
            _minSpeed = movementData.GetMinSpeed;
            _maxAccelerationRate = movementData.GetMaxAccelerationRate;
            _accelerationEasingFactor = movementData.GetAccelerationEasingFactor;
            _minAcceleration = movementData.GetMinAccelration;
            _maxDecelerationRate = movementData.GetMaxDecelerationRate;
            _decelerationEasingFactor = movementData.GetDecelerationEasingFactor;
            _minDeceleration = movementData.GetMinDeceleration;
            _sendCurrentSpeed = currentSpeed;
        }

        private void FixedUpdate()
        {
            CalculateMovement();
        }

        private void CalculateMovement()
        {
            _targetSpeed = (Mathf.Max(_maxSpeed * _mobilityDamageModifier, _minSpeed)) * _mobilityStateModifier;
            float difference = Mathf.Abs(_currentSpeed - _targetSpeed);
            // Calculates movement based off of acceleration/deceleration rate.
            if (_currentSpeed < _targetSpeed)
            {
                _currentSpeed += AccelerationCalc(difference, _maxAccelerationRate * _mobilityDamageModifier, _accelerationEasingFactor, _minAcceleration);
            }
            else
            if (_currentSpeed > _targetSpeed){
                _currentSpeed -= AccelerationCalc(difference, _maxDecelerationRate * _structuralDamageModifier, _decelerationEasingFactor, _minDeceleration);
            }
            // Clamp speed to ensure no engative or overspeed.
            _currentSpeed = Mathf.Clamp(_currentSpeed, 0f, _maxSpeed * _mobilityDamageModifier);
            // sends current speed to listener.
            if (_sendCurrentSpeed != null) _sendCurrentSpeed.SetCurrentSpeed(_currentSpeed);

            // Apply Movement.
            transform.position += transform.forward * _currentSpeed * Time.deltaTime;
        }

        private float AccelerationCalc(float difference, float rate, float easing, float min)
        {
            return Mathf.Max(rate * (difference * easing), min) * Time.deltaTime;
        }
    }
}
