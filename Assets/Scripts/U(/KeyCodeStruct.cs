using System;
using UnityEngine;
[Serializable]
public struct KeyCodeStruct
{
    public KeybindMenuEnums GetKeybindEnum => _keybindEnum;
    public string GetName => _keybindName;
    public KeyCode GetSetKeyCode { get { return _keycode; } set { _keycode = value; } }

    [SerializeField]
    private KeybindMenuEnums _keybindEnum;
    [SerializeField]
    private string _keybindName;
    [SerializeField]
    private KeyCode _keycode;
}
