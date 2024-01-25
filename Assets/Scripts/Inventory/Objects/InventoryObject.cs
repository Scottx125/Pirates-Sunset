using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
public abstract class InventoryObject : MonoBehaviour
{
    [SerializeField]
    private InventoryObjectSO _inventoryObjectData;
    [SerializeField]
    protected int quantity = 0;
    // UI stuff
    private GameObject _inventoryUIAsset;
    private GameObject _abilityHUDAsset;
    private InventoryUI _inventoryUI;
    private InventoryUI _abilityHUD;


    protected bool bIsActive = false;
    // PAss in abilityTracker and interface for UI setup and updating.
    public void SetupForPlayerUI(GameObject inventoryUIAsset, GameObject abilityUIAsset)
    {
        // CacheUI
        _inventoryUIAsset = inventoryUIAsset;
        _abilityHUDAsset = abilityUIAsset;
        _inventoryUI = inventoryUIAsset.GetComponent<InventoryUI>();
        _abilityHUD = abilityUIAsset.GetComponent<InventoryUI>();

        // Disable ability object as it's not being used.
        _abilityHUDAsset.SetActive(false);
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
        // Handles repeat behaviours.
        if (_inventoryObjectData.GetRepeatBehaviourBool == true)
        {
            yield return StartCoroutine(RepeatBehaviours());
        } else
        {
            // For duration behaviours only.
            if (_inventoryObjectData.GetRepeatBehaviourBool == false && _inventoryObjectData.GetActiveTimeFloat > 0f)
            {
                yield return StartCoroutine(DurationBehaviours());
            }
            // Single-Fire behaviours.
            ObjectBehaviour();
        }
        
        yield return StartCoroutine(Cooldown());

        bIsActive = false;
    }

    private IEnumerator RepeatBehaviours()
    {
        float duration = Time.time + _inventoryObjectData.GetActiveTimeFloat;
        _abilityHUDAsset.SetActive(true);
        StartCoroutine(_abilityHUD.UILerpFill(duration, _inventoryObjectData.GetActiveTimeFloat));
        // Repeat behaviours.
        while (Time.time < duration)
        {
            ObjectBehaviour();
            yield return new WaitForSeconds(_inventoryObjectData.GetRepeatTimeFloat);
        }
        _abilityHUDAsset.SetActive(false);
    }
    private IEnumerator DurationBehaviours()
    {
        // Objects that are duration behaviours will be a toggle like system. Activating and then deactivating when called twice.
        float duration = Time.time + _inventoryObjectData.GetActiveTimeFloat;
        // Toggle on
        _abilityHUDAsset.SetActive(true);
        ObjectBehaviour();
        StartCoroutine(_abilityHUD.UILerpFill(duration, _inventoryObjectData.GetActiveTimeFloat));
        // Wait
        yield return new WaitForSeconds(_inventoryObjectData.GetActiveTimeFloat);
        // Toggle off
        ObjectBehaviour();
        _abilityHUDAsset.SetActive(false);
    }
    private IEnumerator Cooldown()
    {
        // Waits for the cooldown to expire and then exits, fully resetting the behaviour.
        float duration = Time.time + _inventoryObjectData.GetCooldownFloat;
        StartCoroutine(_inventoryUI.UILerpFill(duration, _inventoryObjectData.GetActiveTimeFloat));
        yield return new WaitForSeconds(_inventoryObjectData.GetCooldownFloat);
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
        _inventoryUI.UpdateUIQuantity(quantity);
    }
    public void AddQuantity(int addValue) 
    { 
        quantity += addValue;
        _inventoryUI.UpdateUIQuantity(quantity);
    }

    private void OnDestroy()
    {
        Destroy(_inventoryUIAsset.gameObject);
        Destroy(_abilityHUDAsset.gameObject);
    }
}
