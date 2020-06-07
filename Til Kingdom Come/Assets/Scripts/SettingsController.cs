using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider master, music, soundEffect;
    void Start()
    {
        master.value = PlayerPrefs.GetFloat("Master", 1);
        music.value = PlayerPrefs.GetFloat("Music", 1);
        soundEffect.value = PlayerPrefs.GetFloat("SoundEffect", 1);
    }
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", linearise(volume));
        PlayerPrefs.SetFloat("Master", volume);
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music", linearise(volume));
        PlayerPrefs.SetFloat("Music", volume);
    }
    public void SetSoundEffectVolume(float volume)
    {
        audioMixer.SetFloat("SoundEffect", linearise(volume));
        PlayerPrefs.SetFloat("SoundEffect", volume);
    }

    private float linearise(float value)
    {
        return Mathf.Log10(value) * 20;
    }
}
