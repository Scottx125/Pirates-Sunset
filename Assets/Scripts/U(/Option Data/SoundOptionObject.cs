using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SoundOptionObject
{
    public SoundOptionEnums GetSoundEnum => _soundOptionEnum;
    public string GetName => _soundOptionName;
    public float GetSetSoundLevel { get { return _soundOptionLevel; } set { _soundOptionLevel = Mathf.Clamp01(value); } }

    [SerializeField]
    private SoundOptionEnums _soundOptionEnum;
    [SerializeField]
    private string _soundOptionName;
    [SerializeField]
    [Range(0, 1)]
    private float _soundOptionLevel;
}
