using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryObject : MonoBehaviour
{

    public int Quantity { get { return quantity; }}
    [SerializeField]
    protected bool bIsActivateable = false;
    [SerializeField]
    protected bool repeatBehaviour = false;
    [SerializeField]
    protected float activeTime = 1f;
    [SerializeField]
    protected float repeatTime = 0.25f;


    protected bool bIsActive = false;
    protected int quantity = 0;

    public abstract void InventoryObjectBehaviour();
}
