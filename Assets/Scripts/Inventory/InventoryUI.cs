using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _inventoryUIName;
    [SerializeField]
    private bool _inventoryUINameRequired = false;
    [SerializeField]
    private TMP_Text _inventoryUIQuantity;
    [SerializeField]
    private bool _inventoryUIQuantityRequired = false;
    [SerializeField]
    private Image _inventoryUIImage;
    [SerializeField]
    private Image _inventoryUICooldown;
    public TMP_Text Name { get;private set; }
    public TMP_Text Quantity { get; private set; }
    public Image UIImage { get; private set; }
    public Image UICooldownImage { get; private set; }
    private void Awake()
    {
        if(_inventoryUIName != null && _inventoryUINameRequired == true)
        {
            Name = _inventoryUIName;
        } else { Debug.LogError("InventoryUI fields not setup correctly!"); }

        if (_inventoryUIQuantity != null && _inventoryUIQuantityRequired == true)
        {
            Quantity = _inventoryUIQuantity;
        }
        else { Debug.LogError("InventoryUI fields not setup correctly!"); }

        if (_inventoryUIImage != null)
        {
            UIImage = _inventoryUIImage;
        }
        else { Debug.LogError("InventoryUI fields not setup correctly!"); }

        if (_inventoryUICooldown != null)
        {
            UICooldownImage = _inventoryUICooldown;
        }
        else { Debug.LogError("InventoryUI fields not setup correctly!"); }
    }
}
