using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryObject : MonoBehaviour
{
    [SerializeField]
    protected bool bIsActivateable = false;
    [SerializeField]
    protected bool repeatBehaviour = false;
    [SerializeField]
    protected float activeTime = 1f;
    [SerializeField]
    protected float cooldown = 1f;
    [SerializeField]
    protected float repeatTime = 0.25f;
    [SerializeField]
    protected int quantity = 0;


    protected bool bIsActive = false;

    public void CheckBehaviour()
    {
        // Ensure object is activateable and is not currently active.
        if (bIsActivateable == false) return;
        if (bIsActive == true) return;
        // Begin coroutine.
        StartCoroutine(InitiateBehaviour());
    }
    private IEnumerator InitiateBehaviour()
    {
        // Stops a new coroutine being fired and sets up variables.
        bIsActive = true;
        float endTime = Time.time + activeTime;
        // Handles repeat behaviours.
        if (repeatBehaviour == true)
        {
            while (Time.time < endTime)
            {
                ObjectBehaviour();
                yield return new WaitForSeconds(repeatTime);
            }
        } else
        {
            // Single-Fire behaviours.
            ObjectBehaviour();
        }
        // Waits for the cooldown to expire and then exists, fully resetting the behaviour.
        yield return new WaitForSeconds(cooldown);
        bIsActive = false;
    }

    protected virtual void ObjectBehaviour()
    {
        // Implement in child classes.
    }

    public int GetQuantity() { return quantity; }
    public void SubtractQuantity(int subtractValue) { quantity -= subtractValue; }
    public void AddQuantity(int addValue) { quantity += addValue; }
}
