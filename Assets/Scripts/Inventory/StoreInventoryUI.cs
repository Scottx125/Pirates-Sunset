using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreInventoryUI : InventoryUI
{
    public override void Setup(string name, int quantity, Sprite mainImage, int buyPrice = 0, int sellPrice = 0)
    {
        _inventoryUIName.text = name;
        _inventoryUIQuantity.text = quantity.ToString();
        _inventoryUIImage.sprite = mainImage;
        _inventoryUIBuyPrice.text = buyPrice.ToString();
        _inventoryUISellPrice.text = sellPrice.ToString();
        _inventoryUICooldown.fillAmount = 0f;
    }
}
