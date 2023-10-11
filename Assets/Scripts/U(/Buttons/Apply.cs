using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apply : MonoBehaviour
{
    [SerializeField]
    List<IApplySettings> _settingsObjects;
    public void ApplySettings()
    {
        foreach (IApplySettings obj in _settingsObjects)
        {
            obj.Apply();
        }
    }
}
