using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.UI;
public abstract class InventoryObject : MonoBehaviour
{
    [SerializeField]
    private InventoryObjectSO _inventoryObjectData;
    [SerializeField]
    protected int quantity = 0;
    // UI stuff
    [SerializeField]
    private TextMeshProUGUI _uiNameText;
    [SerializeField]
    private TextMeshProUGUI _uiQuantity;
    [SerializeField]
    private Image _uiButtonImage;
    [SerializeField]
    private Image _uiActiveImage;
    [SerializeField]
    private Image _uiCooldownImage;


    protected bool bIsActive = false;

    private Color _transparent = new Color(1f,1f,1f,0f);
    private Color _visible = new Color(1f, 1f, 1f, 1f);
    private void Awake()
    {
        // Break this out so that if it's added later, we can assign it through the inven manager.
        if (_uiButtonImage != null) _uiButtonImage.sprite = _inventoryObjectData.GetImage;
        if (_uiNameText != null) _uiNameText.text = _inventoryObjectData.GetName;
        if (_uiQuantity != null) _uiQuantity.text = quantity.ToString();
        _uiActiveImage.color = _transparent;
        _uiCooldownImage.color = _transparent;
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
        _uiActiveImage.color = _visible;
        _uiButtonImage.color = _transparent;
        // Handles repeat behaviours.
        if (_inventoryObjectData.GetRepeatBehaviourBool == true)
        {
            // Repeat behaviours.
            while (Time.time < endTime)
            {
                ObjectBehaviour();
                yield return new WaitForSeconds(_inventoryObjectData.GetRepeatTimeFloat);
            }
        } else
        {
            // For duration behaviours only.
            if (_inventoryObjectData.GetRepeatBehaviourBool == false && _inventoryObjectData.GetActiveTimeFloat > 0f)
            {
                // Objects that are duration behaviours will be a toggle like system. Activating and then deactivating when called twice.
                ObjectBehaviour();
                yield return new WaitForSeconds(_inventoryObjectData.GetActiveTimeFloat);
                ObjectBehaviour();
            }
            // Single-Fire behaviours.
            ObjectBehaviour();
        }
        _uiCooldownImage.color = _visible;
        _uiActiveImage.color = _transparent;
        // Waits for the cooldown to expire and then exits, fully resetting the behaviour.
        yield return new WaitForSeconds(_inventoryObjectData.GetCooldownFloat);
        _uiButtonImage.color = _visible;
        _uiCooldownImage.color = _transparent;
        bIsActive = false;
    }

    protected virtual void ObjectBehaviour()
    {
        // Implement in child classes.
    }
    public int GetBuyPrice() { return _inventoryObjectData.GetBuyPrice; }
    public int GetSellPrice() { return _inventoryObjectData.GetSellPrice; }
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
