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

    public void ChangeMusic(Scenario scenario)
    {
        switch (scenario)
        {
            case Scenario.Default:
                if (bossMusicSource.isPlaying)
                {
                    StartCoroutine(FadeOut(bossMusicSource, 1f));
                    backgroundSource.volume *= 2f;
                } 
                else
                    bossMusicSource.Stop();

                backgroundSource.clip = background;
                backgroundSource.loop = true;
                if (backgroundSource.isPlaying)
                    backgroundSource.Play();
                else
                    StartCoroutine(FadeIn(backgroundSource, 1f));
                break;

            case Scenario.RandomizerBoss:
                backgroundSource.volume *= 0.5f;

                bossMusicSource.clip = randomizer;
                bossMusicSource.loop = true;
                bossMusicSource.pitch = 0.75f;
                StartCoroutine(FadeIn(bossMusicSource, 2.5f));
                break;

            case Scenario.MonkeyPawBoss:
                backgroundSource.volume *= 0.5f;

                bossMusicSource.clip = monkeyPaw;
                bossMusicSource.loop = true;
                bossMusicSource.pitch = 1f;
                StartCoroutine(FadeIn(bossMusicSource, 5f));
                break;

            case Scenario.EvilRandomizerBoss:
                StartCoroutine(FadeOut(backgroundSource, 0.5f));
                bossMusicSource.clip = evilRandomizer;
                bossMusicSource.loop = true;
                bossMusicSource.pitch = 1f;
                StartCoroutine(FadeIn(bossMusicSource, 1f));
                break;
        }
    }

    IEnumerator FadeIn(AudioSource audioSource, float fadeTime)
    {
        float startVolume = 0f;
        float targetVolume = 0.5f; // NOTE make parameter?
        audioSource.volume = startVolume;
        audioSource.Play();

        float timer = 0f;
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, timer / fadeTime);
            yield return null;
        }
        audioSource.volume = targetVolume;
    }

    IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
