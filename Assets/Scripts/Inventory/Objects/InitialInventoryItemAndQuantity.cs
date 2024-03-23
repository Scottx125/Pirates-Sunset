using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct InitialInventoryItemAndQuantity
{
    public string GetID() => _object.GetId;
    public int GetQuantity() => _quantity;

    [SerializeField]
    private InventoryObjectSO _object;
    [SerializeField]
    private int _quantity;
}
