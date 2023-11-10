using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundOptions : MonoBehaviour, IApplySettings
{
    public static event Action OnSoundOptionsApplyEvent;

    [SerializeField]
    private SoundOptionEnums _soundOptionEnum;
    [SerializeField]
    private Slider _slider;
    [SerializeField]
    private TextMeshProUGUI _soundOptionTitle;

    private float _soundLevel;
    private SettingsSystem _system;
    

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        _system = SettingsSystem.Instance;
        if (_slider != null)
        {
            _system.soundDict.TryGetValue(_soundOptionEnum, out _soundLevel);
            _slider.value = _soundLevel;
        } else { Debug.LogError("SLIDER IN SOUND OPTIONS NOT SETUP!"); }
        
    }

    public void Apply()
    {
        _soundLevel = _slider.value;
        if (_system.soundDict.ContainsKey(_soundOptionEnum))
        {
            _system.soundDict[_soundOptionEnum] = _soundLevel;
        }
        OnSoundOptionsApplyEvent();
    }
}
