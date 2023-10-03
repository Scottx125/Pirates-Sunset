using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Input", menuName = "ScriptableObjects/Input", order = 2)]
public class InputSO : ScriptableObject
{
    [SerializeField]
    private List<KeyCodeStruct> _inputList;

    // Go through list, get keycode you want and return that or even set it.
}
