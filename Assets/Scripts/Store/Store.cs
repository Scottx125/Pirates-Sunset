using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    [SerializeField]
    private GameObject _storeItemPrefab;
    [SerializeField]
    private InventoryManager _storeInventoryManager;

    [SerializeField]
    private TextMeshProUGUI _playerGoldTextComponenet;
    [SerializeField]
    private TextMeshProUGUI _storeGoldTextComponenet;
    [SerializeField]
    private Image _playerGoldComponenet;
    [SerializeField]
    private Image _storeGoldComponenet;

    private InventoryManager _playerInventoryManager;

    private StoreItemData _playerGold;
    private StoreItemData _storeGold;
    
    // Objects currently displayed on screen in the store.
    private List<StoreItem> _storeItems = new List<StoreItem>();
    private List<StoreItem> _playerItems = new List<StoreItem>();


    // Open store method. Handles setting up the store attatched to a collider or button.
    // Will call Setup Store

    // Setup Store
    // Will call methods responsible for setting up store.
    // Will first attempt to aquire player inventory if not aquired..

    // Setup StoreItems
    // Will go through inventories creating the store items and populating the UI. Will also isolate gold for each inventory and
    // set player and store gold.
    // StoreItem will hold UI elements.
    
    // Buy/Sell
    // Store will have a selected StoreItem element. And then buy/sell buttons at top.
    // When you click on an item (List will be sorted alphabetically) it will get the reference for the selected object and use that to select both the store and players version.
    // When you hit buy, the storeitem will compare price to player gold.
    // If it has enough gold for the quantity purchased, it will remove the gold.
    // And add the quantity.

}
public class StoreItemData
{
    public int Quantity { get; set; }
    public int BuyPrice { get; private set; }
    public int SellPrice { get; private set; }
    public string Name { get; private set; }
    public Sprite Image { get; private set; }
    public StoreItemData(int quantity, int buyPrice, int sellPrice, string name, Sprite image)
    {
        Quantity = quantity;
        BuyPrice = buyPrice;
        SellPrice = sellPrice;
        Name = name;
        Image = image;
    }
}
