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
    private TMP_Text _inventoryUIPrice;

    public StoreItemData Data { get; private set; }

    private IStoreInventoryUISelected _store;

    public void Setup(StoreItemData data, int price, IStoreInventoryUISelected store)
    {
        Data = data;
        _store = store;

        if (_inventoryUIName != null) _inventoryUIName.text = data.Name;
        if (_inventoryUIImage != null) _inventoryUIImage.sprite = data.Image;
        if (_inventoryUIQuantity != null) _inventoryUIQuantity.text = data.TempQuantity.ToString();
        if (_inventoryUIPrice != null) _inventoryUIPrice.text = price.ToString();
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
