using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apply : MonoBehaviour
{
    [SerializeField]
    List<GameObject> _settingsObjects;

    private List<ISaveSettings> _settingOptions = new List<ISaveSettings>();
    private ISaveSettingsToFile _settingsSystem;
    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        _settingsSystem = FindFirstObjectByType<SettingsSystem>();
        foreach(GameObject gameObject in _settingsObjects)
        {
            _settingOptions.Add(gameObject.GetComponentInChildren<ISaveSettings>());
        }
    }

    public void ApplySettings()
    {
        foreach (ISaveSettings obj in _settingOptions)
        {
            obj.SaveSettings();
        }
        _settingsSystem.SaveSettingsToFile(false);
    }
}
