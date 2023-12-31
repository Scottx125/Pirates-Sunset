using UnityEngine;

[CreateAssetMenu(fileName = "Movement", menuName = "ScriptableObjects/Movement", order = 2)]
public class MovementSO : ScriptableObject
{
    [SerializeField]
    private float _sailStateChangeDelay;
    [SerializeField]
    private float _maxSpeed, _minSpeed;
    [SerializeField]
    private SpeedModifierEnum _turnSpeedEasePoint;
    [SerializeField]
    private float _maxTurnSpeed, _minTurnSpeed;
    [SerializeField]
    private float _maxAccelerationRate, _accelerationEasingFactor, _minAcceleration;
    [SerializeField]
    private float _maxDecelerationRate, _decelerationEasingFactor, _minDeceleration;

    public float GetMobilityStateChangeDelay => _sailStateChangeDelay;
    public float GetMaxSpeed => _maxSpeed;
    public float GetMinSpeed => _minSpeed;
    public SpeedModifierEnum GetTurnSpeedEasePoint => _turnSpeedEasePoint;
    public float GetMaxTurnSpeed => _maxTurnSpeed;
    public float GetMinTurnSpeed => _minTurnSpeed;
    public float GetMaxAccelerationRate => _maxAccelerationRate;
    public float GetAccelerationEasingFactor => _accelerationEasingFactor;
    public float GetMinAccelration => _minAcceleration;
    public float GetMaxDecelerationRate => _maxDecelerationRate;
    public float GetDecelerationEasingFactor => _decelerationEasingFactor;
    public float GetMinDeceleration => _minDeceleration;
}
