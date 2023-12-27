using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JukeBox : MonoBehaviour
{
    public int musicIndex = 0;
    public AudioClip[] musicList;

    [HideInInspector]
    public AudioSource source;
    private bool randomize = false;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.loop = true;
    }


    public void PlayNextMusic(int index)
    {
        musicIndex = index;
        PlayerPrefs.SetInt("MusicIndex", index);
        PlayerPrefs.Save();
        source.clip = musicList[musicIndex];
        source.Play();
    }

    public void AdjustVolume(float v)
    {
        source.volume = v;
        PlayerPrefs.SetFloat("MusicVolume", source.volume);
        PlayerPrefs.Save();
    }

    public void SetRandomize(bool r)
    {
        randomize = r;
    }

    public void SetMute(bool isMute)
    {
        source.mute = isMute;
        PlayerPrefs.SetInt("MuteMusic", isMute? 1 : 0);
        PlayerPrefs.Save();
    }
}
