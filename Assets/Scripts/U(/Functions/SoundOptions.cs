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
    private SoundOptionEnums _soundOptionEnum;
    [SerializeField]
    private Slider _slider;
    [SerializeField]
    private TextMeshProUGUI _soundOptionTitle;

    private float _soundLevel;
    private SettingsSystem _settingsSystem;
    

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        _settingsSystem = SettingsSystem.Instance;
        if (_slider != null)
        {
            _settingsSystem.soundDict.TryGetValue(_soundOptionEnum, out _soundLevel);
            _slider.value = _soundLevel;
        } else { Debug.LogError("SLIDER IN SOUND OPTIONS NOT SETUP!"); }
        
    }

    public void SaveSettings()
    {
        _soundLevel = _slider.value;
        
        _settingsSystem.soundDict[_soundOptionEnum] = _soundLevel;

        OnSoundOptionsApplyEvent();
    }
}
