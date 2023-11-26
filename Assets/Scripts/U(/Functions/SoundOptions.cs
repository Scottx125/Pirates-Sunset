using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundOptions : MonoBehaviour, ISaveSettings
{
    public static event Action OnSoundOptionsApplyEvent;

    [SerializeField]
    private SoundOptionEnums _desiredKeyForThisObj;
    [SerializeField]
    private Slider _slider;
    [SerializeField]
    private TextMeshProUGUI _soundOptionTitle;

    private float _soundLevel;
    private IGetData _systemSettingsGetData;
    private ISetData _systemSettingsSetData;


    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        SettingsSystem settingsSystem = FindFirstObjectByType<SettingsSystem>();
        if (settingsSystem == null)
        {
            Debug.LogError("Cannot find SettingsSystem!");
            return;
        }
        _systemSettingsGetData = settingsSystem;
        _systemSettingsSetData = settingsSystem;
        if (_slider != null)
        {
            _soundLevel = _systemSettingsGetData.GetData<SoundOptionEnums, float>(_desiredKeyForThisObj);
            _slider.value = _soundLevel;
        } else { Debug.LogError("SLIDER IN SOUND OPTIONS NOT SETUP!"); }
        
    }

    public void SaveSettings()
    {
        _soundLevel = _slider.value;

        _systemSettingsSetData.SetData<SoundOptionEnums, float>(_desiredKeyForThisObj, _soundLevel);

        OnSoundOptionsApplyEvent();
    }
}
