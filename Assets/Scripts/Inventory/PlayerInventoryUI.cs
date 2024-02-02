using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryUI : InventoryUI
{
    public override void Setup(string name, int quantity, Sprite mainImage, int buyPrice = 0, int sellPrice = 0)
    {
        _inventoryUIName.text = name;
        _inventoryUIQuantity.text = quantity.ToString();
        _inventoryUIImage.sprite = mainImage;
        _inventoryUICooldown.fillAmount = 0f;
    }
}
