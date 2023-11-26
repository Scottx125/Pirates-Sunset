using PirateGame.Helpers;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] _clips;
    [SerializeField]
    private SoundOptionEnums _desiredKeyForThisObj;
    [SerializeField]
    private AudioSource _audioSource;

    private IGetData _systemSettingsGetData;

    private void OnEnable()
    {
        SoundOptions.OnSoundOptionsApplyEvent += UpdateSoundSettings;
    }

    private void OnDisable()
    {
        SoundOptions.OnSoundOptionsApplyEvent -= UpdateSoundSettings;
    }

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        SettingsSystem settingsSystem = FindFirstObjectByType<SettingsSystem>();
        if (settingsSystem == null)
        {
            Debug.LogError("Cannot find SettingsSystem!");
            return;
        }
        if (_audioSource == null)
        {
            Debug.LogError("AudioHandler has no AudioSource.");
            return;
        }
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
        _audioSource.volume = _systemSettingsGetData.GetData<SoundOptionEnums, float>(_desiredKeyForThisObj);
    }
}
