using Palmmedia.ReportGenerator.Core;
using PirateGame.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanAmbience : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;

    private SettingsSystem _settings;

    private void Start()
    {
        Setup();
    }
    private void OnEnable()
    {
        SoundOptions.OnSoundOptionsApplyEvent += UpdateSoundSettings;
    }

    private void OnDisable()
    {
        SoundOptions.OnSoundOptionsApplyEvent -= UpdateSoundSettings;
    }

    private void Setup()
    {
        _settings = SettingsSystem.Instance;
        if (_audioSource != null)
        {
            _audioSource.volume = _settings.soundDict[SoundOptionEnums.Ambient];
        }
    }
    private void UpdateSoundSettings()
    {
        _audioSource.volume = _settings.soundDict[SoundOptionEnums.Ambient];
    }
}
