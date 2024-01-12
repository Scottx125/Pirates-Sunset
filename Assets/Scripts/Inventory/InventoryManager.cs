using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _inventoryUIContentsParent;
    [SerializeField]
    private List<InventoryObject> _inventory = new List<InventoryObject>();

    // Store will have a SO of Data that will contain a List of all the InventoryObjects, and a nested List of buy/sell prices.
    // When we send the info. We'll compare the Type to get the relevant price.

    // Inventory will do the same. However, this will hold a list of all the inventorydata.
    // If a new object needs to be made, it'll compare against them to find a match, once it has. It'll instantiate the prefab and get the InventoryObject attatched and add it to the list.
    public void Setup()
    {
        if (_inventoryUIContentsParent == null) {
            Debug.LogError("Need to assign the UI contents object!");
            return;
        }
    }

    // Returns the type and quantity of the objects in the inventory.
    public (Type, int, int, int, string, Sprite)[] GetInventoryCopy()
    {
        (Type, int, int, int, string, Sprite)[] copy = new (Type, int, int, int, string, Sprite)[_inventory.Count];
        for(int i = 0; i < _inventory.Count; i++)
        {
            copy[i] = (_inventory[i].GetType(), _inventory[i].GetQuantity(), _inventory[i].GetBuyPrice(), _inventory[i].GetSellPrice(), _inventory[i].GetName(), _inventory[i].GetSprite());
        }
        return copy;
    }
    public void AddToInventory(Type inventoryType, int amountToAdd)
    {
        InventoryObject existingItem = CheckInventoryForExistingItem(inventoryType);

        if (existingItem == null)
        {

            //_inventory.Add(inventoryObject);
            // Setuo the UI
            // AddToInventoryQuantity(inventoryObject, amountToAdd);
        }
        else
        {
            AddToInventoryQuantity(existingItem, amountToAdd);
        }
    }
    public void RemoveFromInventory(Type inventoryType, int amountToSubtract)
    {
        InventoryObject existingItem = CheckInventoryForExistingItem(inventoryType);

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

    private InventoryObject CheckInventoryForExistingItem(Type inventoryType)
    {
        // Get the type of the item if it's in the list.
        return _inventory.FirstOrDefault(existing => existing.GetType() == inventoryType);
    }

    private void AddToInventoryQuantity(InventoryObject existingItem, int amountToAdd)
    {
        existingItem.AddQuantity(amountToAdd);
    }

    private void RemoveFromInventoryQuantity(InventoryObject existingItem, int amountToSubtract)
    {
        existingItem.SubtractQuantity(amountToSubtract);
        if (existingItem.GetQuantity() <= 0)
        {
            _inventory.Remove(existingItem);
            Destroy(existingItem.gameObject);
        }
    }

    // This will deal with interactions between the store and the inventory objects. Such as adding/removing. Finding items in the inventory etc.
}
