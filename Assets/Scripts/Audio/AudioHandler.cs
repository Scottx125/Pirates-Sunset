using PirateGame.Helpers;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    [SerializeField]
    private SoundSO _soundSO;
    [SerializeField]
    private AudioClip[] _clips;
    [SerializeField]
    private SoundOptionEnums _soundOptionsEnum;
    [SerializeField]
    private AudioSource _audioSource;

    private SoundOptionObject _soundOptionObject;
    private void OnEnable()
    {
        SoundOptions.OnSoundOptionsApplyEvent += UpdateSoundSettings;
    }

    private void OnDisable()
    {
        SoundOptions.OnSoundOptionsApplyEvent -= UpdateSoundSettings;
    }

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        if (_audioSource == null)
        {
            Debug.LogError("AudioHandler has no AudioSource.");
            return;
        }
        if (_soundSO == null)
        {
            Debug.LogError("AudioHandler has no SoundSO.");
            return;
        }
        else { _soundOptionObject = StaticHelpers.GetRequiredSoundObject(_soundSO.GetSoundOptionsData, _soundOptionsEnum); }

        // After every dependency is accounted for set sound options to initial.
        UpdateSoundSettings();
    }

    public void PlayAudio()
    {
        if (_clips.Length == 0)
        {
            Debug.LogError("AudioHandler has no clips.");
            return;
        }
        int soundToPlay = Random.Range(0, _clips.Length - 1);
        _audioSource.clip = _clips[soundToPlay];
        _audioSource.Play();
    }

    private void UpdateSoundSettings()
    {
        _audioSource.volume = _soundOptionObject.GetSetSoundLevel;
    }
}
