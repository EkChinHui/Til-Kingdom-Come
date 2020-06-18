﻿using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public static AudioManager instance;
    public AudioMixerGroup musicOutput;
    public Sound[] music;
    public AudioMixerGroup soundEffectOutput;
    public Sound[] soundEffect;
    private Sound currentMusic;
    private float fadeOutTime = 1f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach(Sound s in music)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = musicOutput;
        }

        foreach(Sound s in soundEffect)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = soundEffectOutput;
        }
    }

    public void Start()
    {
        // update volume slider in settings page according to player preferences
        audioMixer.SetFloat("Master", linearise(PlayerPrefs.GetFloat("Master", 1)));
        audioMixer.SetFloat("Music", linearise(PlayerPrefs.GetFloat("Music", 1)));
        audioMixer.SetFloat("SoundEffect", linearise(PlayerPrefs.GetFloat("SoundEffect", 1)));
        PlayMusic("Main Theme");
    }

    public void PlayMusic(string name)
    {

        Sound s = Array.Find(music, music => music.name == name);
        if (s == null) {
            Debug.LogWarning("Music: " + name + " not found!");
            return;
        }
        Debug.Log("Playing Music: " + name + ".");
        s.source.Play();
        currentMusic = s;
    }

    public void PlaySoundEffect(string name)
    {
        Sound s = Array.Find(soundEffect, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound Effect: " + name + " not found!");
            return;
        }
        Debug.Log("Playing Sound Effect: " + name + ".");
        s.source.Play();
    }

    public void FadeOutCurrentMusic()
    {
        if (currentMusic == null) {
            Debug.LogWarning("No current music!");
            return;
        }
        Debug.Log("No current music!");
        StartCoroutine(AudioFadeOut(currentMusic.source, fadeOutTime));
        currentMusic = null;
    }

    public void PlayCurrentMusic()
    {
        if (currentMusic == null) {
            Debug.LogWarning("No current music!");
            return;
        }
        Debug.Log("Playing Music: " + currentMusic.name + ".");
        currentMusic.source.Play();
    }

    public void PauseCurrentMusic()
    {
        if (currentMusic == null) {
            Debug.LogWarning("No current music!");
            return;
        }
        Debug.Log("Pausing Music: " + currentMusic.name + ".");
        currentMusic.source.Pause();
    }

    public void StopCurrentMusic()
    {
        if (currentMusic == null) {
            Debug.LogWarning("No current music!");
            return;
        }
        Debug.Log("Stopping Music: " + currentMusic.name + ".");
        currentMusic.source.Stop();
        currentMusic = null;
    }

    private IEnumerator AudioFadeOut(AudioSource audioSource, float fadeTime) {
        float startVolume = audioSource.volume;
 
        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
 
        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    private float linearise(float value)
    {
        return Mathf.Log10(value) * 20;
    }
}
