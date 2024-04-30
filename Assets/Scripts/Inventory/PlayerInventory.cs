using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Inventory
{
    [SerializeField]
    protected Transform _inventoryObjectStorage;
    [SerializeField]
    private Transform _inventoryUIContents;
    [SerializeField]
    private Transform _abilityActiveContents;
    [SerializeField]
    private GameObject _inventoryUIPrefab;
    [SerializeField]
    private GameObject _abilityHUDPrefab;
    [SerializeField]
    private AbilityTracker _abilityTracker;

    private void Start()
    {
        if (_inventoryObjectStorage == null) Debug.LogError("No inventoryStorage set!");
        _abilityTracker = GetComponent<AbilityTracker>();
        //if (_abilityTracker == null) Debug.LogError("Missing AbilityTracker in Inventory!");
    }

    protected override InventoryObject CreateInventoryObject(string itemId)
    {
        // Cache data.
        InventoryObjectSO data = _inventoryObjectsSOsDict[itemId];
        // SPAWN MAIN INVENTORY PREFAB AND GET RELEVANT SCRIPTS.
        GameObject inventoryObjectInstance = Instantiate(data.GetInventoryObjectPrefab, _inventoryObjectStorage);
        PlayerInventoryObject inventoryObject = inventoryObjectInstance.GetComponent<PlayerInventoryObject>();
        AbilityObject abilityObject = inventoryObjectInstance.GetComponent<AbilityObject>();
        
        // SPAWN UI PREFABS AND SETUP SCRIPTS.
        // Instantiate PlayerInventoryUIObj and setup.
        GameObject pInventoryUIObj = Instantiate(_inventoryUIPrefab, _inventoryUIContents);
        PlayerInventoryUI pInventoryUI = pInventoryUIObj.GetComponent<PlayerInventoryUI>();
        pInventoryUI.Setup(data.GetName, data.GetImage);

        GameObject pAbilityHUDObj = null;
        // Instantiate PlayerAbilityHUDObj and setup.
        if (_abilityActiveContents != null && _abilityHUDPrefab != null && data.GetIsActivateableBool)
        {
            pAbilityHUDObj = Instantiate(_abilityHUDPrefab, _abilityActiveContents);
            PlayerAbilityHUD pAbilityHUD = pAbilityHUDObj.GetComponent<PlayerAbilityHUD>();
            pAbilityHUD.Setup(data.GetImage);
        }


        // SETUP SCRIPTS OF INVOBJ AND ABILITYOBJ
        // Setup the inv object.
        inventoryObject.Setup(data.GetId, pInventoryUI);

        // Setup the ability object.
        abilityObject.Setup(data.GetIsActivateableBool, data.GetRepeatBehaviourBool,
            data.GetActiveTimeFloat, data.GetCooldownFloat, data.GetRepeatTimeFloat,
            pInventoryUI, inventoryObject, data.GetAbilityType, pAbilityHUDObj, _abilityTracker);
        
        return inventoryObject;
    }
}
