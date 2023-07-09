using UnityEngine;

[CreateAssetMenu(fileName = "Damage", menuName = "ScriptableObjects/DamageTypes", order = 5)]
public class DamageSO : ScriptableObject
{
    public DamageAmountStruct[] GetDamageAmounts => _damageAmounts;
    public float GetAmmunitionType => _ammunitionName;

    [SerializeField]
    private DamageAmountStruct[] _damageAmounts;
    [SerializeField]
    private float _ammunitionName;
}
