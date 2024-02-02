using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
public class InventoryObject : MonoBehaviour
{
    public int InventoryObjectQuantity { get; private set; }
    public string InventoryObjectId { get; private set; }

    private InventoryUI _inventoryUI;

    public void Setup(int quantity, string id, InventoryUI inventoryUI)
    {
        // Setup object variables.
        InventoryObjectQuantity = quantity;
        InventoryObjectId = id;
        _inventoryUI = inventoryUI;
    }

    private void UpdateUI()
    {
        _inventoryUI.UpdateUIQuantity(InventoryObjectQuantity);
    }

    public void SubtractQuantity(int subtractValue) 
    { 
         InventoryObjectQuantity -= subtractValue;
        UpdateUI();
    }

    public void AddQuantity(int addValue)
    {
        InventoryObjectQuantity += addValue;
        UpdateUI();
    }
}
