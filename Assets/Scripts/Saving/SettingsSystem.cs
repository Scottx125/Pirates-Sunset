using PirateGame.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SettingsSystem : MonoBehaviour
{
    public static SettingsSystem Instance { get; private set; }

    public Dictionary<SoundOptionEnums, float> soundDict = new Dictionary<SoundOptionEnums, float>
    {
        { SoundOptionEnums.Ambient, .5f },
        { SoundOptionEnums.Combat, .5f },
        { SoundOptionEnums.Music, .5f }
    };

    public Dictionary<KeybindMenuEnums, KeyCode> keyCodeDict = new Dictionary<KeybindMenuEnums, KeyCode>
    {
        { KeybindMenuEnums.Accelerate, KeyCode.W },
        { KeybindMenuEnums.Decelerate, KeyCode.S },
        { KeybindMenuEnums.Left, KeyCode.A },
        { KeybindMenuEnums.Right, KeyCode.D },
        { KeybindMenuEnums.Options, KeyCode.Escape },
    };

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadSettings();
    }

    public void SaveSettings()
    {
        // Save keybinds based on enum list from dict.
        string fileContent =
            "PLEASE ENSURE EVERYTHING IS CORRECTLY FORMATTED WHEN EDITING THIS FILE AS IT MAY CAUSE ERRORS!\n" +
            "---Sound---\n" +
            "[Sound] Ambient = (" + SoundOptionEnums.Ambient + ") = " + soundDict[SoundOptionEnums.Ambient].ToString() + "\n" +
            "[Sound] Combat = (" + SoundOptionEnums.Combat + ") = " + soundDict[SoundOptionEnums.Combat].ToString() + "\n" +
            "[Sound] Music = (" + SoundOptionEnums.Music + ") = " + soundDict[SoundOptionEnums.Music].ToString() + "\n" +
            "---Keybinds---\n" +
            "[Keybind] Accelerate (" + KeybindMenuEnums.Accelerate + ") = " + keyCodeDict[KeybindMenuEnums.Accelerate].ToString() + "\n" +
            "[Keybind] Decelerate (" + KeybindMenuEnums.Decelerate + ") = " + keyCodeDict[KeybindMenuEnums.Decelerate].ToString() + "\n" +
            "[Keybind] Left (" + KeybindMenuEnums.Left + ") = " + keyCodeDict[KeybindMenuEnums.Left].ToString() + "\n" +
            "[Keybind] Right (" + KeybindMenuEnums.Right + ") = " + keyCodeDict[KeybindMenuEnums.Right].ToString() + "\n" +
            "[Keybind] Options (" + KeybindMenuEnums.Options + ") = " + keyCodeDict[KeybindMenuEnums.Options].ToString();

        System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "Settings.txt", fileContent);
    }
    
    private void LoadSettings()
    {
        string[] lines = System.IO.File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "Settings.txt");

        foreach(string line in lines)
        {
            if (line.StartsWith("[Sound]"))
            {
                ParseSoundSettings(line);
            } else
            if (line.StartsWith("[Keybind]"))
            {
                ParseKeybindSettings(line);
            } else { Debug.LogError("Settings file not found!"); }
        }
    }

    private void ParseKeybindSettings(string line)
    {
        // Splits the string
        string[] parts = line.Split('=');
        if (parts.Length == 2)
        {
            // Split at ( gives name and key value of for example 1)
            string[] keyParts = parts[0].Split('(');
            if (keyParts.Length == 2)
            {
                // Get key.
                string dictKeyString = keyParts[1].Trim();
                dictKeyString = dictKeyString.Substring(0, dictKeyString.Length  - 1);
                KeybindMenuEnums key = StaticHelpers.GetEnumFromString<KeybindMenuEnums>(dictKeyString);

                // Get Value.
                string dictValue = parts[2].Trim();
                KeyCode keyCode = StaticHelpers.GetEnumFromString<KeyCode>(dictValue);
                
                // Apply.
                if (keyCodeDict.ContainsKey(key))
                {
                    keyCodeDict[key] = keyCode;
                }

            }
        }
    }

    private void ParseSoundSettings(string line)
    {
        // Splits the string
        string[] parts = line.Split('=');
        if (parts.Length == 2)
        {
            // Split at ( gives name and key value of for example 1)
            string[] keyParts = parts[0].Split('(');
            if (keyParts.Length == 2)
            {
                // Get key.
                string dictKeyString = keyParts[1].Trim();
                dictKeyString = dictKeyString.Substring(0, dictKeyString.Length - 1);
                SoundOptionEnums key = StaticHelpers.GetEnumFromString<SoundOptionEnums>(dictKeyString);

                // Get Value.
                string dictValueString = parts[2].Trim();
                float dictValue = float.Parse(dictValueString);
                // Apply.
                if (soundDict.ContainsKey(key))
                {
                    soundDict[key] = dictValue;
                }

            }
        }

    }
}
