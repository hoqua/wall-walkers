
using UnityEngine;


public class ItemSelectScreenSoundController : MonoBehaviour
{
    private AudioSource _audioSource;

    [SerializeField] private AudioClip menuSelectSound;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayMenuSelectSound()
    {
        if (menuSelectSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(menuSelectSound);
        }
    }
}
