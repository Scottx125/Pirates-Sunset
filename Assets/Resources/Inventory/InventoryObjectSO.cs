using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Inventory", menuName = "ScriptableObjects/InventoryObject", order = 2)]
public class InventoryObjectSO : ScriptableObject
{
    [SerializeField]
    private bool _isActivateable = false;
    [SerializeField]
    private bool _repeatBehaviour = false;
    [SerializeField]
    private float _activeTime = 1f;
    [SerializeField]
    private float _cooldown = 1f;
    [SerializeField]
    private float _repeatTime = 0.25f;
    [SerializeField]
    private string _objectId;
    [SerializeField]
    private string _invenObjName;
    [SerializeField]
    private Sprite _invenObjSprite;
    [SerializeField]
    private int _sellPrice;
    [SerializeField]
    private int _buyPrice;
    [SerializeField]
    private InventoryObject _inventoryObject;
    [SerializeField]
    private AbilityObject _abilityObject;

    public string GetId => _objectId;
    public InventoryObject GetInventoryObject => _inventoryObject;
    public AbilityObject GetAbilityObject => _abilityObject;
    public int GetSellPrice => _sellPrice;
    public int GetBuyPrice => _buyPrice;
    public bool GetIsActivateableBool => _isActivateable;
    public bool GetRepeatBehaviourBool => _repeatBehaviour;
    public float GetActiveTimeFloat => _activeTime;
    public float GetCooldownFloat => _cooldown;
    public float GetRepeatTimeFloat => _repeatTime;
    public string GetName => _invenObjName;
    public Sprite GetImage => _invenObjSprite;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(_objectId))
        {
            _objectId = Guid.NewGuid().ToString();
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
            Debug.Log("Generated UUID for ScriptableObject: " + _objectId);
        }
    }
}
