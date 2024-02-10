using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItemData
{
    public int TempQuantity { get; set; }
    public int Quantity { get; private set; }
    public string ItemId { get; private set; }
    public int BuyPrice { get; private set; }
    public int SellPrice { get; private set; }
    public string Name { get; private set; }
    public Sprite Image { get; private set; }
    public StoreItemData(int quantity, string itemId, int buyPrice, int sellPrice, string name, Sprite image, StoreInventoryUI uiScript)
    {
        Quantity = quantity;
        TempQuantity = quantity;
        ItemId = itemId;
        BuyPrice = buyPrice;
        SellPrice = sellPrice;
        Name = name;
        Image = image;
    }

    public void ApplyQuantity()
    {
        Quantity = TempQuantity;
    }
    public void ResetQuantity()
    {
        TempQuantity = Quantity;
    }
}
