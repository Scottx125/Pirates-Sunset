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

    private PlayerInventoryUI _pInventoryUI;

    public void Setup(string id, PlayerInventoryUI pInventoryUI)
    {
        // Setup object variables.
        InventoryObjectQuantity = 0;
        InventoryObjectId = id;
        _pInventoryUI = pInventoryUI;
    }

    private void UpdateUI()
    {
        _pInventoryUI.UpdateUIQuantity(InventoryObjectQuantity);
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
