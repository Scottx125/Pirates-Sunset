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
    private InventoryManager _storeInventoryManager;
    [SerializeField]
    private List<InventoryObjectSO> _allowedObjectsForTrade;
    // UI STUFF
    [SerializeField]
    private GameObject _storeUIPrefab;
    [SerializeField]
    private Transform _storeContentsSection;
    [SerializeField]
    private Transform _playerContentsSection;
    [SerializeField]
    private string _playerGoldId;
    [SerializeField]
    private string _storeGoldId;
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
    private Dictionary<string, InventoryObjectSO> _allowedObjectsDict = new Dictionary<string, InventoryObjectSO>();
    private Dictionary<string, GameObject> _storeInventoryUIDict = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> _playerStoreInventoryUIDict = new Dictionary<string, GameObject>();


    // Use this for the selected Item, use it to populate UI to show what is currently being traded.
    private string _selectedItemId;
    private List<string> _modififedItemsIDs = new List<string>();

    private bool _storeOpen = false;

    private void Awake()
    {
        // Make search of allowed objects easier.
        _allowedObjectsDict = _allowedObjectsForTrade.ToDictionary(item => item.GetId, item => item);

        // Create pool of objects for the store to use.
        for(int i = 0; i < _allowedObjectsForTrade.Count; i++)
        {
            // Spawn objects and add them to the correct sections.
            GameObject playerStoreInventoryUIGameObj = Instantiate(_storeUIPrefab, _playerContentsSection);
            _playerStoreInventoryUIDict.Add(_allowedObjectsForTrade[i].GetId, playerStoreInventoryUIGameObj);
            GameObject storeInventoryUIGameObj = Instantiate(_storeUIPrefab, _storeContentsSection);
            _storeInventoryUIDict.Add(_allowedObjectsForTrade[i].GetId, storeInventoryUIGameObj);

            // Get UI Scripts.
            StoreInventoryUI playerStoreUIScript = playerStoreInventoryUIGameObj.GetComponent<StoreInventoryUI>();
            StoreInventoryUI storeUIScript = storeInventoryUIGameObj.GetComponent<StoreInventoryUI>();

            // Setup the item data.
            StoreItemData itemDataPlayer = new StoreItemData(0, _allowedObjectsForTrade[i].GetId, _allowedObjectsForTrade[i].GetBuyPrice,
                _allowedObjectsForTrade[i].GetSellPrice, _allowedObjectsForTrade[i].GetName, _allowedObjectsForTrade[i].GetImage, playerStoreUIScript);
            StoreItemData itemDataStore = new StoreItemData(0, _allowedObjectsForTrade[i].GetId, _allowedObjectsForTrade[i].GetBuyPrice,
                _allowedObjectsForTrade[i].GetSellPrice, _allowedObjectsForTrade[i].GetName, _allowedObjectsForTrade[i].GetImage, storeUIScript);

            // Setup UI Scripts.
            playerStoreUIScript.Setup(itemDataPlayer);
            storeUIScript.Setup(itemDataStore);

            // Disable until needed.
            playerStoreInventoryUIGameObj.SetActive(false);
            storeInventoryUIGameObj.SetActive(false);
        }
    }

    public void OpenStore()
    {
        SetupStore();
    }

    private void SetupStore()
    {
        // Reset Store to a base default
        ResetStore();
        // Get player Inventory
        SetupPlayerInventory();
        SetupStoreInventory();
        // Setup Store UI elements.
    }

    private void SetupStoreInventory()
    {
        if (_storeInventoryManager == null)
        {
            Debug.LogError("PlayerInventoryManager not set!");
            return;
        }
        else
        {
            // Assemble the store dictionary.
            List<Tuple<string, int>> inventoryData = _storeInventoryManager.GetInventory();
            SetupInventories(inventoryData, _storeInventoryUIDict);
        }
    }
    private void SetupPlayerInventory()
    {
        if (_playerInventoryManager == null)
        {
            Debug.LogError("PlayerInventoryManager not set!");
            return;
        }
        else
        {
            // Assemble the store dictionary.
            List<Tuple<string, int>> inventoryData = _playerInventoryManager.GetInventory();
            SetupInventories(inventoryData, _playerStoreInventoryUIDict);
        }
    }

    private void SetupInventories(List<Tuple<string, int>> inventoryData, Dictionary<string, GameObject> dict)
    {
        foreach (Tuple<string, int> item in inventoryData)
        {
            if (_allowedObjectsDict.ContainsKey(item.Item1))
            {
                GameObject storeUIObj = dict[item.Item1];
                storeUIObj.SetActive(true);
                StoreInventoryUI ui = storeUIObj.GetComponent<StoreInventoryUI>();

                ui.Data.Quantity = item.Item2;
                ui.UpdateUI();
            }
        }
    }

    private void ResetStore()
    {
        foreach(GameObject gameObject in _playerStoreInventoryUIDict.Values)
        {
            gameObject.SetActive(false);
        }
        foreach (GameObject gameObject in _storeInventoryUIDict.Values)
        {
            gameObject.SetActive(false);
        }
        _selectedItemId = null;
        //_changedItems.Clear();
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
