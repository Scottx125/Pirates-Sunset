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
    private InventoryUI _inventoryUI;
    private InventoryUI _abilityHUD;
    private GameObject _inventoryUIAsset;
    private GameObject _abilityHUDAsset;



    protected bool bIsActive = false;
    public void Setup(GameObject inventoryUIAsset, GameObject abilityUIAsset)
    {
        // Setup cached variables.
        _inventoryUIAsset = inventoryUIAsset;
        _abilityHUDAsset = abilityUIAsset;
        _inventoryUI = inventoryUIAsset.GetComponent<InventoryUI>();
        _abilityHUD = inventoryUIAsset.GetComponent<InventoryUI>();

        // Set text of these variables.
        _inventoryUI.Name.text = _inventoryObjectData.GetName;
        _inventoryUI.UIImage.sprite = _inventoryObjectData.GetImage;
        _inventoryUI.Quantity.text = quantity.ToString();
        _inventoryUI.UICooldownImage.fillAmount = 0;
        _abilityHUD.UIImage.sprite = _inventoryObjectData.GetImage;
        _abilityHUD.UICooldownImage.fillAmount = 0;
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
        // Waits for the cooldown to expire and then exits, fully resetting the behaviour.
        float cooldownTime = Time.time + _inventoryObjectData.GetCooldownFloat;
        while (Time.time < cooldownTime)
        {
            float timeRemaining = cooldownTime - Time.time;
            _inventoryUI.UICooldownImage.fillAmount = Mathf.Lerp(1, 0, timeRemaining / _inventoryObjectData.GetCooldownFloat);
            yield return null;
        }
        
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
        UpdateUI();
    }
    public void AddQuantity(int addValue) 
    { 
        quantity += addValue;
        UpdateUI();
    }

    private void UpdateUI()
    {
        _inventoryUI.Quantity.text = quantity.ToString();
    }
}
