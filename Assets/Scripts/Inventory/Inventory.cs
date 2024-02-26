using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Inventory : MonoBehaviour
{
    public List<InventoryObject> GetInventoryList => _inventoryList;

    protected Dictionary<string, InventoryObjectSO> _inventoryObjectsSOsDict = new Dictionary<string, InventoryObjectSO>();

    [SerializeField]
    private InventoryObjectSO _goldData;
    [SerializeField]
    private List<InventoryObject> _inventoryList = new List<InventoryObject>();

    private string _goldId;
    private InventoryObject _gold;
    public void Setup()
    {
        if (_goldData == null) Debug.LogError("No Gold data set!");
        _goldId = _goldData.GetId;
        // Load IOSO's into the dict so that if we have an object we don't know of we can search for it.
        InventoryObjectSO[] inventoryObjectSOsArray = Resources.LoadAll<InventoryObjectSO>("ScriptableObjects/Inventory");
        _inventoryObjectsSOsDict = inventoryObjectSOsArray.ToDictionary(item => item.GetId, item => item);
        _gold = CheckInventoryForExistingItem(_goldId);
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
            UpdateInventoryObjectQuantity(obj, newQuantity);
        }
        else
        {
            // If it is in the inventory.
            UpdateInventoryObjectQuantity(existingItem, newQuantity);
        }
    }

    private void UpdateInventoryObjectQuantity(InventoryObject existingItem, int newQuantity)
    {
        // Gold is the only object we never disable. As it's needed when going into the store.
        if (existingItem == _gold)
        {
            SetQuantity(existingItem, newQuantity);
            return;
        }
        // If we're setting the quantity to 0, keep in list but disable the object.
        SetQuantityZero(existingItem, newQuantity);
        SetQuantity(existingItem, newQuantity);

    }

    private static void SetQuantity(InventoryObject existingItem, int newQuantity)
    {
        if (newQuantity > 0)
        {
            existingItem.gameObject.SetActive(true);
            existingItem.SetQuantity(newQuantity);
        }
    }

    private static void SetQuantityZero(InventoryObject existingItem, int newQuantity)
    {
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
