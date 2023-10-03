using System;
using UnityEngine;
[Serializable]
public struct KeyCodeStruct
{
    public KeybindMenuEnums GetKeybindEnum => _keybindEnum;
    public KeyCode GetSetKeyCode { get { return _keycode; } set { _keycode = value; } }

    [SerializeField]
    private KeybindMenuEnums _keybindEnum;
    [SerializeField]
    private KeyCode _keycode;

}
