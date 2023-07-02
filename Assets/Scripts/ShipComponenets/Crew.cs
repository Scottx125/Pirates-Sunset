using PirateGame.Health;
using PirateGame.Helpers;
using UnityEngine;

public class Crew : Health, ICrewDamageModifier
{
    private float _crewDamageModifier = 1f;

    private float _maxTurnSpeed, _minTurnSpeed;
    private int _turnSpeedEasePoint;
    private bool _leftTurn, _rightTurn;
    private float _maxSpeed;

    private ICurrentSpeed _speed;

    // Turning enabled/disabled methods.
    public void SetLeftTurn(bool state) => _leftTurn = state; 
    public void SetRightTurn(bool state) => _rightTurn = state; 

    public void SetupMovementComponenet(ICurrentSpeed speed, MoverDataStruct movementData)
    {
        _speed = speed;
        _maxTurnSpeed = movementData.GetMaxTurnSpeed;
        _minTurnSpeed = movementData.GetMinTurnSpeed;
        _turnSpeedEasePoint = movementData.GetTurnSpeedEasePoint;
        _maxSpeed = movementData.GetMaxSpeed;
    }

    private void FixedUpdate()
    {
        CalculateRotation();
    }

    public override void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _crewDamageModifier = ToPercent(_currentHealth, _maxHealth);
    }

    private void CalculateRotation()
    {
        float turnDirectionSpeed = 0f;

        // Calculate the ease point based on the _turnSpeedEasePoint and then 
        // use that to determine the speed at which the turn speed will linearly ramp up or down from.
        // So that at low speeds the ship turns slower, but at a certain speed it will reach it's maximum turn rate.
        float easePointValue = _maxSpeed * StaticHelpers.GetSailStateEnumValue(_turnSpeedEasePoint);
        float easePointDifference = _speed.GetCurrentSpeed() < easePointValue ? _speed.GetCurrentSpeed() / easePointValue : 1f;

        float turnSpeed = (Mathf.Max((_maxTurnSpeed * easePointDifference) * _crewDamageModifier, _minTurnSpeed));

        if (_leftTurn){
            turnDirectionSpeed = -turnSpeed;
        }
        if (_rightTurn){
            turnDirectionSpeed = turnSpeed;
        }

        transform.Rotate(new Vector3(0f,turnDirectionSpeed,0f));
    }

    public float GetCrewDamageModifier()
    {
        return _crewDamageModifier;
    }
}
