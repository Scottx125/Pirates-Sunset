using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreInventoryObject : InventoryObject
{
    public void Setup(string id)
    {
        // Setup object variables.
        InventoryObjectId = id;
    }

    public override void SetQuantity(int quantity)
    {
        _inventoryObjectQuantity = quantity;
    }
}
