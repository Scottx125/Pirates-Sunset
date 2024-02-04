using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreInventoryObject : InventoryObject
{
    public void Setup(string id)
    {
        // Setup object variables.
        InventoryObjectQuantity = 0;
        InventoryObjectId = id;
    }

    public override void AddQuantity(int addValue)
    {
        InventoryObjectQuantity += addValue;
    }

    public override void SubtractQuantity(int subtractValue)
    {
        InventoryObjectQuantity -= subtractValue;
    }
}