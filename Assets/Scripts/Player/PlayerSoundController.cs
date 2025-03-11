using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerSoundController : MonoBehaviour
{
    private AudioSource _audioSource;
    
    [SerializeField] private AudioClip gemPickupSound;
    [SerializeField] private AudioClip potionDrinkSound;
    [SerializeField] private AudioClip chestOpenSound;
    [SerializeField] private AudioClip levelUpSound;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayExpGemPickupSound()
    {
        if (gemPickupSound != null && _audioSource != null)
        {
            _audioSource.volume = 0.1f;
            _audioSource.pitch = Random.Range(0.7f, 1.05f); // Рандомная тональность звука
            _audioSource.PlayOneShot(gemPickupSound); 
        }
    }

    public void PlayPotionDrinkSound()
    {
        if (gemPickupSound != null && _audioSource != null)
        {
            _audioSource.volume = 0.2f;
            _audioSource.pitch = Random.Range(1f, 1.1f); // Рандомная тональность звука
            _audioSource.PlayOneShot(potionDrinkSound); 
        }
    }
    
    public void PlayChestOpenSound()
    {
        if (gemPickupSound != null && _audioSource != null)
        {
            _audioSource.volume = 0.45f;
            _audioSource.pitch = Random.Range(0.9f, 1.1f); // Рандомная тональность звука
            _audioSource.PlayOneShot(chestOpenSound); 
        }
    }
    
    public void PlayLevelUpSound()
    {
        if (levelUpSound != null && _audioSource != null)
        {
            _audioSource.volume = 0.1f;
            _audioSource.pitch = 1; 
            _audioSource.PlayOneShot(levelUpSound); 
        }
    }
}
