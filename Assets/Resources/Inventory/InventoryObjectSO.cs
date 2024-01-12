using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Inventory", menuName = "ScriptableObjects/Inventory", order = 2)]
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
    private string _invenObjName;
    [SerializeField]
    private Sprite _invenObjSprite;
    [SerializeField]
    private int _sellPrice;
    [SerializeField]
    private int _buyPrice;
    [SerializeField]
    private GameObject _prefab;
    [SerializeField]
    private InventoryObject _inventoryObjectType;


    public int GetSellPrice => _sellPrice;
    public int GetBuyPrice => _buyPrice;
    public GameObject GetPrefab => _prefab;
    public Type GetInventoryObjectType => _inventoryObjectType.GetType();
    public bool GetIsActivateableBool => _isActivateable;
    public bool GetRepeatBehaviourBool => _repeatBehaviour;
    public float GetActiveTimeFloat => _activeTime;
    public float GetCooldownFloat => _cooldown;
    public float GetRepeatTimeFloat => _repeatTime;
    public string GetName => _invenObjName;
    public Sprite GetImage => _invenObjSprite;
}
