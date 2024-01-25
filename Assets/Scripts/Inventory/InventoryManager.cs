using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private InventoryUIBuilder _inventoryUIBuilder;
    [SerializeField]
    private Inventory _inventory;
    [SerializeField]
    private AbilityTracker _abilityTracker;


    // Will simply hold all the aspects for the inventory system.

    public void Setup()
    {
        // SEtup inventory
    }






    // This will deal with interactions between the store and the inventory objects. Such as adding/removing. Finding items in the inventory etc.
}
