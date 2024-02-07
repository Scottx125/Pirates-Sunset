using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _inventoryUIName;
    [SerializeField]
    private TMP_Text _inventoryUIQuantity;
    [SerializeField]
    private Image _inventoryUIImage;
    [SerializeField]
    private Image _inventoryUICooldown;
    public void Setup(string name, Sprite mainImage)
    {
        _inventoryUIName.text = name;
        _inventoryUIQuantity.text = "0";
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
