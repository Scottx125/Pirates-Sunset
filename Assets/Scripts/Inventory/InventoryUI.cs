using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class InventoryUI : MonoBehaviour
{
    [SerializeField]
    protected TMP_Text _inventoryUIName;
    [SerializeField]
    protected TMP_Text _inventoryUIQuantity;
    [SerializeField]
    protected TMP_Text _inventoryUIBuyPrice;
    [SerializeField]
    protected TMP_Text _inventoryUISellPrice;
    [SerializeField]
    protected Image _inventoryUIImage;
    [SerializeField]
    protected Image _inventoryUICooldown;
    public abstract void Setup(string name, int quantity = 0, Sprite mainImage, int buyPrice = 0, int sellPrice = 0);

    public void UpdateUIQuantity(int quantity)
    {
        _inventoryUIQuantity.text = quantity.ToString();
    }

    public IEnumerator UILerpFill(float duration, float abilityLength)
    {
        while (Time.time < duration)
        {
            float timeRemaining = duration - Time.time;
            _inventoryUICooldown.fillAmount = Mathf.Lerp(1, 0, timeRemaining / abilityLength);
            yield return null;
        }
    }
}
