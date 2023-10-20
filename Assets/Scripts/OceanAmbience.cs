using PirateGame.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanAmbience : MonoBehaviour
{
    [SerializeField]
    private SoundSO _soundSO;
    [SerializeField]
    private AudioSource _audioSource;

    private SoundOptionObject _soundOptionObject;
    private void Awake()
    {
        // Load sound setting and set it up.
        if (_soundSO != null)
        {
            _soundOptionObject = StaticHelpers.GetRequiredSoundObject(_soundSO.GetSoundOptionsData, SoundOptionEnums.Ambient);
        }

        if (_audioSource != null)
        {
            UpdateSoundSettings();
        }
    }
    private void OnEnable()
    {
        SoundOptions.OnSoundOptionsApplyEvent += UpdateSoundSettings;
    }

    private void OnDisable()
    {
        SoundOptions.OnSoundOptionsApplyEvent -= UpdateSoundSettings;
    }

    private void UpdateSoundSettings()
    {
        _audioSource.volume = _soundOptionObject.GetSetSoundLevel;
    }
}
