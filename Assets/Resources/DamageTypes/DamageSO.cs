using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Damage", menuName = "ScriptableObjects/DamageTypes", order = 1)]
public class DamageSO : ScriptableObject
{
    public DamageAmount[] GetDamageAmounts => _damageAmounts;

    [SerializeField]
    private DamageAmount[] _damageAmounts;
}
