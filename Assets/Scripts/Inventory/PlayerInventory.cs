using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Inventory
{
    [SerializeField]
    private Transform _inventoryUIContents;
    [SerializeField]
    private Transform _abilityActiveContents;
    [SerializeField]
    private GameObject _inventoryUIPrefab;
    [SerializeField]
    private GameObject _abilityHUDPrefab;
    protected override InventoryObject CreateInventoryObject(string itemId)
    {
        // SPAWN MAIN INVENTORY PREFAB AND GET RELEVANT SCRIPTS.
        GameObject inventoryObjectInstance = Instantiate(_inventoryObjectPrefab, _inventoryObjectStorage);
        InventoryObject inventoryObject = inventoryObjectInstance.GetComponent<InventoryObject>();
        AbilityObject abilityObject = inventoryObjectInstance.GetComponent<AbilityObject>();
        
        // SPAWN UI PREFABS AND SETUP SCRIPTS.
        // Instantiate PlayerInventoryUIObj and setup.
        GameObject pInventoryUIObj = Instantiate(_inventoryUIPrefab, _inventoryUIContents);
        PlayerInventoryUI pInventoryUI = pInventoryUIObj.GetComponent<PlayerInventoryUI>();
        pInventoryUI.Setup(_inventoryObjectsSOsDict[itemId].GetName, _inventoryObjectsSOsDict[itemId].GetImage);
        // Instantiate PlayerAbilityHUDObj and setup.
        GameObject pAbilityHUDObj = Instantiate(_abilityHUDPrefab, _abilityActiveContents);
        PlayerAbilityHUD pAbilityHUD = pAbilityHUDObj.GetComponent<PlayerAbilityHUD>();
        pAbilityHUD.Setup(_inventoryObjectsSOsDict[itemId].GetImage);

        // SETUP SCRIPTS OF INVOBJ AND ABILITYOBJ
        // Setup the inv object.
        inventoryObject.Setup(_inventoryObjectsSOsDict[itemId].GetId, pInventoryUI);

        // Setup the ability object.
        abilityObject.Setup(_inventoryObjectsSOsDict[itemId].GetIsActivateableBool, _inventoryObjectsSOsDict[itemId].GetRepeatBehaviourBool,
            _inventoryObjectsSOsDict[itemId].GetActiveTimeFloat, _inventoryObjectsSOsDict[itemId].GetCooldownFloat, _inventoryObjectsSOsDict[itemId].GetRepeatTimeFloat,
            pAbilityHUDObj, pInventoryUI, inventoryObject);
        
        return inventoryObject;
    }
}
