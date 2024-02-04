using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
public abstract class InventoryObject : MonoBehaviour
{
    public int InventoryObjectQuantity { get; protected set; }
    public string InventoryObjectId { get; protected set; }

    public abstract void SubtractQuantity(int subtractValue);
    public abstract void AddQuantity(int addValue);
}
