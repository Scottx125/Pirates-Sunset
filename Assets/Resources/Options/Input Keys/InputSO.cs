using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Input", menuName = "ScriptableObjects/Options/Input", order = 2)]
public class InputSO : ScriptableObject
{
    public List<KeyCode> GetInputBlacklist => _blackList;
    [SerializeField]
    private List<KeyCode> _blackList;

    // Go through list, get keycode you want and return that or even set it.
}
