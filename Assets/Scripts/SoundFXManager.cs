using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public AudioClip mergeSoundFX;
    public float mergeSoundVolume = 1f;

    public AudioClip dropSoundFX;
    public float dropSoundVolume = 1f;

    [HideInInspector]
    public AudioSource source;

    // Start is called before the first frame update
    void Awake()
    {
        source = GetComponent<AudioSource>();
        
    }

    public void AdjustVolume(float v)
    {
        source.volume = v;
        PlayerPrefs.SetFloat("SoundFXVolume", source.volume);
        PlayerPrefs.Save();
    }

    public void SetMute(bool isMute)
    {
        source.mute = isMute;
        PlayerPrefs.SetInt("MuteSoundFX", isMute ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void PlayMergeSoundFX()
    {
        source.PlayOneShot(mergeSoundFX, mergeSoundVolume);
    }

    public void PlayDropSoundFX()
    {
        source.PlayOneShot(dropSoundFX, dropSoundVolume);
    }
}
