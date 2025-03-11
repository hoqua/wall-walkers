
using UnityEngine;


public class MimicSoundController : MonoBehaviour
{
    private AudioSource _audioSource;
    
    [SerializeField]private AudioClip mimicBiteSound;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayMimicBiteSound()
    {
        if (mimicBiteSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(mimicBiteSound); // Воспроизводим звук
        }
    }
}
