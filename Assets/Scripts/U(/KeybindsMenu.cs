using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class KeybindsMenu : MonoBehaviour, IApplySettings
{
    public static event Action OnKeybindOptionsApplyEvent;

    [SerializeField]
    private InputSO _inputSO;
    [SerializeField]
    public KeybindMenuEnums _keybindEnum;
    [SerializeField]
    private TextMeshProUGUI _keybindText;
    [SerializeField]
    private TextMeshProUGUI _enumText;
    [SerializeField]
    private GameObject _blacklistedGameObjectText;
    [SerializeField]
    private float _blacklistTextMessageApperanceTime = 2.5f;

    private KeyCodeObject _keyCodeObject;
    private KeyCode _tempKeyCode;
    private KeybindsMenu[] _keybindsMenus;

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        _keybindsMenus = GetComponents<KeybindsMenu>();
        // Match enum to object in _inputSO list and cache object.
        // Load object data such as keycode name, enum name etc.
        foreach (KeyCodeObject obj in _inputSO.GetInputs)
        {
            if (obj.GetKeybindEnum == _keybindEnum)
            {
                _keyCodeObject = obj;
            }
        }
        if (_keyCodeObject == null)
        {
            Debug.LogError("{0} keycodeObject is null!", this.gameObject);
            return;
        }

        RenameUIText();
    }

    private void OnEnable()
    {
        RenameUIText();
    }

    public void ChangeKeyCode()
    {
        StartCoroutine(WaitForKeyPress());
        // check if the keycode is blacklisted.
        if (CheckForBlacklist() == true) return;

        // Check if the key is already set to something else.
        CheckForSharedKeyCode();

        // Set obj keycode to the temp keycode.
        _keyCodeObject.GetSetKeyCode = _tempKeyCode;

        // Update the text on all the KBM to reflect changes.
        foreach(KeybindsMenu kbm in _keybindsMenus)
        {
            kbm.UpdateKeybindsUI();
        }
    }

    public void UpdateKeybindsUI()
    {
        RenameUIText();
    }

    public void Apply()
    {
        OnKeybindOptionsApplyEvent();
    }

    private void RenameUIText()
    {
        _keybindText.text = _keyCodeObject.GetSetKeyCode.ToString();
        _enumText.text = _keyCodeObject.GetKeybindEnum.ToString();
    }

    private IEnumerator WaitForKeyPress()
    {
        Event e = Event.current;
        while (true)
        {
            if (e.isKey)
            {
                _tempKeyCode = e.keyCode;
                break;
            }
        }
        yield return null;
    }

    private IEnumerator BlackListedKey()
    {
        _blacklistedGameObjectText.SetActive(true);

        yield return new WaitForSeconds(_blacklistTextMessageApperanceTime);

        _blacklistedGameObjectText.SetActive(false);
    }

    private void CheckForSharedKeyCode()
    {
        foreach (KeyCodeObject obj in _inputSO.GetInputs)
        {
            if (_tempKeyCode == obj.GetSetKeyCode && obj != _keyCodeObject)
            {
                // Change original to be the current keycode of this object.
                obj.GetSetKeyCode = _keyCodeObject.GetSetKeyCode;
            }
        }
    }

    private bool CheckForBlacklist()
    {
        foreach (KeyCode kc in _inputSO.GetInputBlacklist)
        {
            if (_tempKeyCode == kc)
            {
                StartCoroutine(BlackListedKey());
                return true;
            }
        }
        return false;
    }
}
