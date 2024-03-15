using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private Inventory _inventory;

    // Will simply hold all the aspects for the inventory system.

    public void Setup()
    {
        _inventory.Setup();
    }

    // Returns the Id and Quantity of the objects in the inventory.
    public List<Tuple<string, int>> GetInventory()
    {
        List<Tuple<string, int>> copy = new List<Tuple<string, int>>();
        foreach (InventoryObject obj in _inventory.GetInventoryList)
        {
            copy.Add(Tuple.Create(obj.InventoryObjectId, obj.GetInventoryObjectQuantity));
        }
        return copy;
    }

    public void AddOrRemoveFromInventory(string id, int quantity)
    {
        _inventory.UpdateInventoryQuantity(id, quantity);
    }
}
