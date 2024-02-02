using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreInventory : Inventory
{
    [SerializeField]
    private Transform _inventoryUIContentsPlayer;
    [SerializeField]
    private Transform _inventoryUIContentsStore;
    [SerializeField]
    private GameObject _inventoryUIPrefab;
    protected override InventoryObject CreateInventoryObject(string itemId)
    {
        // Create the UI aspects and then give the IO the refernces to the UI objects.
        GameObject inventoryObjectGameObject = Instantiate(_inventoryObjectPrefab, _inventoryObjectStorage);
        InventoryObject instanceInventoryObject = inventoryObjectGameObject.GetComponent<InventoryObject>();

        return instanceInventoryObject;
    }
}
