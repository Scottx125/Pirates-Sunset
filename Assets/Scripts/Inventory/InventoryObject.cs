using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public abstract class InventoryObject : MonoBehaviour
{
    [SerializeField]
    private InventoryObjectSO _inventoryObjectData;
    [SerializeField]
    protected int quantity = 0;
    [SerializeField]
    private TextMeshProUGUI _uiNameText;
    [SerializeField]
    private TextMeshProUGUI _uiQuantity;
    [SerializeField]
    private Image _uiButtonImage;

    protected bool bIsActive = false;

    private void Awake()
    {
        // Break this out so that if it's added later, we can assign it through the inven manager.
        if (_uiButtonImage != null) _uiButtonImage.sprite = _inventoryObjectData.GetImage;
        if (_uiNameText != null) _uiNameText.text = _inventoryObjectData.GetName;
        if (_uiQuantity != null) _uiQuantity.text = quantity.ToString();
    }
    public void CheckBehaviour()
    {
        // Ensure object is activateable and is not currently active.
        if (_inventoryObjectData.GetIsActivateableBool == false) return ;
        if (bIsActive == true) return ;
        if (quantity <= 0) return ;
        // Begin coroutine.
        StartCoroutine(InitiateBehaviour());
    }
    private IEnumerator InitiateBehaviour()
    {
        // Stops a new coroutine being fired and sets up variables.
        bIsActive = true;
        float endTime = Time.time + _inventoryObjectData.GetActiveTimeFloat;
        // Handles repeat behaviours.
        if (_inventoryObjectData.GetRepeatBehaviourBool == true)
        {
            while (Time.time < endTime)
            {
                ObjectBehaviour();
                yield return new WaitForSeconds(_inventoryObjectData.GetRepeatTimeFloat);
            }
        } else
        {
            // Single-Fire behaviours.
            ObjectBehaviour();
        }
        // Waits for the cooldown to expire and then exists, fully resetting the behaviour.
        yield return new WaitForSeconds(_inventoryObjectData.GetCooldownFloat);
        bIsActive = false;
    }

    protected virtual void ObjectBehaviour()
    {
        // Implement in child classes.
    }

    public string GetName() { return _inventoryObjectData.GetName; }

    public Sprite GetSprite() { return _inventoryObjectData.GetImage; }
    public int GetQuantity() { return quantity; }

    public void SubtractQuantity(int subtractValue) 
    { 
        quantity -= subtractValue;
        _uiQuantity.text = quantity.ToString();
    }
    public void AddQuantity(int addValue) 
    { 
        quantity += addValue;
        _uiQuantity.text = quantity.ToString();
    }
}
