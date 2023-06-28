using System;
using UnityEngine;

[Serializable]
public struct MoverDataStruct
{
    [SerializeField]
    private float _sailStateChangeDelay;
    [SerializeField]
    private float _maxSpeed, _minSpeed;
    [SerializeField]
    private float _maxTurnSpeed, _minTurnSpeed;
    [SerializeField]
    private float _maxAccelerationRate, _accelerationEasingFactor, _minAcceleration;
    [SerializeField]
    private float _maxDecelerationRate, _decelerationEasingFactor, _minDeceleration;

    public float GetSailStateChangeDelay => _sailStateChangeDelay;
    public float GetMaxSpeed => _maxSpeed;
    public float GetMinSpeed => _minSpeed;
    public float GetMaxTurnSpeed => _maxTurnSpeed;
    public float GetMinTurnSpeed => _minTurnSpeed;
    public float GetMaxAccelerationRate => _maxAccelerationRate;
    public float GetAccelerationEasingFactor => _accelerationEasingFactor;
    public float GetMinAccelration => _minAcceleration;
    public float GetMaxDecelerationRate => _maxDecelerationRate;
    public float GetDecelerationEasingFactor => _decelerationEasingFactor;
    public float GetMinDeceleration => _minDeceleration;
}
