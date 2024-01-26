using PirateGame.Health;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairObject : InventoryObject
{
    [SerializeField]
    int amountToHeal = 5;
    [SerializeField]
    IHeal[] _healableComponenets;

    private void Awake()
    {
        if (_healableComponenets == null)
        {
            _healableComponenets = GetComponents<HealthComponent>();
        }
    }
    protected override void ObjectBehaviour()
    {
        base.SubtractQuantity(1);
        foreach (IHeal healableComponent in _healableComponenets)
        {
            healableComponent.Heal(amountToHeal);
        }
    }
}
