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
    public Sprite InventoryObjectUIImage { get; private set; }
    public String InventoryObjectName { get; private set; }
    public int InventoryObjectBuyPrice { get; private set; }
    public int InventoryObjectSellPrice { get; private set; }


    public void Setup(int quantity, string id, Sprite image, string name, int buyPrice, int sellPrice)
    {
        // Setup object variables.
        InventoryObjectQuantity = quantity;
        InventoryObjectId = id;
        InventoryObjectUIImage = image;
        InventoryObjectName = name;
        InventoryObjectBuyPrice = buyPrice;
        InventoryObjectSellPrice = sellPrice;
    }

    public void SubtractQuantity(int subtractValue) 
    { 
         InventoryObjectQuantity -= subtractValue;
    }

    public void AddQuantity(int addValue)
    {
        InventoryObjectQuantity += addValue;
    }
}
