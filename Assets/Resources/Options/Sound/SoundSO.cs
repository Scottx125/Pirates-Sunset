using UnityEngine;
[CreateAssetMenu(fileName = "Sound", menuName = "ScriptableObjects/Options/Sound", order = 1)]
public class SoundSO : ScriptableObject
{
    public float Music { get { return _musicAudioLevel; } set { _musicAudioLevel = Mathf.Clamp01(value); } }
    public float Ambient { get { return _ambientAudioLevel; } set { _ambientAudioLevel = Mathf.Clamp01(value); } }
    public float Combat { get { return _combatAudioLevel; } set { _combatAudioLevel = Mathf.Clamp01(value); } }

    [SerializeField]
    [Range(0, 1)]
    private float _musicAudioLevel;

    [SerializeField]
    [Range(0, 1)]
    private float _ambientAudioLevel;

    [SerializeField]
    [Range(0, 1)]
    private float _combatAudioLevel;

}
