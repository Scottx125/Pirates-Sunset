using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct InitialInventoryItemAndQuantity
{
    public string GetID() => _id;
    public int GetQuantity() => _quantity;

    [SerializeField]
    private string _id;
    [SerializeField]
    private int _quantity;
}
