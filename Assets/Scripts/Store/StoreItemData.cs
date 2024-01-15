using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItemData
{
    public int Quantity { get; set; }
    public InventoryObject ItemType { get; private set; }
    public int BuyPrice { get; private set; }
    public int SellPrice { get; private set; }
    public string Name { get; private set; }
    public Sprite Image { get; private set; }
    public StoreItemData(int quantity, InventoryObject type, int buyPrice, int sellPrice, string name, Sprite image)
    {
        Quantity = quantity;
        ItemType = type;
        BuyPrice = buyPrice;
        SellPrice = sellPrice;
        Name = name;
        Image = image;
    }
}
