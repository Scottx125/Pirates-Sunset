using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _inventoryUIContentsParent;
    // This will hold the inventory
    [SerializeField]
    private List<InventoryObject> _inventory = new List<InventoryObject>();

    // STORE WILL HOLD PREFABS FOR THESE OBJECTS. IF THE PLAYER DOES NOT HAVE A BOUGHT OBJECT.
    // IT WILL CREATE A NEW OBJECT AND PASS IN THAT OBJECT AND QUANTITY.
    public void Setup()
    {
        if (_inventoryUIContentsParent == null) {
            Debug.LogError("Need to assign the UI contents object!");
            return;
        }
    }

    // Returns the type and quantity of the objects in the inventory.
    public (Type, int)[] GetInventoryCopy()
    {
        (Type, int)[] copy = new (Type, int)[_inventory.Count];
        for(int i = 0; i < _inventory.Count; i++)
        {
            copy[i] = (_inventory[i].GetType(), _inventory[i].GetQuantity());
        }
        return copy;
    }
    public void AddToInventory(InventoryObject inventoryObject, int amountToAdd)
    {
        InventoryObject existingItem = CheckInventoryForExistingItem(inventoryObject);

        if (existingItem == null)
        {
            _inventory.Add(inventoryObject);
            AddToInventoryQuantity(inventoryObject, amountToAdd);
        }
        else
        {
            AddToInventoryQuantity(existingItem, amountToAdd);
        }
    }
    public void RemoveFromInventory(InventoryObject inventoryObject, int amountToSubtract)
    {
        InventoryObject existingItem = CheckInventoryForExistingItem(inventoryObject);

        if (existingItem == null)
        {
            // Safety
            return;
        }
        else
        {
            // Get specific object and then remove X amount.
            RemoveFromInventoryQuantity(existingItem, amountToSubtract);
        }
    }

    private InventoryObject CheckInventoryForExistingItem(InventoryObject inventoryObject)
    {
        // Get the type of the item if it's in the list.
        return _inventory.FirstOrDefault(existing => existing.GetType() == inventoryObject.GetType());
    }

    private void AddToInventoryQuantity(InventoryObject existingItem, int amountToAdd)
    {
        existingItem.AddQuantity(amountToAdd);
        // add update to reflect change in quantity
    }

    private void RemoveFromInventoryQuantity(InventoryObject existingItem, int amountToSubtract)
    {
        existingItem.SubtractQuantity(amountToSubtract);
        // add update to reflect change in quantity
        if (existingItem.GetQuantity() <= 0)
        {
            _inventory.Remove(existingItem);
            Destroy(existingItem.gameObject);
        }
    }

    // This will deal with interactions between the store and the inventory objects. Such as adding/removing. Finding items in the inventory etc.
}
