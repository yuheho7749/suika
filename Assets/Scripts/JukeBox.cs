using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JukeBox : MonoBehaviour
{
    public int musicIndex = 0;
    public AudioClip[] musicList;

    public AudioSource source;
    private bool randomize = false;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        PlayNextMusic(0);
    }

    public void PlayNextMusic(int index)
    {
        musicIndex = index;
        source.clip = musicList[musicIndex];
        source.Play();
    }

    public void AdjustVolume(float v)
    {
        source.volume = v;
    }

    public void SetRandomize(bool r)
    {
        randomize = r;
    }

    public void SetMute(bool isMute)
    {
        source.mute = isMute;
    }
}
