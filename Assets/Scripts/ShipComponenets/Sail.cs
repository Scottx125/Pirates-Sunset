using PirateGame.Health;
using UnityEngine;
using PirateGame.Helpers;

public class Sail : Health, ICurrentSpeed
{
    // States of control for sails.
    private float _sailStateModifier;
    private int _sailState = 0;

    // State change delay.
    private float _sailStateChangeDelay;
    private float _sailStateTimeSinceChanged;
    
    private float _maxSpeed, _minSpeed;
    private float _maxAccelerationRate, _accelerationEasingFactor, _minAcceleration;
    private float _maxDecelerationRate, _decelerationEasingFactor, _minDeceleration;
    private float _currentSpeed;
    private float _targetSpeed;
    private float _sailDamageModifier = 1f;

    private ICrewDamageModifier _crewDamage;
    private IHullDamageModifier _hullDamage;

    public void SetupMovementComponenet(ICrewDamageModifier crewDamage, IHullDamageModifier hullDamage, MoverDataStruct movementData)
    {
        // Allow the sail state to change immediately.
        _sailStateTimeSinceChanged = 1f;
        // Setup of variables.
        _crewDamage = crewDamage;
        _hullDamage = hullDamage;
        _sailStateChangeDelay = movementData.GetSailStateChangeDelay;
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
        SailStateTimer();
        CalculateMovement();
    }

    public override void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _sailDamageModifier = ToPercent(_currentHealth, _maxHealth);
    }

    // Increase/decrease ship sail state.
    public void SailStateIncrease()
    {
        if (_sailState < StaticHelpers.GetSailStateEnumLength() && _sailStateTimeSinceChanged >= _sailStateChangeDelay){
            _sailState++;
             _sailStateModifier = StaticHelpers.GetSailStateEnumValue(_sailState);
             _sailStateTimeSinceChanged = 0f;
        }
    }
    public void SailStateDecrease()
        {
        if (_sailState > 0 && _sailStateTimeSinceChanged >= _sailStateChangeDelay){
            _sailState--;
            _sailStateModifier = StaticHelpers.GetSailStateEnumValue(_sailState);
            _sailStateTimeSinceChanged = 0f;
        }
    }

    // Timer controling the delay between changing sail state.
    private void SailStateTimer()
    {
        if (_sailStateTimeSinceChanged >= _sailStateChangeDelay / _crewDamage.GetCrewDamageModifier()){
            _sailStateTimeSinceChanged = 1f;
            return;
        }
        _sailStateTimeSinceChanged += Time.deltaTime;
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
            _currentSpeed -= AccelerationCalc(difference, _maxDecelerationRate * _hullDamage.GetHullDamageModifier(), _decelerationEasingFactor, _minDeceleration);
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

    public float GetCurrentSpeed()
    {
        return _currentSpeed;
    }
}
