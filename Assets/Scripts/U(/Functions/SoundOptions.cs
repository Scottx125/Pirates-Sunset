using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundOptions : MonoBehaviour, IApplySettings
{
    // Hold the slider value temp.
    // On enable load the current value.
    // Load the SO on awake. When we apply set the SO setting to the temp setting. 
    [SerializeField]
    private SoundSO _soundData;
    [SerializeField]
    private SoundOptionEnums _desiredOptionDataEnum;
    [SerializeField]
    private Slider _slider;
    [SerializeField]
    private TextMeshProUGUI _soundOptionTitle;

    private SoundOptionObject _optionStruct;
    private float _soundLevel;
    private string _soundOptionName;
    

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        if (_soundData != null)
        {
            foreach (SoundOptionObject x in _soundData.GetSoundOptionsData)
            {
                if (x.GetSoundEnum == _desiredOptionDataEnum)
                {
                    _optionStruct = x;
                }
            }
        } else { Debug.LogError("SOUND DATA NOT FOUND!"); }

        _soundLevel = _optionStruct.GetSetSoundLevel;
        _soundOptionName = _optionStruct.GetName;

        if (_soundOptionTitle != null)
        {
            _soundOptionTitle.text = _soundOptionName;
        } else { Debug.LogError("SOUND OPTION TITLE NOT SETUP!"); }

        if (_slider != null)
        {
            _slider.value = _soundLevel;
        } else { Debug.LogError("SLIDER IN SOUND OPTIONS NOT SETUP!"); }
    }

    private void OnEnable()
    {
        _soundLevel = _optionStruct.GetSetSoundLevel;
        _slider.value = _soundLevel;
    }

    public void Apply()
    {
        _soundLevel = _slider.value;
        _optionStruct.GetSetSoundLevel = _soundLevel;
    }
}
