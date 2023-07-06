using System;
using UnityEngine;
[Serializable]
public struct DamageAmount
{
    public DamageType GetDamageType => _damageType;
    public int GetDamage => _damage;

    [SerializeField]
    private DamageType _damageType;
    [SerializeField]
    private int _damage;
}
