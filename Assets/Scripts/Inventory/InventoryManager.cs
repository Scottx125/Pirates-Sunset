using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _inventoryUIContents;
    [SerializeField]
    private GameObject _abilityActiveContents;
    [SerializeField]
    private List<InventoryObject> _inventory = new List<InventoryObject>();

    private Dictionary<InventoryObject ,InventoryObjectSO> _inventoryObjectsSOsDict = new Dictionary<InventoryObject, InventoryObjectSO>();

    // Store will have a SO of Data that will contain a List of all the InventoryObjects, and a nested List of buy/sell prices.
    // When we send the info. We'll compare the Type to get the relevant price.

    // Inventory will do the same. However, this will hold a list of all the inventorydata.
    // If a new object needs to be made, it'll compare against them to find a match, once it has. It'll instantiate the prefab and get the InventoryObject attatched and add it to the list.
    public void Setup()
    {
        if (_inventoryUIContents == null) {
            Debug.LogError("Need to assign the UI contents object!");
            return;
        }
        // Load IOSO's into the dict so that if we have an object we don't know of we can search for it.
        InventoryObjectSO []inventoryObjectSOsArray = Resources.LoadAll<InventoryObjectSO>("ScriptableObjects/Inventory");
        _inventoryObjectsSOsDict = inventoryObjectSOsArray.ToDictionary(item => item.GetInventoryObjectType, item => item);
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
        InventoryObject existingItem = CheckInventoryForExistingItem(inventoryType);

        if (existingItem == null && _inventoryObjectsSOsDict.ContainsKey(inventoryType))
        {
            // Spawn UI prefabs. Pass itself in the inventory and the ability UI.
            GameObject inventoryUIInstance = Instantiate(_inventoryObjectsSOsDict[inventoryType].GetInventoryUIPrefab, _inventoryUIContents.transform);
            GameObject abilityUIInstance = Instantiate(_inventoryObjectsSOsDict[inventoryType].GetAbilityUIPrefab, _abilityActiveContents.transform);
            InventoryObject instanceInventoryObject = inventoryUIInstance.GetComponent<InventoryObject>();
            instanceInventoryObject.Setup(inventoryUIInstance, abilityUIInstance);
            // Add reference to instance via inventory.
            //_inventory.Add();
            // AddToInventoryQuantity(inventoryObject, amountToAdd);
        }
        else
        {
            AddToInventoryQuantity(existingItem, amountToAdd);
        }
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

    private InventoryObject CheckInventoryForExistingItem(InventoryObject inventoryType)
    {
        // Get the type of the item if it's in the list.
        return _inventory.FirstOrDefault(existing => existing == inventoryType);
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
