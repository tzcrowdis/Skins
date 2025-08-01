using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip background;
    public AudioClip randomizer;
    public AudioClip monkeyPaw;
    public AudioClip evilRandomizer;
    
    [Header("Audio Sources")]
    public AudioSource backgroundSource;
    public AudioSource bossMusicSource;

    public enum Scenario
    {
        Default,
        RandomizerBoss,
        MonkeyPawBoss,
        EvilRandomizerBoss
    }


    public static MusicController instance { get; private set; }
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    void Start()
    {
        backgroundSource.clip = background;
        backgroundSource.loop = true;
        backgroundSource.Play();
    }

    public void ChangeMusic(Scenario scenario) // TODO consider fading in/out
    {
        switch (scenario)
        {
            case Scenario.Default:
                bossMusicSource.Stop();
                backgroundSource.clip = background;
                backgroundSource.loop = true;
                backgroundSource.volume *= 2f;
                backgroundSource.Play();
                break;

            case Scenario.RandomizerBoss:
                backgroundSource.volume *= 0.5f;

                bossMusicSource.clip = randomizer;
                bossMusicSource.loop = true;
                bossMusicSource.pitch = 0.75f;
                bossMusicSource.Play();
                break;

            case Scenario.MonkeyPawBoss:
                backgroundSource.volume *= 0.5f;

                bossMusicSource.clip = monkeyPaw;
                bossMusicSource.loop = true;
                bossMusicSource.pitch = 1f;
                bossMusicSource.Play();
                break;

            case Scenario.EvilRandomizerBoss:
                backgroundSource.Stop();
                bossMusicSource.clip = evilRandomizer;
                bossMusicSource.loop = true;
                bossMusicSource.pitch = 1f;
                bossMusicSource.Play();
                break;
        }
        
    }
}
