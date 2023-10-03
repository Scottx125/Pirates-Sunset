using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybindsMenu : MonoBehaviour
{
    [SerializeField]
    private InputSO _inputSO;
    [SerializeField]
    public KeybindMenuEnums _keybindEnum;
    [SerializeField]
    private TMPro.TextMeshPro _text;

    private void OnEnable()
    {
        // Match enum to object in _inputSO list.
    }

    private void OnDisable()
    {
        // Update keybindings for everything by telling them to update? idk if this is needed?
    }
}
