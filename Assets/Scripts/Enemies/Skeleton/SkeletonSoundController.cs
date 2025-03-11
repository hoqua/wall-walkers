using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class SkeletonSoundController : MonoBehaviour
{
    private AudioSource _audioSource;
    
    [SerializeField] private AudioClip skeletonAttackSound;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    public IEnumerator PlayDoubleSwordHitSound()
    {
        if (skeletonAttackSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(skeletonAttackSound); 
            yield return new WaitForSeconds(0.15f); // Задержка между ударами
            _audioSource.PlayOneShot(skeletonAttackSound);
        }
    }
}
