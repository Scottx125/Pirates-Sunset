using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AttackTypesStruct
{
    public void Setup(AmmunitionSO ammoData)
    {
        _ammoData = ammoData;
    }
    public AmmunitionSO GetAmmoData => _ammoData;
    public float DesireBeforeChange
    {
        get
        {
            return _desireBeforeChange;
        }
        set
        {
            _desireBeforeChange = value;
        }
    }
    private AmmunitionSO _ammoData;
    private float _desireBeforeChange;
}
