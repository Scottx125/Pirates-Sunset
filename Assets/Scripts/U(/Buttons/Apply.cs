using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apply : MonoBehaviour
{
    [SerializeField]
    List<GameObject> _settingsObjects;

    private List<SoundOptions> _soundOptions = new List<SoundOptions>();

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        foreach(GameObject gameObject in _settingsObjects)
        {
            _soundOptions.Add(gameObject.GetComponentInChildren<SoundOptions>());
        }
    }

    public void ApplySettings()
    {
        foreach (IApplySettings obj in _soundOptions)
        {
            obj.Apply();
        }
    }
}
