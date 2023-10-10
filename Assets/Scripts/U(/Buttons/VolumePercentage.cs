using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VolumePercentage : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;

    public void UpdatePercentage(float value)
    {
        text.text = Mathf.RoundToInt(value * 100) + "%";
    }
}
