using UnityEngine;

public abstract class InventoryObject : MonoBehaviour
{
    public string InventoryObjectId { get; protected set; }
    public abstract void SetQuantity(int addValue);
    public int GetInventoryObjectQuantity => _inventoryObjectQuantity;

    protected int _inventoryObjectQuantity;
}
