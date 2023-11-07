using PirateGame.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SettingsSystem : MonoBehaviour
{
    public static SettingsSystem Instance { get; private set; }

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
            "---Sound---\n" +
            "[Sound] Ambient = " + ambientVolume + "\n" +
            "[Sound] Combat = " + combatVolume + "\n" +
            "[Sound] Music = " + musicVolume + "\n" +
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
        string[] parts = line.Split('=');

        if (parts.Length == 2)
        {
            string key = parts[0].Trim();
            float value = float.Parse(parts[1].Trim());

            switch (key)
            {
                case "Ambient":
                    ambientVolume = value;
                    break;
                case "Combat":
                    combatVolume = value;
                    break;
                case "Music":
                    musicVolume = value;
                    break;
            }
        }

    }
}
