using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class KeybindsMenu : MonoBehaviour, ISaveSettings, IChangeKey
{
    [SerializeField]
    private KeybindMenuEnums _desiredKeyForThisObj;
    [SerializeField]
    private InputSO _inputSO;
    [SerializeField]
    private TextMeshProUGUI _keybindText;
    [SerializeField]
    private TextMeshProUGUI _actionText;
    [SerializeField]
    private GameObject _blacklistedGameObjectText;
    [SerializeField]
    private float _blacklistTextMessageApperanceTime = 2.5f;

    private KeyCode _tempKeyCode;
    private KeyCode _keyCode;
    private List <IChangeKey> _keybindsMenus = new List<IChangeKey>();
    private IGetData _systemSettingsGetData;
    private ISetData _systemSettingsSetData;

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        _systemSettingsGetData = SettingsSystem.Instance;
        _systemSettingsSetData = SettingsSystem.Instance;
        _keyCode = _systemSettingsGetData.GetKeyCodeData(_desiredKeyForThisObj);
        foreach(KeybindsMenu kbm in transform.parent.GetComponentsInChildren<KeybindsMenu>())
        {
            if (kbm != this)
            {
                _keybindsMenus.Add(kbm);
            }
        }
        _actionText.text = _desiredKeyForThisObj.ToString();
        _tempKeyCode = _keyCode;
        RenameUIText(_keyCode);
    }

    private void OnEnable()
    {
        RenameUIText(_keyCode);
    }

    public void InitiateChangeKey()
    {
        StartCoroutine(ChangeKey());
    }

    public void SaveSettings()
    {
        if (_tempKeyCode != KeyCode.None)
        {
            _keyCode = _tempKeyCode;
        }
        _systemSettingsSetData.SetKeyCodeData(_desiredKeyForThisObj, _keyCode);
    }

    public void RenameUIText(KeyCode kC)
    {
        _keybindText.text = kC.ToString();
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

            // Update the text.
            RenameUIText(_tempKeyCode);

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
        foreach (IChangeKey kbm in _keybindsMenus)
        {
            if (_tempKeyCode == kbm.GetTempKeyCode())
            {
                // Change original to be the current keycode of this object.
                kbm.SetTempKeyCode(_keyCode);
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

    public KeyCode GetTempKeyCode()
    {
        return _tempKeyCode;
    }

    public void ResetTempKeyCode()
    {
        _tempKeyCode = _keyCode;
        RenameUIText(_keyCode);
    }

    public void SetTempKeyCode(KeyCode value)
    {
        _tempKeyCode = value;
        RenameUIText(value);
    }
}
