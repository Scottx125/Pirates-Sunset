using System;
using UnityEngine;
[Serializable]
public class KeyCodeObject
{
    public KeybindMenuEnums GetKeybindEnum => _keybindEnum;
    public KeyCode GetSetKeyCode { get { return _keycode; } set { _keycode = value;} }

    [SerializeField]
    private KeybindMenuEnums _keybindEnum;
    [SerializeField]
    private KeyCode _keycode;
}
