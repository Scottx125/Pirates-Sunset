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
        // Create the UI aspects and then give the IO the refernces to the UI objects.
        GameObject inventoryObjectGameObject = Instantiate(_inventoryObjectPrefab, _inventoryObjectStorage);
        InventoryObject instanceInventoryObject = inventoryObjectGameObject.GetComponent<InventoryObject>();
        AbilityObject instanceAbilityObject = inventoryObjectGameObject.GetComponent<AbilityObject>();
        
        // Instantiate HUD/UI objects.
        GameObject inventoryUIInstance = Instantiate(_inventoryUIPrefab, _inventoryUIContents);
        InventoryUI invUI = inventoryUIInstance.GetComponent<InventoryUI>();
        GameObject abilityHUDInstance = Instantiate(_abilityHUDPrefab, _abilityActiveContents);
        InventoryUI abilityHUD = abilityHUDInstance.GetComponent<InventoryUI>().Setup(_inventoryObjectsSOsDict[itemId].GetName,);
        
        // Setup the inv object.
        instanceInventoryObject.Setup(0, _inventoryObjectsSOsDict[itemId].GetId, inventoryUIInstance.GetComponent<InventoryUI>());
        
        // Setup the ability object.
        instanceAbilityObject.Setup(_inventoryObjectsSOsDict[itemId].GetIsActivateableBool, _inventoryObjectsSOsDict[itemId].GetRepeatBehaviourBool,
            _inventoryObjectsSOsDict[itemId].GetActiveTimeFloat, _inventoryObjectsSOsDict[itemId].GetCooldownFloat, _inventoryObjectsSOsDict[itemId].GetRepeatTimeFloat,
            abilityHUDInstance, inventoryUIInstance.GetComponent<InventoryUI>(), instanceInventoryObject);
        
        return instanceInventoryObject;
    }
}
