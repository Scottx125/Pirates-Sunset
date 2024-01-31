using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerInventory : Inventory
{
    protected override InventoryObject CreateInventoryObject(string itemId)
    {
        // Create the UI aspects and then give the IO the refernces to the UI objects.
        InventoryObject instanceInventoryObject = Instantiate(_inventoryObjectPrefab, _inventoryObjectStorage).GetComponent<InventoryObject>();
        if (_inventoryUIBuilder != null)
        {
            GameObject inventoryUIInstance = _inventoryUIBuilder.CreateInventoryUIPrefab(_inventoryObjectsSOsDict[itemId].GetName, _inventoryObjectsSOsDict[itemId].GetImage);
            GameObject abilityHUDInstance = _inventoryUIBuilder.CreateAbilityHUDPrefab(_inventoryObjectsSOsDict[itemId].GetName, _inventoryObjectsSOsDict[itemId].GetImage);
            // Setup the object.
            instanceInventoryObject.Setup(0, _inventoryObjectsSOsDict[itemId].GetId,
                inventoryUIInstance.GetComponent<InventoryUI>());
        }
        return instanceInventoryObject;
    }
}
