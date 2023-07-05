using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
    [SerializeField] private AudioClip[] _clipArray;

    private AudioSource _audioSource;

    public AudioSource Source
    {
        get
        {
            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
            }

            return _audioSource;
        }
    }

    public void Play(int index)
    {
        if (index >= 0 && index < _clipArray.Length)
        {
            Source.clip = _clipArray[index];
            Source.Play();
        }
    }
}
