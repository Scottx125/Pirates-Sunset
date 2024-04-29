using PirateGame.Health;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairObject : AbilityObject
{
    [SerializeField]
    int amountToHeal = 5;
    [SerializeField]
    IHeal[] _healableComponenets;

    private void Start()
    {
        if (_healableComponenets == null)
        {
            _healableComponenets = GetComponentsInParent<HealthComponent>();
        }
    }
    protected override void ObjectBehaviour()
    {
        ConsumedQuantity(1);
        foreach (IHeal healableComponent in _healableComponenets)
        {
            healableComponent.Heal(amountToHeal);
        }
    }
}
