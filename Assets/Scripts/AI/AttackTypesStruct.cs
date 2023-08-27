using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTypesStruct
{
    public void Setup(AmmunitionSO ammoData)
    {
        _ammoData = ammoData;
    }
    public AmmunitionSO GetAmmoData => _ammoData;
    public float SelectedModifier
    {
        get
        {
            return _selectedModifier;
        }
        set
        {
            _selectedModifier = value;
        }
    }
    public float WeightValue
    {
        get
        {
            return _weightValue;
        }
        set
        {
            _weightValue = value;
        }
    }
    public float TotalWeightValue
    {
        get
        {
            return _weightValue + _selectedModifier;
        }
    }
    private AmmunitionSO _ammoData;
    private float _selectedModifier;
    private float _weightValue;
}
