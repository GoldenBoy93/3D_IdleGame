using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip[] clip;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void PlayAttackSoundEffect()
    {
        audioSource.PlayOneShot(clip[0]);
    }

    void PlayDeathSoundEffect()
    {
        audioSource.PlayOneShot(clip[1]);
    }
}
