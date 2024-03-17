using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public abstract class Inventory : MonoBehaviour
{
    public List<InventoryObject> GetInventoryList => _inventoryList;

    protected Dictionary<string, InventoryObjectSO> _inventoryObjectsSOsDict = new Dictionary<string, InventoryObjectSO>();

    [SerializeField]
    private string _goldId;
    [SerializeField]
    private List<InitialInventoryItemAndQuantity> _initialInventory = new List<InitialInventoryItemAndQuantity>();

    private List<InventoryObject> _inventoryList = new List<InventoryObject>();

    private InventoryObject _gold;

    public void Setup()
    {
        if (_goldId == null) Debug.LogError("No Gold data set!");
        // Load IOSO's into the dict so that if we have an object we don't know of we can search for it.
        InventoryObjectSO[] inventoryObjectSOsArray = Resources.LoadAll<InventoryObjectSO>("Inventory/");
        _inventoryObjectsSOsDict = inventoryObjectSOsArray.ToDictionary(item => item.GetId, item => item);
        LoadInitialObjects();
    }

    private void LoadInitialObjects()
    {
        foreach (InitialInventoryItemAndQuantity obj in _initialInventory)
        {
            string id = obj.GetID();
            int quantity = obj.GetQuantity();
            UpdateInventoryQuantity(id, quantity);
            if ( id == _goldId)
            {
                _gold = CheckInventoryForExistingItem(id);
            }
        }
    }

    public void UpdateInventoryQuantity(string itemId, int newQuantity)
    {
        InventoryObject existingItem = CheckInventoryForExistingItem(itemId);

        // If object isn't in the inventory but does exist.
        if (existingItem == null && _inventoryObjectsSOsDict.ContainsKey(itemId))
        {
            // Spawn UI prefabs. Pass itself in the inventory and the ability UI.
            InventoryObject obj = CreateInventoryObject(itemId);
            _inventoryList.Add(obj);
            SetQuantity(obj, newQuantity);
        }
        else
        {
            // If it is in the inventory.
            SetQuantity(existingItem, newQuantity);
        }
    }

    private static void SetQuantity(InventoryObject existingItem, int newQuantity)
    {
        if (newQuantity > 0)
        {
            existingItem.gameObject.SetActive(true);
            existingItem.SetQuantity(newQuantity);
        } else
        if (newQuantity == 0)
        {
            existingItem.SetQuantity(newQuantity);
            existingItem.gameObject.SetActive(false);
        }
        
    }

    protected abstract InventoryObject CreateInventoryObject(string itemId);

    private InventoryObject CheckInventoryForExistingItem(string itemId)
    {
        // Get the type of the item if it's in the list.
        return _inventoryList.FirstOrDefault(existing => existing.InventoryObjectId == itemId);
    }

}
