using PirateGame.Health;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairObject : InventoryObject
{
    [SerializeField]
    int amountToHeal = 5;
    [SerializeField]
    HealthComponent[] _healthComponenet;

    private void Awake()
    {
        if (_healthComponenet == null)
        {
            _healthComponenet = GetComponents<HealthComponent>();
        }
    }
    protected override void ObjectBehaviour()
    {
        base.SubtractQuantity(1);
        foreach (HealthComponent healthComponenet in _healthComponenet)
        {
            healthComponenet.Heal(amountToHeal);
        }
    }
}
