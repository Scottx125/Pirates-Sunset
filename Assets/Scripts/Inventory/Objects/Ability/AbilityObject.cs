using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbilityObject : MonoBehaviour
{
    // Track if the ability is active.    
    protected bool bIsActive = false;

    // Ability Data
    private bool _bAbilityObjectIsActivateable;
    private bool _bAbilityObjectRepeatsBehaviour;
    private float _abilityObjectActiveTime;
    private float _abilityObjectCooldownTime;
    private float _abilityObjectRepeatTime;

    // UI stuff
    private GameObject _abilityHUDObj;
    private PlayerAbilityHUD _pAbilityHUD;
    private PlayerInventoryUI _pInventoryUI;

    // Other references.
    private InventoryObject _inventoryObject;
    private AbilityTracker _abilityTracker;
    private AbilityType _abilityType;
    // Pass in ability tracker.
    // Also set ability Type variable.
    public void Setup(bool isActivateable, bool repeatsBehaviour, float activeTime, float cooldownTime, float repeatTime, 
        PlayerInventoryUI pInventoryUI, InventoryObject inventoryObject, AbilityType abilityType, GameObject abilityHUDObj = null, AbilityTracker abilityTracker = null)
    {
        _abilityHUDObj = abilityHUDObj;
        // Setup UI Stuff.
        if (_abilityHUDObj != null)
        {
            _pAbilityHUD = _abilityHUDObj.GetComponent<PlayerAbilityHUD>();
            // Ability is not active so disable hud obj.
            _abilityHUDObj.SetActive(false);
        }
        _pInventoryUI = pInventoryUI;


        // Setup linking of button to ability.
        Button setupButton = _pInventoryUI.gameObject.GetComponent<Button>();
        setupButton.onClick.AddListener(delegate { CheckAndActivateBehaviour(); });


        // Setup references.
        _inventoryObject = inventoryObject;
        _abilityTracker = abilityTracker;

        // Setup Data
        _bAbilityObjectIsActivateable = isActivateable;
        _bAbilityObjectRepeatsBehaviour = repeatsBehaviour;
        _abilityObjectActiveTime = activeTime;
        _abilityObjectCooldownTime = cooldownTime;
        _abilityObjectRepeatTime = repeatTime;
        _abilityType = abilityType;


    }
    public void CheckAndActivateBehaviour()
    {
        // Ensure object is activateable and is not currently active.
        if (_bAbilityObjectIsActivateable == false) return;
        if (bIsActive == true) return;
        if (_inventoryObject.GetInventoryObjectQuantity <= 0) return;
        // Begin coroutine.
        StartCoroutine(InitiateBehaviour());
    }
    private IEnumerator InitiateBehaviour()
    {
        bIsActive = true;
        ConsumedQuantity(1);
        // Stops a new coroutine being fired and sets up variables.
        // Handles repeat behaviours.
        if (_bAbilityObjectRepeatsBehaviour == true)
        {
            StartCoroutine(RepeatBehaviours());
        }
        else
        {
            // For duration behaviours only.
            if (_bAbilityObjectRepeatsBehaviour == false && _abilityObjectActiveTime > 0f)
            {
                StartCoroutine(DurationBehaviours());
            } 
            else
            {
                // Single-Fire behaviours.
                ObjectBehaviour();
                yield return StartCoroutine(Cooldown(0, _abilityObjectCooldownTime));
                bIsActive = false;
            }
        }
        yield return null;
    }

    private IEnumerator RepeatBehaviours()
    {
        StartCoroutine(Cooldown(_abilityObjectActiveTime, _abilityObjectCooldownTime));
        float duration = Time.time + _abilityObjectActiveTime;
        CheckAndEnableAbilityTrackerHUD();
        // Repeat behaviours.
        while (Time.time < duration)
        {
            ObjectBehaviour();
            yield return new WaitForSeconds(_abilityObjectRepeatTime);
        }
        CheckAndEnableAbilityTrackerHUD();
        yield return new WaitForSeconds(_abilityObjectCooldownTime);
        bIsActive = false;
    }

    private void CheckAndEnableAbilityTrackerHUD()
    {
        if (_abilityHUDObj != null && _abilityHUDObj.activeInHierarchy == false)
        {
            _abilityHUDObj.SetActive(true);
            if (_abilityObjectActiveTime > 0f) StartCoroutine(_pAbilityHUD.UILerpFill(_abilityObjectActiveTime));
        } else if (_abilityHUDObj != null && _abilityHUDObj.activeInHierarchy == true)
        {
            _abilityHUDObj.SetActive(false);
        }
    }

    private IEnumerator DurationBehaviours()
    {
        // Objects that are duration behaviours will be a toggle like system. Activating and then deactivating when called twice.
        StartCoroutine(Cooldown(_abilityObjectActiveTime, _abilityObjectCooldownTime));
        // Toggle on
        CheckAndEnableAbilityTrackerHUD();
        ObjectBehaviour();
        // Wait
        yield return new WaitForSeconds(_abilityObjectActiveTime);
        // Toggle off
        ObjectBehaviour();
        CheckAndEnableAbilityTrackerHUD();
        yield return new WaitForSeconds(_abilityObjectCooldownTime);
        bIsActive = false;
    }
    private IEnumerator Cooldown(float activeTime = 0f, float abilityCooldownTime = 0f)
    {
        // Waits for the cooldown to expire and then exits, fully resetting the behaviour.
        float duration = _abilityObjectCooldownTime + activeTime;
        StartCoroutine(_pInventoryUI.UILerpFill(duration));
        yield return new WaitForSeconds(duration);
    }

    protected virtual void ObjectBehaviour()
    {
        // Implement in child classes.
    }

    protected void ConsumedQuantity(int i)
    {
        _inventoryObject.SetQuantity(_inventoryObject.GetInventoryObjectQuantity - i);
    }
}
