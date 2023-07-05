using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private float _maxSpeed, _minSpeed;
    private float _maxAccelerationRate, _accelerationEasingFactor, _minAcceleration;
    private float _maxDecelerationRate, _decelerationEasingFactor, _minDeceleration;
    private float _currentSpeed;
    private float _targetSpeed;

    private float _sailStateModifier = 0;
    private float _sailDamageModifier = 1f;
    private float _hullDamageModifier = 1f;

    public void Setup(MoverDataStruct movementData)
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
    }

    private void FixedUpdate()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        _targetSpeed = (Mathf.Max(_maxSpeed * _sailDamageModifier, _minSpeed)) * _sailStateModifier;
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
        return Mathf.Max(rate * (difference * easing), min) * Time.deltaTime;
    }
}
