using System;
using UnityEngine;
[Serializable]
public struct DamageAmountStruct
{
    public DamageType GetDamageType => _damageType;
    public int GetDamage => _damage;

    [SerializeField]
    private DamageType _damageType;
    [SerializeField]
    private int _damage;
}
