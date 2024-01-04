using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public abstract class InventoryObject : MonoBehaviour
{
    [SerializeField]
    protected bool isActivateable = false;
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
    [SerializeField]
    private TextMeshProUGUI _uiNameText;
    [SerializeField]
    private TextMeshProUGUI _uiQuantity;
    [SerializeField]
    private string _invenObjName;
    [SerializeField]
    private Image _invenObjimage;
    [SerializeField]
    private Button _uiButton;

    // USE SCRIPTABLE OBJECTS TO HOLD ABOVE DATA EXCLUDING THINGS THAT WILL DYNAMICALLY CHANGE LIKE QUANITTY.


    protected bool bIsActive = false;

    private void Awake()
    {
        // Break this out so that if it's added later, we can assign it through the inven manager.
        if (_uiButton != null) _uiButton.image = _invenObjimage;
        if (_uiNameText != null) _uiNameText.text = _invenObjName;
        if (_uiQuantity != null) _uiQuantity.text = quantity.ToString();
    }
    public void CheckBehaviour()
    {
        // Ensure object is activateable and is not currently active.
        if (isActivateable == false) return ;
        if (bIsActive == true) return ;
        if (quantity <= 0) return ;
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
    public void SubtractQuantity(int subtractValue) 
    { 
        quantity -= subtractValue;
        _uiQuantity.text = quantity.ToString();
    }
    public void AddQuantity(int addValue) 
    { 
        quantity += addValue;
        _uiQuantity.text = quantity.ToString();
    }
}
