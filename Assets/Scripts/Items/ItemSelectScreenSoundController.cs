using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
        _audioSource.PlayOneShot(menuSelectSound); 
    }
}
