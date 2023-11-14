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

    private string _filepath = AppDomain.CurrentDomain.BaseDirectory + "Settings.txt";

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        Debug.Log(_filepath);
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
        if (File.Exists(_filepath))
        {
            LoadSettings();
        } else
        {
            SaveSettings();
            LoadSettings();
        }
    }

    public void SaveSettings()
    {
        // Save keybinds based on enum list from dict.
        string fileContent =
            "PLEASE ENSURE EVERYTHING IS CORRECTLY FORMATTED WHEN EDITING THIS FILE AS IT MAY CAUSE ERRORS!\n" +
            "---Sound---\n" +
            SoundOptionEnums.Ambient.ToString() + " = " + soundDict[SoundOptionEnums.Ambient].ToString() + "\n" +
            SoundOptionEnums.Combat.ToString() + " = " + soundDict[SoundOptionEnums.Combat].ToString() + "\n" +
            SoundOptionEnums.Music.ToString() + " = " + soundDict[SoundOptionEnums.Music].ToString() + "\n" +
            "---Keybinds---\n" +
            KeybindMenuEnums.Accelerate.ToString() + " = " + keyCodeDict[KeybindMenuEnums.Accelerate].ToString() + "\n" +
            KeybindMenuEnums.Decelerate.ToString() + " = " + keyCodeDict[KeybindMenuEnums.Decelerate].ToString() + "\n" +
            KeybindMenuEnums.Left.ToString() + " = " + keyCodeDict[KeybindMenuEnums.Left].ToString() + "\n" +
            KeybindMenuEnums.Right.ToString() + " = " + keyCodeDict[KeybindMenuEnums.Right].ToString() + "\n" +
            KeybindMenuEnums.Options.ToString() + " = " + keyCodeDict[KeybindMenuEnums.Options].ToString();

        System.IO.File.WriteAllText(_filepath, fileContent);
    }
    
    private void LoadSettings()
    {
        string[] lines = System.IO.File.ReadAllLines(_filepath);

        foreach(string line in lines)
        {
            if (line.StartsWith("[Sound]"))
            {
                ParseSoundSettings(line);
            } else
            if (line.StartsWith("[Keybind]"))
            {
                ParseKeybindSettings(line);
            }
        }
    }

    private void ParseKeybindSettings(string line)
    {
        // Splits the string
        string[] parts = line.Split('=');
        string stringKey = parts[0].Trim();
        string stringValue = parts[1].Trim();

        // Get key.
        KeybindMenuEnums key = StaticHelpers.GetEnumFromString<KeybindMenuEnums>(stringKey);

        // Get Value.
        KeyCode keyCode = StaticHelpers.GetEnumFromString<KeyCode>(stringValue);
                
        // Apply.
        if (keyCodeDict.ContainsKey(key))
        {
            keyCodeDict[key] = keyCode;
        }
    }

    private void ParseSoundSettings(string line)
    {
        // Splits the string
        string[] parts = line.Split('=');
        string stringKey = parts[0].Trim();
        string stringValue = parts[1].Trim();

        // Get key.
        SoundOptionEnums key = StaticHelpers.GetEnumFromString<SoundOptionEnums>(stringKey);

        // Get Value.
        float volume = float.Parse(stringValue);

        // Apply.
        if (soundDict.ContainsKey(key))
        {
            soundDict[key] = volume;
        }



    }
}
