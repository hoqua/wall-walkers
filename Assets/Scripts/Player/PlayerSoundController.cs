using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerSoundController : MonoBehaviour
{
    private AudioSource _audioSource;
    
    [SerializeField] private AudioClip gemPickupSound;
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
