using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityObject : MonoBehaviour
{
    // Track if the ability is active.    
    private bool bIsActive = false;

    // Ability Data
    private bool _bAbilityObjectIsActivateable;
    private bool _bAbilityObjectRepeatsBehaviour;
    private float _abilityObjectActiveTime;
    private float _abilityObjectCooldownTime;
    private float _abilityObjectRepeatTime;

    // UI stuff
    private GameObject _abilityHUDAsset;
    private InventoryUI _abilityHUD;
    private InventoryUI _inventoryUI;

    // Other references.
    private InventoryObject _inventoryObject;

    public void Setup(bool isActivateable, bool repeatsBehaviour, float activeTime, float cooldownTime, float repeatTime, 
        GameObject abilityUIAsset, InventoryUI inventoryUI, InventoryObject inventoryObject)
    {
        // Setup UI Stuff.
        _abilityHUDAsset = abilityUIAsset;
        _abilityHUD = _abilityHUDAsset.GetComponent<InventoryUI>();
        _inventoryUI = inventoryUI;

        // Setup references.
        _inventoryObject = inventoryObject;

        // Setup Data
        _bAbilityObjectIsActivateable = isActivateable;
        _bAbilityObjectRepeatsBehaviour = repeatsBehaviour;
        _abilityObjectActiveTime = activeTime;
        _abilityObjectCooldownTime = cooldownTime;
        _abilityObjectRepeatTime = repeatTime;

        // Ability is not active so disable.
        _abilityHUDAsset.SetActive(false);
    }
    public void CheckAndActivateBehaviour()
    {
        // Ensure object is activateable and is not currently active.
        if (_bAbilityObjectIsActivateable == false) return;
        if (bIsActive == true) return;
        if (_inventoryObject.InventoryObjectQuantity <= 0) return;
        // Begin coroutine.
        StartCoroutine(InitiateBehaviour());
    }
    private IEnumerator InitiateBehaviour()
    {
        // Stops a new coroutine being fired and sets up variables.
        bIsActive = true;
        // Handles repeat behaviours.
        if (_bAbilityObjectRepeatsBehaviour == true)
        {
            yield return StartCoroutine(RepeatBehaviours());
        }
        else
        {
            // For duration behaviours only.
            if (_bAbilityObjectRepeatsBehaviour == false && _abilityObjectActiveTime > 0f)
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
        float duration = Time.time + _abilityObjectActiveTime;
        _abilityHUDAsset.SetActive(true);
        StartCoroutine(_abilityHUD.UILerpFill(duration, _abilityObjectActiveTime));
        // Repeat behaviours.
        while (Time.time < duration)
        {
            ObjectBehaviour();
            yield return new WaitForSeconds(_abilityObjectRepeatTime);
        }
        _abilityHUDAsset.SetActive(false);
    }
    private IEnumerator DurationBehaviours()
    {
        // Objects that are duration behaviours will be a toggle like system. Activating and then deactivating when called twice.
        float duration = Time.time + _abilityObjectActiveTime;
        // Toggle on
        _abilityHUDAsset.SetActive(true);
        ObjectBehaviour();
        StartCoroutine(_abilityHUD.UILerpFill(duration, _abilityObjectActiveTime));
        // Wait
        yield return new WaitForSeconds(_abilityObjectActiveTime);
        // Toggle off
        ObjectBehaviour();
        _abilityHUDAsset.SetActive(false);
    }
    private IEnumerator Cooldown()
    {
        // Waits for the cooldown to expire and then exits, fully resetting the behaviour.
        float duration = Time.time + _abilityObjectCooldownTime;
        StartCoroutine(_inventoryUI.UILerpFill(duration, _abilityObjectActiveTime));
        yield return new WaitForSeconds(_abilityObjectCooldownTime);
    }

    protected virtual void ObjectBehaviour()
    {
        // Implement in child classes.
    }

    protected void ConsumedQuantity(int i)
    {
        _inventoryObject.SubtractQuantity(i);
    }
}
