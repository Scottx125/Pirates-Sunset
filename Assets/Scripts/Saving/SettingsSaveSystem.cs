using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SettingsSaveSystem : MonoBehaviour
{
    public static SettingsSaveSystem Instance { get; private set; }

    public float ambientVolume;
    public float combatVolume;
    public float musicVolume;

    public Dictionary<KeybindMenuEnums, KeyCode> keyCodeDict = new Dictionary<KeybindMenuEnums, KeyCode>
    {
        { KeybindMenuEnums.Accelerate, KeyCode.None },
        { KeybindMenuEnums.Decelerate, KeyCode.None },
        { KeybindMenuEnums.Left, KeyCode.None },
        { KeybindMenuEnums.Right, KeyCode.None },
        { KeybindMenuEnums.Options, KeyCode.None },
    };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        } else
        {
            Destroy(gameObject);
        }
    }


    public void SaveSettings()
    {
        // Save keybinds based on enum list from dict.
        string fileContent =
            "[Sound]\n" +
            "Ambient = " + ambientVolume + "\n" +
            "Combat = " + combatVolume + "\n" +
            "Music = " + musicVolume + "\n" +
            "[Keybinds]\n" +
            "Accelerate (" + KeybindMenuEnums.Accelerate + ") = " + (int)acceleration + "\n" +
            "Decelerate (" + KeybindMenuEnums.Decelerate + ") = " + (int)deceleration + "\n" +
            "Left (" + KeybindMenuEnums.Left + ") = " + (int)left + "\n" +
            "Right (" + KeybindMenuEnums.Right + ") = " + (int)right + "\n" +
            "Options (" + KeybindMenuEnums.Options + ") = " + (int)options;

        System.IO.File.WriteAllText("Settings.txt", fileContent);
    }
    
    public void LoadSettings()
    {

    }
}
