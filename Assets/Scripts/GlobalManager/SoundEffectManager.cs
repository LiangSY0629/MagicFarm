using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance;

    static AudioSource audioSource1;
    static AudioSource audioSource2;
    static AudioSource voiceSource;
    static SoundEffectLibrary SoundEffectLibrary;
    [SerializeField] private Slider sfxSlider;

    float volumeChange = 0f;
    bool change = false;

    //注释与函数与musicEffectManager相同；

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            AudioSource[] audioSources = GetComponents<AudioSource>();
            audioSource1 = audioSources[0];
            audioSource2 = audioSources[1];
            voiceSource = audioSources[2];
            SoundEffectLibrary = GetComponent<SoundEffectLibrary>();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void PlayAudio(string soundName)
    {
        AudioClip audioClip = SoundEffectLibrary.GetRedomClip(soundName);
        if (audioClip != null)
        {
            audioSource1.PlayOneShot(audioClip);
        }

    }

    public void PlaySecondAudio(string soundName)
    {
        AudioClip audioClip = SoundEffectLibrary.GetRedomClip(soundName);
        if (audioClip != null)
        {
            audioSource2.PlayOneShot(audioClip);
        }
    }

    public void PlaySpeakAudio(AudioClip audioClip)
    {
        voiceSource.PlayOneShot(audioClip);
    }



    private void Start()
    {
        sfxSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
        OnValueChanged();
    }

    public static void SetVolume(float volume)
    {
        audioSource1.volume = volume;
        audioSource2.volume = volume * 0.75f;
        voiceSource.volume = volume * 0.3f;
    }

    public void OnValueChanged()
    {
        if (!change)
        {
            volumeChange = sfxSlider.value;
            change = true;
        }
        else
        {
            if (Mathf.Abs(volumeChange - sfxSlider.value) > 0.03f)
            {
                SoundEffectManager.Instance.PlayAudio("Select");
                change = false;
            }
        }
        SetVolume(sfxSlider.value * 0.8f);
    }

}
