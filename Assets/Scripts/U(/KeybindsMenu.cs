using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        // Match enum to object in _inputSO list and cache object.
        // Load object data such as keycode name, enum name etc.
        foreach(KeyCodeObject keycodeStruct in _inputSO.GetInputs)
        {
            if (keycodeStruct.GetKeybindEnum == _keybindEnum)
            {

            }
        }
    }

    private void OnDisable()
    {
        // Update keybindings for everything by telling them to update? idk if this is needed?
    }
}
