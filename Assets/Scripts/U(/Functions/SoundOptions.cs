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
    private Slider _slider;
    [SerializeField]
    private TextMeshProUGUI _soundOptionTitle;

    private float _soundLevel;
    

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        if (_slider != null)
        {
            _slider.value = _soundLevel;
        } else { Debug.LogError("SLIDER IN SOUND OPTIONS NOT SETUP!"); }
    }

    public void Apply()
    {
        _soundLevel = _slider.value;
        OnSoundOptionsApplyEvent();
    }
}
