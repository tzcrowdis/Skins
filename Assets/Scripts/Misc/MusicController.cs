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
    AudioSource audioSource;
    
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
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = background;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void ChangeMusic(Scenario scenario) // TODO consider fading in/out
    {
        switch (scenario)
        {
            case Scenario.Default:
                audioSource.clip = background;
                break;
            case Scenario.RandomizerBoss:
                audioSource.clip = randomizer;
                break;
            case Scenario.MonkeyPawBoss:
                audioSource.clip = monkeyPaw;
                break;
            case Scenario.EvilRandomizerBoss:
                audioSource.clip = evilRandomizer;
                break;
        }
        audioSource.loop = true;
        audioSource.Play();
    }
}
