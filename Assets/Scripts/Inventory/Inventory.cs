using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Inventory : MonoBehaviour
{
    [SerializeField]
    protected Transform _inventoryObjectStorage;
    [SerializeField]
    protected GameObject _inventoryObjectPrefab;
    [SerializeField]
    protected List<InventoryObject> _inventory = new List<InventoryObject>();

    protected Dictionary<string, InventoryObjectSO> _inventoryObjectsSOsDict = new Dictionary<string, InventoryObjectSO>();
  
    protected InventoryUIBuilder _inventoryUIBuilder;
    public void Setup(InventoryUIBuilder inventoryUIBuilder = null)
    {
        if (_inventoryObjectStorage == null) Debug.LogError("No inventoryStorage set!");
        // Load IOSO's into the dict so that if we have an object we don't know of we can search for it.
        InventoryObjectSO[] inventoryObjectSOsArray = Resources.LoadAll<InventoryObjectSO>("ScriptableObjects/Inventory");
        _inventoryObjectsSOsDict = inventoryObjectSOsArray.ToDictionary(item => item.GetId, item => item);
        if (_inventoryUIBuilder != null)
        {
            _inventoryUIBuilder = inventoryUIBuilder;
        }
    }

    // Returns the type and quantity of the objects in the inventory.
    public ItemData[] GetInventory()
    {
        ItemData[] copy = new ItemData[_inventory.Count];
        for (int i = 0; i < _inventory.Count; i++)
        {
            copy[i] = new ItemData(_inventory[i].InventoryObjectQuantity, _inventory[i].InventoryObjectId, _inventory[i].InventoryObjectBuyPrice, _inventory[i].InventoryObjectSellPrice, _inventory[i].InventoryObjectName, _inventory[i].InventoryObjectUIImage);
        }
        return copy;
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
    public void AddToInventory(string itemId, int amountToAdd)
    {
        InventoryObject existingItem = CheckInventoryForExistingItem(itemId);
        if (existingItem == null && _inventoryObjectsSOsDict.ContainsKey(itemId))
        {
            // Spawn UI prefabs. Pass itself in the inventory and the ability UI.
            InventoryObject obj = CreateInventoryObject(itemId);
            _inventory.Add(obj);
            AddToInventoryQuantity(obj, amountToAdd);
        }
        else
        {
            AddToInventoryQuantity(existingItem, amountToAdd);
        }
    }
    protected abstract InventoryObject CreateInventoryObject(string itemId);

    private InventoryObject CheckInventoryForExistingItem(string itemId)
    {
        // Get the type of the item if it's in the list.
        return _inventory.FirstOrDefault(existing => existing.InventoryObjectId == itemId);
    }

    private void AddToInventoryQuantity(InventoryObject existingItem, int amountToAdd)
    {
        existingItem.AddQuantity(amountToAdd);
    }

    private void RemoveFromInventoryQuantity(InventoryObject existingItem, int amountToSubtract)
    {
        existingItem.SubtractQuantity(amountToSubtract);
        if (existingItem.InventoryObjectQuantity <= 0)
        {
            _inventory.Remove(existingItem);
            Destroy(existingItem.gameObject);
        }
    }
}
