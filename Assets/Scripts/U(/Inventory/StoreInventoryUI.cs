using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreInventoryUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _inventoryUIName;
    [SerializeField]
    private TMP_Text _inventoryUIQuantity;
    [SerializeField]
    private Image _inventoryUIImage;
    [SerializeField]
    private TMP_Text _inventoryUIBuyPrice;
    [SerializeField]
    private TMP_Text _inventoryUISellPrice;

    public StoreItemData Data { get; private set; }

    private IStoreInventoryUISelected _store;

    public void Setup(StoreItemData data, IStoreInventoryUISelected store)
    {
        Data = data;
        _store = store;

        if (_inventoryUIName != null) _inventoryUIName.text = data.Name;
        if (_inventoryUIImage != null) _inventoryUIImage.sprite = data.Image;
        if (_inventoryUIQuantity != null) _inventoryUIQuantity.text = data.TempQuantity.ToString();
        if (_inventoryUIBuyPrice != null) _inventoryUIBuyPrice.text = data.BuyPrice.ToString();
        if (_inventoryUISellPrice != null) _inventoryUISellPrice.text = data.SellPrice.ToString();
    }

    public void OnSelect()
    {
        _store.OnInventoryUISelected(Data.ItemId);
    }
    public void UpdateUI()
    {
        _inventoryUIQuantity.text = Data.TempQuantity.ToString();
    }
}
