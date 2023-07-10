using UnityEngine;

[CreateAssetMenu(fileName = "Damage", menuName = "ScriptableObjects/DamageTypes", order = 5)]
public class DamageSO : ScriptableObject
{
    public DamageAmountStruct[] GetDamageAmounts => _damageAmounts;
    public AmmunitionTypeEnum GetAmmunitionType => _ammunitionType;

    [SerializeField]
    private DamageAmountStruct[] _damageAmounts;

    [SerializeField]
    private AmmunitionTypeEnum _ammunitionType;
}
