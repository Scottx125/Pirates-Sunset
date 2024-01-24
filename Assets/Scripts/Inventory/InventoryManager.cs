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


    // Store will have a SO of Data that will contain a List of all the InventoryObjects, and a nested List of buy/sell prices.
    // When we send the info. We'll compare the Type to get the relevant price.

    // Inventory will do the same. However, this will hold a list of all the inventorydata.
    // If a new object needs to be made, it'll compare against them to find a match, once it has. It'll instantiate the prefab and get the InventoryObject attatched and add it to the list.
    public void Setup()
    {
        // SEtup inventory
    }

    // Returns the type and quantity of the objects in the inventory.
    public StoreItemData[] GetInventoryCopy()
    {
        StoreItemData[] copy = new StoreItemData[_inventory.Count];
        for (int i = 0; i < _inventory.Count; i++)
        {
            copy[i] = new StoreItemData(_inventory[i].GetQuantity(), _inventory[i], _inventory[i].GetBuyPrice(), _inventory[i].GetSellPrice(), _inventory[i].GetName(), _inventory[i].GetSprite());
        }
        return copy;
    }
    public void AddToInventory(InventoryObject inventoryType, int amountToAdd)
    {
        InventoryObject existingItem = _inventory.CheckInventoryForExistingItem(inventoryType);

        if (existingItem == null && _inventoryObjectsSOsDict.ContainsKey(inventoryType))
        {
            // Spawn UI prefabs. Pass itself in the inventory and the ability UI.
            InventoryObject obj = CreateInventoryObject(inventoryType);
            _inventory.Add(obj);
            _inventory.AddToInventoryQuantity(obj, amountToAdd);
        }
        else
        {
            _inventory.AddToInventoryQuantity(existingItem, amountToAdd);
        }
    }

    private InventoryObject CreateInventoryObject(InventoryObject inventoryType)
    {
        // Create the UI aspects and then give the IO the refernces to the UI objects.
        GameObject inventoryUIInstance = _inventoryUIBuilder.CreateInventoryUIPrefab(_inventoryObjectsSOsDict[inventoryType].GetName, _inventoryObjectsSOsDict[inventoryType].GetImage);
        GameObject abilityUIInstance = _inventoryUIBuilder.CreateAbilityHUDPrefab(_inventoryObjectsSOsDict[inventoryType].GetName, _inventoryObjectsSOsDict[inventoryType].GetImage);
        InventoryObject instanceInventoryObject = inventoryUIInstance.GetComponent<InventoryObject>();
        instanceInventoryObject.Setup(inventoryUIInstance, abilityUIInstance);
        return instanceInventoryObject;
    }

    public void RemoveFromInventory(InventoryObject inventoryType, int amountToSubtract)
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


    // This will deal with interactions between the store and the inventory objects. Such as adding/removing. Finding items in the inventory etc.
}
