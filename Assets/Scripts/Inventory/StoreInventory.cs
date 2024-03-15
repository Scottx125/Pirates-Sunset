using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreInventory : Inventory
{
    [SerializeField]
    protected Transform _storeInventoryObjectStorage;
    protected override InventoryObject CreateInventoryObject(string itemId)
    {
        // Cache data.
        InventoryObjectSO data = _inventoryObjectsSOsDict[itemId];
        // SPAWN MAIN INVENTORY PREFAB AND GET RELEVANT SCRIPTS.
        GameObject inventoryObjectInstance = Instantiate(data.GetStoreInventoryObjectPrefab, _storeInventoryObjectStorage);
        StoreInventoryObject storeInvenObjectInstance = inventoryObjectInstance.GetComponent<StoreInventoryObject>();
        storeInvenObjectInstance.Setup(itemId);

        return storeInvenObjectInstance;
    }
}
