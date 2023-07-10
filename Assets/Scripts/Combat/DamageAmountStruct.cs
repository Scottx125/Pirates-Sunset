using System;
using UnityEngine;
[Serializable]
public struct DamageAmountStruct
{
    public DamageType GetDamageType => _damageType;
    public int GetDamage => _damage;
    public bool GetOverTimeBool => _overTimeBool;
    public float GetTime => _time;
    public int GetRepeatAmount => _repeatAmount;

    [SerializeField]
    private DamageType _damageType;
    [SerializeField]
    private int _damage;
    [SerializeField]
    private bool _overTimeBool;
    [SerializeField]
    private float _time;
    [SerializeField]
    private int _repeatAmount;
}
