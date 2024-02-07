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

    public void Setup(StoreItemData data)
    {
        Data = data;
        // Other stuff
    }
    public void UpdateUI()
    {
        _inventoryUIQuantity.text = Data.Quantity.ToString();
    }
}
