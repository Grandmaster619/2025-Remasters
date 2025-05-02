using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.loop = sound.loop;
        }
    }

    public void Play(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null)
        {
            Debug.LogWarning($"Sound {name} could not be found");
            return;
        }
        if (name == "tankExplosion" || name == "playerShot" || name == "tealShot" || name == "greenShot")
        {
            sound.source.PlayOneShot(sound.clip);
        }
        else if (!sound.source.isPlaying)
        {
            Debug.Log(name + " should be playing");
            sound.source.volume = sound.volume;

            sound.source.Play();
        }
    }

    public void Stop(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null)
        {
            Debug.LogWarning($"Sound {name} could not be found");
            return;
        }
        if (name == "Movement")
        {
            Debug.Log(name + " should not be playing");
            StartCoroutine(StartFade(sound.source, 0.25f, sound.source.volume, 0f));
        }
        else
            sound.source.Stop();

    }


    IEnumerator StartFade(AudioSource audioSource, float duration, float startingVolume, float targetVolume)
    {
        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startingVolume, targetVolume, currentTime / duration);
            yield return null;
        }
        if (targetVolume == 0)
            audioSource.Stop();
    }

    public void ToggleMusic()
    {
        Sound sound = Array.Find(sounds, sound => sound.name == "theme");
        if (sound == null)
        {
            Debug.LogWarning($"Sound {name} could not be found");
            return;
        }
        if (sound.source.isPlaying)
            Stop("theme");
        else
            Play("theme");
    }
}
