using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreInventory : Inventory
{
    protected override InventoryObject CreateInventoryObject(string itemId)
    {
        // SPAWN MAIN INVENTORY PREFAB AND GET RELEVANT SCRIPTS.
        StoreInventoryObject inventoryObject = new StoreInventoryObject();
        inventoryObject.Setup(itemId);

        return inventoryObject;
    }
}
