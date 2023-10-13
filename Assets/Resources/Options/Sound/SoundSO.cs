using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Sound", menuName = "ScriptableObjects/Options/Sound", order = 1)]
public class SoundSO : ScriptableObject
{
    public List<SoundOptionObject> GetSoundOptionsData => _soundOptionsDataList;

    [SerializeField]
    private List<SoundOptionObject> _soundOptionsDataList;

}
