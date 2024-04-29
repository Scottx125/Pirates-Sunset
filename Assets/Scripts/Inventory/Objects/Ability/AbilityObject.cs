using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        // Setup UI Stuff.
        if (_abilityHUDObj != null)
        {
            _abilityHUDObj = abilityHUDObj;
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
        CheckIfAbilityHudIsNull(true ,duration);
        // Repeat behaviours.
        while (Time.time < duration)
        {
            ObjectBehaviour();
            yield return new WaitForSeconds(_abilityObjectRepeatTime);
        }
        CheckIfAbilityHudIsNull(false);
    }

    private void CheckIfAbilityHudIsNull(bool bActivityState, float duration = 0f)
    {
        if (_abilityHUDObj != null)
        {
            _abilityHUDObj.SetActive(bActivityState);
            if (duration > 0f) StartCoroutine(_pAbilityHUD.UILerpFill(duration, _abilityObjectActiveTime));
        }
    }

    private IEnumerator DurationBehaviours()
    {
        // Objects that are duration behaviours will be a toggle like system. Activating and then deactivating when called twice.
        float duration = Time.time + _abilityObjectActiveTime;
        // Toggle on
        CheckIfAbilityHudIsNull(true, duration);
        ObjectBehaviour();
        // Wait
        yield return new WaitForSeconds(_abilityObjectActiveTime);
        // Toggle off
        ObjectBehaviour();
        CheckIfAbilityHudIsNull(false);
    }
    private IEnumerator Cooldown()
    {
        // Waits for the cooldown to expire and then exits, fully resetting the behaviour.
        float duration = Time.time + _abilityObjectCooldownTime;
        StartCoroutine(_pInventoryUI.UILerpFill(duration, _abilityObjectActiveTime));
        yield return new WaitForSeconds(_abilityObjectCooldownTime);
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
