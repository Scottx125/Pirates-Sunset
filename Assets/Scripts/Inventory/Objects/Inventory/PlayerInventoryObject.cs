using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryObject : InventoryObject
{
    private PlayerInventoryUI _pInventoryUI;

    public void Setup(string id, PlayerInventoryUI pInventoryUI)
    {
        // Setup object variables.
        InventoryObjectId = id;
        _pInventoryUI = pInventoryUI;
        UpdateUI();
    }

    private void UpdateUI()
    {
        _pInventoryUI.UpdateUIQuantity(GetInventoryObjectQuantity);
    }

    public override void SetQuantity(int quantity)
    {
        _inventoryObjectQuantity = quantity;
        UpdateUI();
    }

    private void OnDisable()
    {
        _pInventoryUI.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        _pInventoryUI.gameObject.SetActive(true);
    }
}
