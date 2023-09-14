using System;
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
        private List<ICurrentSpeed> _sendCurrentSpeedList = new List<ICurrentSpeed>();
        private Transform _noMoveArea;
        private float _mobilityDamageModifier = 1f;
        private float _structuralDamageModifier = 1f;

        public void SetMobilityDamageModifier(float modifier) => _mobilityDamageModifier = modifier;
        public void SetStructuralDamageModifier(float modifier) => _structuralDamageModifier = modifier;
        public void MobilityStateModifier(float modifier) => _mobilityStateModifier = modifier;

        public void Setup(MovementSO movementData, List<ICurrentSpeed> currentSpeedList)
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
            foreach(ICurrentSpeed reciever in currentSpeedList)
            {
                if (reciever != null)
                {
                    _sendCurrentSpeedList.Add(reciever);
                }
            }
        }

        private void DetermineIfWeCanMove()
        {
            // Check if ship is facing away from the no-movement area
            Vector3 directionToNoMovementArea = (_noMoveArea.position - transform.position).normalized;
            float dotProduct = Vector3.Dot(transform.forward, directionToNoMovementArea);
            // If facing away allow movement.
            if (dotProduct <= 0)
            {
                CalculateMovement();
            }
        }

        private void FixedUpdate()
        {
            if (_noMoveArea)
            {
                DetermineIfWeCanMove();
            } else
            {
                CalculateMovement();
            }
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
            _currentSpeed = Mathf.Clamp(_currentSpeed, 0f, _targetSpeed);
            // sends current speed to listener. (determines turn rate based on speed.)
            if (_sendCurrentSpeedList != null)
            {
                foreach(ICurrentSpeed reciever in _sendCurrentSpeedList)
                {
                    reciever.SetCurrentSpeed(_currentSpeed);
                }
            }
            // Apply Movement.
            transform.position += transform.forward * _currentSpeed * Time.deltaTime;
        }

        private float AccelerationCalc(float difference, float rate, float easing, float min)
        {
            return Mathf.Max(rate * (difference * easing), min) * Time.deltaTime;
        }

        // Detects if we enter a no-move area.
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "NoMove")
            {
                _noMoveArea = other.transform;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "NoMove")
            {
                _noMoveArea = null;
            }
        }
    }
}
