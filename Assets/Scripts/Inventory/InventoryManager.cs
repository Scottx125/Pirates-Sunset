using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // This will hold the inventory
    [SerializeField]
    private List<InventoryObject> _inventory = new List<InventoryObject>();
    public void Setup()
    {
        
    }
    // This will deal with interactions between the store and the inventory objects. Such as adding/removing. Finding items in the inventory etc.
}
