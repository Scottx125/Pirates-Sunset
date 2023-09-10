using UnityEngine;

[CreateAssetMenu(fileName = "Ammo", menuName = "ScriptableObjects/AmmoTypes", order = 5)]
public class AmmunitionSO : ScriptableObject
{
    [SerializeField]
    private DamageAmountStruct[] _damageAmounts;

    [SerializeField]
    private AmmunitionTypeEnum _ammunitionType;

    [SerializeField]
    private float _maxRange;
    [SerializeField]
    private float _highestPointDistance;
    [SerializeField]
    private float _highestPointOffset;
    [SerializeField]
    private float _rangeOffset; 
    [SerializeField]
    private float _angleOffset;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private Sprite _ammoImage;
    public DamageAmountStruct[] GetDamageAmounts => _damageAmounts;
    public AmmunitionTypeEnum GetAmmunitionType => _ammunitionType;
    public float GetMaxRange => _maxRange;
    public float GetHighestPointDistance => _highestPointDistance;
    public float GetRangeOffset => _rangeOffset;
    public float GetAngleOffset => _angleOffset;
    public float GetSpeed => _speed;
    public float GetHighestPointOffset => _highestPointOffset;

    public Sprite GetAmmoImage => _ammoImage;

}
