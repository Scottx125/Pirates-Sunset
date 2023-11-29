using System;
using UnityEngine;

public interface IGetData 
{
    public KeyCode GetKeyCodeData(KeybindMenuEnums key);
    public float GetSoundData(SoundOptionEnums key);
}
