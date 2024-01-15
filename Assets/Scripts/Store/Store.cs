using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    [SerializeField]
    private GameObject _storeItemPrefab;
    [SerializeField]
    private InventoryManager _storeInventoryManager;
    // UI STUFF
    [SerializeField]
    private GameObject _storeContentsSection;
    [SerializeField]
    private GameObject _playerContentsSection;
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
    private Dictionary<InventoryObject, StoreItemData> _storeItems = new Dictionary<InventoryObject, StoreItemData>();
    private Dictionary<InventoryObject, StoreItemData> _playerItems = new Dictionary<InventoryObject, StoreItemData>();

    // Hold a list of all the types changed.
    private List<InventoryObject> _changedItems = new List<InventoryObject>();

    // Use this for the selected Item, use it to populate UI to show what is currently being traded.
    private StoreItemData _selectedItem;

    private bool _storeOpen = false;

    public void OpenStore()
    {
        SetupStore();
    }

    private void SetupStore()
    {
        // Reset Store to a base default
        ResetStore();
        // Get player Inventory
        GetPlayerInventory();
        GetStoreInventory();
        // Setup Store UI elements.
    }

    private void GetStoreInventory()
    {
        if (_storeInventoryManager == null)
        {
            Debug.LogError("PlayerInventoryManager not set!");
            return;
        }
        else
        {
            // Assemble the store dictionary.
            StoreItemData[] temp = _storeInventoryManager.GetInventoryCopy();
            for (int i = 0; i < temp.Length; i++)
            {
                _storeItems.Add(temp[i].ItemType, temp[i]);
            }
        }
    }

    private void GetPlayerInventory()
    {
        if (_playerInventoryManager == null)
        {
            Debug.LogError("PlayerInventoryManager not set!");
            return;
        }
        else
        {
            // Assemble the player dictionary.
            StoreItemData[] temp = _playerInventoryManager.GetInventoryCopy();
            for (int i = 0; i < temp.Length; i++)
            {
                _playerItems.Add(temp[i].ItemType, temp[i]);
            }
        }
    }

    private void ResetStore()
    {
        _playerGold = null;
        _storeGold = null;
        _storeItems.Clear();
        _playerItems.Clear();
        _changedItems.Clear();
    }

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerManager>())
        {
            if (_playerInventoryManager == null)
            {
                _playerInventoryManager = other.GetComponent<InventoryManager>();
            }
            OpenStore();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerManager>())
        {
            // Close store if it's not closed already.
        }
    }
}
