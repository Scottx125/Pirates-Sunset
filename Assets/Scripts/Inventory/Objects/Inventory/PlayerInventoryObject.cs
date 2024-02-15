using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryObject : InventoryObject
{
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

    public override void SetQuantity(int quantity)
    {
        InventoryObjectQuantity = quantity;
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
