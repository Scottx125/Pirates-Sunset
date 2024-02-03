using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.PostProcessing.SubpixelMorphologicalAntialiasing;

public class InventoryUIBuilder : MonoBehaviour
{
    [SerializeField]
    private Transform _inventoryUIContents;
    [SerializeField]
    private Transform _abilityActiveContents;
    [SerializeField]
    private GameObject _inventoryUIPrefab;
    [SerializeField]
    private GameObject _abilityHUDPrefab;

    public void Setup()
    {
        if (_inventoryUIContents == null)
        {
            Debug.LogError("Need to assign the UI contents object!");
            return;
        }
        if (_abilityActiveContents == null)
        {
            Debug.LogError("Need to assign the HUD ability contents object!");
            return;
        }
        if (_inventoryUIPrefab == null)
        {
            Debug.LogError("Need to assign the inventoryUIPrefab!");
            return;
        }
        if (_abilityHUDPrefab == null)
        {
            Debug.LogError("Need to assign the abilityHUDPrefab!");
            return;
        }
    }

    public GameObject CreateInventoryUIPrefab()
    {
        GameObject obj = Instantiate(_inventoryUIPrefab, _inventoryUIContents);
        return obj;
    }
    public GameObject CreateAbilityHUDPrefab()
    {
        GameObject obj = Instantiate(_abilityHUDPrefab, _abilityActiveContents);
        return obj;
    }
}
