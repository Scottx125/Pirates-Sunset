using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreInventoryUI : MonoBehaviour
{
    [SerializeField]
    protected TMP_Text _inventoryUIName;
    [SerializeField]
    protected TMP_Text _inventoryUIQuantity;
    [SerializeField]
    protected Image _inventoryUIImage;
    [SerializeField]
    protected TMP_Text _inventoryUIBuyPrice;
    [SerializeField]
    protected TMP_Text _inventoryUISellPrice;
    public void Setup(string name, int quantity, Sprite mainImage, int buyPrice = 0, int sellPrice = 0)
    {
        _inventoryUIName.text = name;
        _inventoryUIQuantity.text = quantity.ToString();
        _inventoryUIImage.sprite = mainImage;
        _inventoryUIBuyPrice.text = buyPrice.ToString();
        _inventoryUISellPrice.text = sellPrice.ToString();
    }
    public void UpdateUIQuantity(int quantity)
    {
        _inventoryUIQuantity.text = quantity.ToString();
    }
}
