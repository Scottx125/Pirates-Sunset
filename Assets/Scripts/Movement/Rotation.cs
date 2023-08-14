using UnityEngine;
using PirateGame.Helpers;

public class Rotation : MonoBehaviour, ICurrentSpeed
{
    private float _corporealDamageModifier = 1f;

    private float _maxTurnSpeed, _minTurnSpeed;
    private int _turnSpeedEasePoint;
    private float _maxSpeed;
    private bool _leftTurn, _rightTurn;
    private float _currentSpeed;

    // Turning enabled/disabled methods.
    public void SetLeftTurn(bool state) => _leftTurn = state; 
    public void SetRightTurn(bool state) => _rightTurn = state; 
    public void SetCorporealDamageModifier(float modifier) => _corporealDamageModifier = modifier;
    public void SetCurrentSpeed(float currentSpeed) => _currentSpeed = currentSpeed;

    public void Setup(MovementSO movementData)
    {
        _maxTurnSpeed = movementData.GetMaxTurnSpeed;
        _minTurnSpeed = movementData.GetMinTurnSpeed;
        _turnSpeedEasePoint = (int)movementData.GetTurnSpeedEasePoint;
        _maxSpeed = movementData.GetMaxSpeed;
    }

    private void FixedUpdate()
    {
        CalculateRotation();
    }

    private void CalculateRotation()
    {
        float turnDirectionSpeed = 0f;

        // Calculate the ease point based on the _turnSpeedEasePoint and then 
        // use that to determine the speed at which the turn speed will linearly ramp up or down from.
        // So that at low speeds the ship turns slower, but at a certain speed it will reach it's maximum turn rate.
        float easePointValue = _maxSpeed * StaticHelpers.GetMobilityStateEnumValue(_turnSpeedEasePoint);
        float easePointDifference = _currentSpeed < easePointValue ? _currentSpeed / easePointValue : 1f;

        float turnSpeed = (Mathf.Max((_maxTurnSpeed * easePointDifference) * _corporealDamageModifier, _minTurnSpeed));

        if (_leftTurn){
            turnDirectionSpeed = -turnSpeed;
        }
        if (_rightTurn){
            turnDirectionSpeed = turnSpeed;
        }

        transform.Rotate(new Vector3(0f,turnDirectionSpeed,0f));
    }
}
