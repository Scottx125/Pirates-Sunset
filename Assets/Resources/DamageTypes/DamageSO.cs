using UnityEngine;

[CreateAssetMenu(fileName = "Damage", menuName = "ScriptableObjects/DamageTypes", order = 5)]
public class DamageSO : ScriptableObject
{
    public DamageAmount[] GetDamageAmounts => _damageAmounts;

    [SerializeField]
    private DamageAmount[] _damageAmounts;
}
