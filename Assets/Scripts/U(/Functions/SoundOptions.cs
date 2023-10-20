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
    private SoundSO _soundData;
    [SerializeField]
    private SoundOptionEnums _desiredOptionDataEnum;
    [SerializeField]
    private Slider _slider;
    [SerializeField]
    private TextMeshProUGUI _soundOptionTitle;

    private SoundOptionObject _optionObject;
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
                    _optionObject = x;
                }
            }
        } else { Debug.LogError("SOUND DATA NOT FOUND!"); }

        _soundLevel = _optionObject.GetSetSoundLevel;
        _soundOptionName = _optionObject.GetName;

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
        _soundLevel = _optionObject.GetSetSoundLevel;
        _slider.value = _soundLevel;
    }

    public void Apply()
    {
        _soundLevel = _slider.value;
        _optionObject.GetSetSoundLevel = _soundLevel;
        OnSoundOptionsApplyEvent();
    }
}
