using PirateGame.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SettingsSystem : MonoBehaviour
{
    public static SettingsSystem Instance { get; private set; }

    public Dictionary<SoundOptionEnums, float> soundDict = new Dictionary<SoundOptionEnums, float>
    {
        { SoundOptionEnums.Ambient, 0f },
        { SoundOptionEnums.Combat, 0f },
        { SoundOptionEnums.Music, 0f }
    };

    public Dictionary<KeybindMenuEnums, KeyCode> keyCodeDict = new Dictionary<KeybindMenuEnums, KeyCode>
    {
        { KeybindMenuEnums.Accelerate, KeyCode.None },
        { KeybindMenuEnums.Decelerate, KeyCode.None },
        { KeybindMenuEnums.Left, KeyCode.None },
        { KeybindMenuEnums.Right, KeyCode.None },
        { KeybindMenuEnums.Options, KeyCode.None },
    };

    private string _filepath = AppDomain.CurrentDomain.BaseDirectory + "Settings.txt";

    private string defaultContent =
    "PLEASE ENSURE EVERYTHING IS CORRECTLY FORMATTED WHEN EDITING THIS FILE AS IT MAY CAUSE ERRORS!\n" +
    "---Sound---\n" +
    SoundOptionEnums.Ambient.ToString() + " = " + .5f.ToString("#.##") + "\n" +
    SoundOptionEnums.Combat.ToString() + " = " + .5f.ToString("#.##") + "\n" +
    SoundOptionEnums.Music.ToString() + " = " + .5f.ToString("#.##") + "\n" +
    "---Keybinds---\n" +
    KeybindMenuEnums.Accelerate.ToString() + " = " + KeyCode.W.ToString() + "\n" +
    KeybindMenuEnums.Decelerate.ToString() + " = " + KeyCode.S.ToString() + "\n" +
    KeybindMenuEnums.Left.ToString() + " = " + KeyCode.A.ToString() + "\n" +
    KeybindMenuEnums.Right.ToString() + " = " + KeyCode.D.ToString() + "\n" +
    KeybindMenuEnums.Options.ToString() + " = " + KeyCode.Escape.ToString();

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
        if (File.Exists(_filepath))
        {
            LoadSettings();
        } else
        {
            SaveSettings(true);
            LoadSettings();
        }
    }


    public void SaveSettings(bool createDefault)
    {
        string fileContent;
        if (createDefault)
        {
            fileContent = defaultContent;
        } else {
            // Save keybinds based on enum list from dict.
            fileContent =
                "PLEASE ENSURE EVERYTHING IS CORRECTLY FORMATTED WHEN EDITING THIS FILE AS IT MAY CAUSE ERRORS!\n" +
                "---Sound---\n" +
                SoundOptionEnums.Ambient.ToString() + " = " + soundDict[SoundOptionEnums.Ambient].ToString("#.##") + "\n" +
                SoundOptionEnums.Combat.ToString() + " = " + soundDict[SoundOptionEnums.Combat].ToString("#.##") + "\n" +
                SoundOptionEnums.Music.ToString() + " = " + soundDict[SoundOptionEnums.Music].ToString("#.##") + "\n" +
                "---Keybinds---\n" +
                KeybindMenuEnums.Accelerate.ToString() + " = " + keyCodeDict[KeybindMenuEnums.Accelerate].ToString() + "\n" +
                KeybindMenuEnums.Decelerate.ToString() + " = " + keyCodeDict[KeybindMenuEnums.Decelerate].ToString() + "\n" +
                KeybindMenuEnums.Left.ToString() + " = " + keyCodeDict[KeybindMenuEnums.Left].ToString() + "\n" +
                KeybindMenuEnums.Right.ToString() + " = " + keyCodeDict[KeybindMenuEnums.Right].ToString() + "\n" +
                KeybindMenuEnums.Options.ToString() + " = " + keyCodeDict[KeybindMenuEnums.Options].ToString();
        }

        System.IO.File.WriteAllText(_filepath, fileContent);
    }
    
    private void LoadSettings()
    {
        string[] lines = System.IO.File.ReadAllLines(_filepath);
        foreach(string line in lines)
        {
            if (!line.Contains("=")) continue;

            string[] stringParts = SplitString(line);
            if (StaticHelpers.IsDefinedIgnoreCase(typeof(SoundOptionEnums), stringParts[0]))
            {
                ParseSoundSettings(stringParts);
            }
            else
            if (StaticHelpers.IsDefinedIgnoreCase(typeof(KeybindMenuEnums), stringParts[0]))
            {
                ParseKeybindSettings(stringParts);
            }
        }
    }

    private string[] SplitString(string line)
    {
        // Splits the string
        string[] parts = line.Split('=');
        parts[0] = parts[0].Trim();
        parts[1] = parts[1].Trim();
        return parts;
    }

    private void ParseKeybindSettings(string[] parts)
    {
        // Get key.
        KeybindMenuEnums key = StaticHelpers.GetEnumFromString<KeybindMenuEnums>(parts[0]);

        // Get Value.
        KeyCode keyCode = StaticHelpers.GetEnumFromString<KeyCode>(parts[1]);
                
        // Apply.
        // Check added to prevent duplicate keys.
        if (keyCodeDict.ContainsKey(key) && !keyCodeDict.ContainsValue(keyCode))
        {
            keyCodeDict[key] = keyCode;
        } else
        {
            keyCodeDict[key] = KeyCode.None;
        }
    }

    private void ParseSoundSettings(string[] parts)
    {
        // Get key.
        SoundOptionEnums key = StaticHelpers.GetEnumFromString<SoundOptionEnums>(parts[0]);

        // Get Value.
        float volume = float.Parse(parts[1]);

        // Apply.
        if (soundDict.ContainsKey(key))
        {
            soundDict[key] = Mathf.Clamp01(volume);
        }



    }
}
