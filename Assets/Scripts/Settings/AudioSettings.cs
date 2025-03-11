using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer; // Ссылка на AudioMixer
    [SerializeField] private Slider sfxSlider;      // Ссылка на слайдер громкости SFX

    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
        sfxSlider.value = savedVolume;
        SetSfxVolume(savedVolume);
        
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);
    }

    public void SetSfxVolume(float volume)
    {
        audioMixer.SetFloat("Sfx Volume", Mathf.Log10(volume) * 20);
        
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}