using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _inventoryUIName;
    [SerializeField]
    private TMP_Text _inventoryUIQuantity;
    [SerializeField]
    private Image _inventoryUIImage;
    [SerializeField]
    private Image _inventoryUICooldown;
    public void Setup(string name, int quantity, Sprite mainImage)
    {
        _inventoryUIName.text = name;
        _inventoryUIQuantity.text = quantity.ToString();
        _inventoryUIImage.sprite = mainImage;
        _inventoryUICooldown.fillAmount = 0f;
    }

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
