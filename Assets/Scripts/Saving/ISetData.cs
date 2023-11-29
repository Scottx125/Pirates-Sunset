using System;
using UnityEngine;

public interface ISetData
{
    public void SetKeyCodeData(KeybindMenuEnums key, KeyCode value);
    public void SetSoundData(SoundOptionEnums key, float value);
}
