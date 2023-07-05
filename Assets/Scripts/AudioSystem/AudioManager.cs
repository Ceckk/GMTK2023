using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "_SoundManager", menuName = "ScriptableObjects/SoundManager", order = 1)]
public class AudioManager : ScriptableSingleton<AudioManager>
{
    public const float MIN_VOLUME = -80.0f;
    public const float MAX_VOLUME = 0f;

    private const string VOLUME_MASTER = "volume_master";
    private const string VOLUME_MUSIC = "volume_music";
    private const string VOLUME_SFX = "volume_sfx";

    [SerializeField] private AudioMixer _masterMixer;

    public float MasterVolume
    {
        get
        {
            return GetVolume(VOLUME_MASTER);
        }
        set
        {
            SetVolume(VOLUME_MASTER, value);
        }
    }

    public float SfxVolume
    {
        get
        {
            return GetVolume(VOLUME_SFX);
        }
        set
        {
            SetVolume(VOLUME_SFX, value);
        }
    }

    public float MusicVolume
    {
        get
        {
            return GetVolume(VOLUME_MUSIC);
        }
        set
        {
            SetVolume(VOLUME_MUSIC, value);
        }
    }

    void Start()
    {
        _masterMixer.SetFloat(VOLUME_MUSIC, MasterVolume);
        _masterMixer.SetFloat(VOLUME_MUSIC, MusicVolume);
        _masterMixer.SetFloat(VOLUME_SFX, SfxVolume);
    }

    private float GetVolume(string name)
    {
        return PlayerPrefs.GetFloat(name, MAX_VOLUME);
    }

    private void SetVolume(string name, float value)
    {
        value = Mathf.Clamp(value, MIN_VOLUME, MAX_VOLUME);
        _masterMixer.SetFloat(name, value);
        PlayerPrefs.SetFloat(name, value);
    }
}
