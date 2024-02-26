using UnityEngine;

public abstract class InventoryObject : MonoBehaviour
{
    public int InventoryObjectQuantity { get; protected set; }
    public string InventoryObjectId { get; protected set; }
    public abstract void SetQuantity(int addValue);

}
