using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class KeybindsMenu : MonoBehaviour, ISaveSettings
{
    public KeyCode KeyCode { get { return _keyCode; } set { _keyCode = value;} }

    [SerializeField]
    private KeybindMenuEnums _desiredKeyForThisObj;
    [SerializeField]
    private InputSO _inputSO;
    [SerializeField]
    private TextMeshProUGUI _keybindText;
    [SerializeField]
    private GameObject _blacklistedGameObjectText;
    [SerializeField]
    private float _blacklistTextMessageApperanceTime = 2.5f;

    private KeyCode _tempKeyCode;
    private KeyCode _keyCode;
    private KeybindsMenu[] _keybindsMenus;
    private SettingsSystem _settingsSystem;

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        _settingsSystem = SettingsSystem.Instance;
        _settingsSystem.keyCodeDict.TryGetValue(_desiredKeyForThisObj, out _keyCode);
        _keybindsMenus = transform.parent.GetComponentsInChildren<KeybindsMenu>();
        RenameUIText();
    }

    private void OnEnable()
    {
        RenameUIText();
    }

    public void InitiateChangeKey()
    {
        StartCoroutine(ChangeKey());
    }

    public void UpdateKeybindsUI()
    {
        RenameUIText();
    }

    public void SaveSettings()
    {
        _settingsSystem.keyCodeDict[_desiredKeyForThisObj] = _keyCode;
    }

    private void RenameUIText()
    {
        _keybindText.text = _keyCode.ToString();
    }

    private IEnumerator ChangeKey()
    {
        while (true)
        {
            yield return new WaitUntil(() => Input.anyKeyDown);
            
            _tempKeyCode = DetectKeyPressed();

            // check if the keycode is blacklisted.
            if (CheckForBlacklist() == true) break;

            // Check if the key is already set to something else.
            CheckForSharedKeyCode();

            // Set obj keycode to the temp keycode.
            _keyCode = _tempKeyCode;

            // Update the text.
            UpdateKeybindsUI();

            yield break;
        }
        yield return null;
    }

    private KeyCode DetectKeyPressed()
    {
        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keyCode))
            {
                return keyCode;
            }
        }
        return KeyCode.None;
    }

    private IEnumerator BlackListedKey()
    {
        _blacklistedGameObjectText.SetActive(true);

        yield return new WaitForSeconds(_blacklistTextMessageApperanceTime);

        _blacklistedGameObjectText.SetActive(false);
    }

    private void CheckForSharedKeyCode()
    {
        foreach (KeybindsMenu kbm in _keybindsMenus)
        {
            if (_tempKeyCode == kbm.KeyCode && kbm != this)
            {
                // Change original to be the current keycode of this object.
                kbm.KeyCode = _keyCode;
                kbm.UpdateKeybindsUI();
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
