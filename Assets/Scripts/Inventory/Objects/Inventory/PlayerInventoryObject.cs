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

    public override void AddQuantity(int addValue)
    {
        InventoryObjectQuantity += addValue;
        UpdateUI();
    }

    public override void SubtractQuantity(int subtractValue)
    {
        InventoryObjectQuantity -= subtractValue;
        UpdateUI();
    }
}
