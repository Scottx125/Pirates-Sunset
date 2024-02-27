using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour, IStoreInventoryUISelected, IStoreSliderUpdate
{

    [SerializeField]
    private InventoryManager _storeInventoryManager;
    [SerializeField]
    private InventoryManager _playerInventoryManager;
    [SerializeField]
    private List<InventoryObjectSO> _allowedObjectsForTrade;

    // UI STUFF
    [SerializeField]
    private Image _selectedObjectImage;
    [SerializeField]
    private TMP_Text _selectedObjectText;
    [SerializeField]
    private TMP_Text _selectedObjectStoreQuantity;
    [SerializeField]
    private TMP_Text _selectedObjectPlayerQuantity;
    [SerializeField]
    private StoreSlider _storeSlider;
    [SerializeField]
    private GameObject _storeUIPrefab;
    [SerializeField]
    private GameObject _storeUIGoldPrefab;
    [SerializeField]
    private Transform _storeContentsSection;
    [SerializeField]
    private Transform _playerContentsSection;

    // Gold stuff
    [SerializeField]
    private Transform _storeGoldContentSection;
    [SerializeField]
    private Transform _playerGoldContentsSection;
    [SerializeField]
    private InventoryObjectSO _goldData;

    // LOCAL CACHED DATA.
    // Gold
    private string _goldId;
    private StoreInventoryUI _playerGold;
    private StoreInventoryUI _storeGold;


    // Objects currently displayed on screen in the store.
    private Dictionary<string, InventoryObjectSO> _allowedObjectsDict = new Dictionary<string, InventoryObjectSO>();
    private Dictionary<string, StoreInventoryUI> _storeInventoryUIDict = new Dictionary<string, StoreInventoryUI>();
    private Dictionary<string, StoreInventoryUI> _playerStoreInventoryUIDict = new Dictionary<string, StoreInventoryUI>();

    // Get the Gameobject with .gameobject and access the data with the .Data accessor.

    // Use this for the selected Item, use it to populate UI to show what is currently being traded.
    private string _selectedItemId;

    private void Awake()
    {
        // Make search of allowed objects easier.
        _allowedObjectsDict = _allowedObjectsForTrade.ToDictionary(item => item.GetId, item => item);


        // Cache local variables.
        if (CacheLocalVariables() == false)
        {
            Debug.LogError("Store Awake method failed due to an issue with caching local variables!");
            return;
        }

        // Create pool of objects for the store to use.
        for (int i = 0; i < _allowedObjectsForTrade.Count; i++)
        {
            // Check for gold.
            if (_allowedObjectsForTrade[i].GetId == _goldId)
            {
                SetupGold();
                continue;
            }
            SetupStoreItemPool(i);
        }
    }
    private bool CacheLocalVariables()
    {
        if (_goldData == null)
        {
            Debug.LogError("No Gold data set!");
            return false;
        }
        _goldId = _goldData.GetId;
        if (_storeSlider == null)
        {
            Debug.LogError("No StoreSlider set!");
            return false;
        }
        _storeSlider.Setup(this);
        if (_selectedObjectImage == null)
        {
            Debug.LogError("No SelectedObjectImage set!");
            return false;
        }
        if (_selectedObjectText == null)
        {
            Debug.LogError("No SelectedObjectText set!");
            return false;
        }
        return true;
    }
    public void OpenStore()
    {
        SetupStore();
    }
    public void CloseStore()
    {
        // disable store ui
        gameObject.SetActive(false);
    }
    public void OnInventoryUISelected(string id)
    {
        if (id == _selectedItemId) return;

        ResetPreviouslySelectedItem(id);
        SetupSelectedItem(id);
    }
    private void SetupSelectedItem(string id)
    {

        // Ensure new selection is cached.
        _selectedItemId = id;

        // Cache local variables.
        StoreItemData playerItemData = _playerStoreInventoryUIDict[_selectedItemId].Data;
        StoreItemData storeItemData = _storeInventoryUIDict[_selectedItemId].Data;
        int sellPrice = _allowedObjectsDict[id].GetSellPrice;
        int buyPrice = _allowedObjectsDict[id].GetBuyPrice;

        // Reset UI.
        _playerGold.UpdateUI();
        _storeGold.UpdateUI();

        // Set Image and Show.
        _selectedObjectImage.sprite = _allowedObjectsDict[_selectedItemId].GetImage;
        _selectedObjectImage.color = Color.white;

        // Set text
        _selectedObjectText.text = _allowedObjectsDict[_selectedItemId].GetName;
        _selectedObjectPlayerQuantity.text = playerItemData.TempQuantity.ToString();
        _selectedObjectStoreQuantity.text = storeItemData.TempQuantity.ToString();

        // Determine how much we can buy/sell with the current gold amount and prices.
        // Then determine if the theoretical maximum is greater or smaller than the item quantity.
        int maxSellable = (int)Mathf.Floor(_storeGold.Data.Quantity / sellPrice);
        maxSellable = maxSellable > playerItemData.Quantity ? playerItemData.Quantity : maxSellable;
        int maxBuyable = (int)Mathf.Floor(_playerGold.Data.Quantity / buyPrice);
        maxBuyable = maxBuyable > storeItemData.Quantity ? storeItemData.Quantity : maxBuyable;

        // Setup slider.
        _storeSlider.MaxMinSliderValues(maxSellable, maxBuyable);
        
        //NEED TO IMPLEMENT SETUP OF SELECTED TITLE AND IMAGE.
        // AND RESET THEM.
    }
    private void ResetPreviouslySelectedItem(string id)
    {
        // Simple UI referesh. Resets data to it's correct value and updates the UI.
        _playerGold.Data.ResetQuantity();
        _playerGold.UpdateUI();
        _storeGold.Data.ResetQuantity();
        _storeGold.UpdateUI();
        _selectedObjectImage.color = Color.clear;
        _selectedObjectText.text = "None";
        _selectedObjectPlayerQuantity.text = "0";
        _selectedObjectStoreQuantity.text = "0";
        _storeInventoryUIDict[id].Data.ResetQuantity();
        _storeInventoryUIDict[id].UpdateUI();
        _playerStoreInventoryUIDict[id].Data.ResetQuantity();
        _playerStoreInventoryUIDict[id].UpdateUI();
        _storeSlider.MaxMinSliderValues(1, 1);
    }
    public void StoreSliderUpdateUI(int amount)
    {
        // Used current selected ID.
        // This will be clamped to between max buy/sell amount.
        // As the slider changes, the player gold and store gold will change.

        if (amount == 0) return;

        // Cache variables.
        StoreItemData playerItemData = _playerStoreInventoryUIDict[_selectedItemId].Data;
        StoreItemData storeItemData = _storeInventoryUIDict[_selectedItemId].Data;
        // Reset quantity.
        playerItemData.ResetQuantity();
        storeItemData.ResetQuantity();
        _playerGold.Data.ResetQuantity();
        _storeGold.Data.ResetQuantity();
        // Add new quantity.
        storeItemData.TempQuantity = storeItemData.Quantity + (-1 * amount);
        playerItemData.TempQuantity = playerItemData.Quantity + (-1 * -amount);
        // Update gold
        int price = amount < 0 ? _allowedObjectsDict[_selectedItemId].GetSellPrice : _allowedObjectsDict[_selectedItemId].GetBuyPrice;
        int cost = price * Math.Abs(amount);
        _storeGold.Data.TempQuantity = _storeGold.Data.Quantity + (Math.Sign(-1 * amount) * cost);
        _playerGold.Data.TempQuantity = _playerGold.Data.Quantity + (Math.Sign(-1 * -amount) * cost);

        // Update UI
        _selectedObjectPlayerQuantity.text = playerItemData.TempQuantity.ToString();
        _selectedObjectStoreQuantity.text = storeItemData.TempQuantity.ToString();
        _playerGold.UpdateUI();
        _storeGold.UpdateUI();
    }
    public void ApplyTrade()
    {
        // Whatever items are currently being traded are applied.
        StoreItemData playerItem = _playerStoreInventoryUIDict[_selectedItemId].Data;
        StoreItemData storeItem = _storeInventoryUIDict[_selectedItemId].Data;

        playerItem.ApplyQuantity();
        storeItem.ApplyQuantity();
        _playerGold.Data.ApplyQuantity();
        _storeGold.Data.ApplyQuantity();

        // Update via the main inventory via the manager.
        _playerInventoryManager.AddOrRemoveFromInventory(_selectedItemId, playerItem.Quantity);
        _playerInventoryManager.AddOrRemoveFromInventory(_goldId, _playerGold.Data.Quantity);
        _storeInventoryManager.AddOrRemoveFromInventory(_selectedItemId, storeItem.Quantity);
        _storeInventoryManager.AddOrRemoveFromInventory(_goldId, _storeGold.Data.Quantity);

        // Reset and then reload currently selected items to reflect changes.
        ResetPreviouslySelectedItem(_selectedItemId);
        SetupSelectedItem(_selectedItemId);
    }

    private void SetupStoreItemPool(int i)
    {
        // Spawn objects.
        GameObject playerStoreInventoryUIGameObj = Instantiate(_storeUIPrefab, _playerContentsSection);
        GameObject storeInventoryUIGameObj = Instantiate(_storeUIPrefab, _storeContentsSection);

        // Get UI Scripts.
        StoreInventoryUI playerStoreUIScript = playerStoreInventoryUIGameObj.GetComponent<StoreInventoryUI>();
        StoreInventoryUI storeUIScript = storeInventoryUIGameObj.GetComponent<StoreInventoryUI>();

        // Add them to the correct dicts.
        _playerStoreInventoryUIDict.Add(_allowedObjectsForTrade[i].GetId, playerStoreUIScript);
        _storeInventoryUIDict.Add(_allowedObjectsForTrade[i].GetId, storeUIScript);

        // Setup the item data.
        StoreItemData itemDataPlayer = new StoreItemData(0, _allowedObjectsForTrade[i].GetId,
            _allowedObjectsForTrade[i].GetName, _allowedObjectsForTrade[i].GetImage, playerStoreUIScript);
        StoreItemData itemDataStore = new StoreItemData(0, _allowedObjectsForTrade[i].GetId,
            _allowedObjectsForTrade[i].GetName, _allowedObjectsForTrade[i].GetImage, storeUIScript);

        // Setup UI Scripts.
        playerStoreUIScript.Setup(itemDataPlayer, _allowedObjectsForTrade[i].GetSellPrice, this);
        storeUIScript.Setup(itemDataStore, _allowedObjectsForTrade[i].GetBuyPrice, this);
    }
    // Same as item pool except is specific for gold.
    private void SetupGold()
    {
        // Spawn objects.
        GameObject playerStoreGoldUIGameObj = Instantiate(_storeUIGoldPrefab, _playerGoldContentsSection);
        GameObject storeGoldUIGameObj = Instantiate(_storeUIGoldPrefab, _storeGoldContentSection);

        // Get UI Scripts.
        StoreInventoryUI playerStoreGoldUIScript = playerStoreGoldUIGameObj.GetComponent<StoreInventoryUI>();
        StoreInventoryUI storeGoldUIScript = storeGoldUIGameObj.GetComponent<StoreInventoryUI>();

        // Add to the correct dicts.
        _playerStoreInventoryUIDict.Add(_goldId, playerStoreGoldUIScript);
        _storeInventoryUIDict.Add(_goldId, storeGoldUIScript);

        // Setup the item data.
        StoreItemData itemDataPlayer = new StoreItemData(0, _goldId, _allowedObjectsDict[_goldId].GetName, 
            _allowedObjectsDict[_goldId].GetImage, playerStoreGoldUIScript);
        StoreItemData itemDataStore = new StoreItemData(0, _goldId, _allowedObjectsDict[_goldId].GetName, 
            _allowedObjectsDict[_goldId].GetImage, storeGoldUIScript);

        // Setup UI Scripts.
        playerStoreGoldUIScript.Setup(itemDataPlayer, _allowedObjectsDict[_goldId].GetSellPrice, this);
        storeGoldUIScript.Setup(itemDataStore, _allowedObjectsDict[_goldId].GetBuyPrice, this);

        // Cache the store and player gold data scripts.
        _playerGold = playerStoreGoldUIScript;
        _storeGold = storeGoldUIScript;
    }
    private void SetupStore()
    {
        // Activate Store UI
        // Reset Store to a base default
        ResetStore();
        // Get player Inventory
        SetupPlayerInventory();
        SetupStoreInventory();
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
    private void SetupInventories(List<Tuple<string, int>> inventoryData, Dictionary<string, StoreInventoryUI> dict)
    {
        foreach (Tuple<string, int> item in inventoryData)
        {
            if (_allowedObjectsDict.ContainsKey(item.Item1))
            {
                // If the object is in the dict.
                // aquire it and set it up.
                StoreInventoryUI ui = dict[item.Item1];
                ui.Data.TempQuantity = item.Item2;
                ui.Data.ApplyQuantity();
                ui.UpdateUI();
            }
        }
    }
    private void ResetStore()
    {
        // Reset everyhting to zero.
        foreach(StoreInventoryUI ui in _playerStoreInventoryUIDict.Values)
        {
            ui.Data.TempQuantity = 0;
            ui.Data.ApplyQuantity();
            ui.UpdateUI();
        }
        foreach (StoreInventoryUI ui in _storeInventoryUIDict.Values)
        {
            ui.Data.TempQuantity = 0;
            ui.Data.ApplyQuantity();
            ui.UpdateUI();
        }
        _selectedItemId = null;
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

    private void OnEnable()
    {
        OpenStore();
    }
    private void OnDisable()
    {
        CloseStore();
    }
}
