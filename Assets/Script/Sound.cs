using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public bool isPlaying { get; private set; }
    private AudioSource sound;
    
    private void Start()
    {
        isPlaying = false;
        sound = GetComponent<AudioSource>();
    }
    
    public void PlayOrPause()
    {
        if (sound.isPlaying)
        {
            sound.Pause();
        }
        else
        {
            sound.Play();
        }
    }
    
}

