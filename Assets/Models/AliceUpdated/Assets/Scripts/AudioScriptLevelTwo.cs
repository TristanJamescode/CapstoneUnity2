using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScriptLevelTwo : MonoBehaviour
{
    public AudioClip MainMenuMusic;
    public AudioSource MusicSource; 
    void Start()
    {
        MusicSource.clip = MainMenuMusic;
        MusicSource.Play(); 
    }

    // Update is called once per frame
    void Update()
    {
        MusicSource.loop = true; 
    }
}
