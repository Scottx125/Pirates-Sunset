using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private bool _isStore = false;
    [SerializeField]
    private List<InventoryObject> _inventory = new List<InventoryObject>();

    private Dictionary<InventoryObject, InventoryObjectSO> _inventoryObjectsSOsDict = new Dictionary<InventoryObject, InventoryObjectSO>();

    private InventoryUIBuilder _inventoryUIBuilder = null;
  
    public void Setup(bool isStore, InventoryUIBuilder _uiBuilder = null)
    {
        // Load IOSO's into the dict so that if we have an object we don't know of we can search for it.
        InventoryObjectSO[] inventoryObjectSOsArray = Resources.LoadAll<InventoryObjectSO>("ScriptableObjects/Inventory");
        _inventoryObjectsSOsDict = inventoryObjectSOsArray.ToDictionary(item => item.GetInventoryObjectType, item => item);
        _isStore = isStore;
        _inventoryUIBuilder = _uiBuilder;
    }

    // Returns the type and quantity of the objects in the inventory.
    public StoreItemData[] GetInventory()
    {
        StoreItemData[] copy = new StoreItemData[_inventory.Count];
        for (int i = 0; i < _inventory.Count; i++)
        {
            copy[i] = new StoreItemData(_inventory[i].GetQuantity(), _inventory[i], _inventory[i].GetBuyPrice(), _inventory[i].GetSellPrice(), _inventory[i].GetName(), _inventory[i].GetSprite());
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
    public void AddToInventory(InventoryObject inventoryType, int amountToAdd)
    {
        InventoryObject existingItem = CheckInventoryForExistingItem(inventoryType);

        if (existingItem == null && _inventoryObjectsSOsDict.ContainsKey(inventoryType))
        {
            // Spawn UI prefabs. Pass itself in the inventory and the ability UI.
            InventoryObject obj = CreateInventoryObject(inventoryType);
            _inventory.Add(obj);
            AddToInventoryQuantity(obj, amountToAdd);
        }
        else
        {
            AddToInventoryQuantity(existingItem, amountToAdd);
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
}
