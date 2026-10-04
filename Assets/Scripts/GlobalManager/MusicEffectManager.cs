using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MusicEffectManager : MonoBehaviour
{
    public static MusicEffectManager Instance;

    static AudioSource audioSource;
    static SoundEffectLibrary SoundEffectLibrary;
    [SerializeField] private Slider MusicSlider;
    float time = 450f;
    float BgmTime;
    float fadeVolume = 1f;
    float fadeTime = 0;
    bool music = true;

    float volumeChange = 0;
    bool change = true;

    //静态化该脚本，同一时间只能存在一个；
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            SoundEffectLibrary = GetComponent<SoundEffectLibrary>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //绑定滑块，只要滑块发生变化，调用音量改变函数；
    private void Start()
    {
        MusicSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
    }

    //判断该不该有音乐，并用倒计时判断什么时候切歌
    private void Update()
    {
        if (music)
        {
            if (BgmTime > 0)
            {
                BgmTime -= Time.deltaTime;
            }
            else
            {
                BgmTime = time;
                PlayMusic("BackGroundMusic");
            }
        }
        
    }
    public void OnValueChanged()
    {
        //滑块音效，当滑块变化超过0.02，相当于两点音量值，播放一次音效；
        if (!change)
        {
            volumeChange = MusicSlider.value;
            change = true;
        }
        else
        {
            if (Mathf.Abs(volumeChange - MusicSlider.value) > 0.03f)
            {
                SoundEffectManager.Instance.PlayAudio("Select");
                change = false;
            }
        }
        SetVolume(MusicSlider.value);
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume * fadeVolume;
    }

    //切换歌曲函数，从soundEffectLibrary中获取歌曲信息，播放；
    public void PlayMusic(string soundName)
    {

        AudioClip audioClip = SoundEffectLibrary.GetRedomClip(soundName);
        if (audioClip != null)
        {
            if (fadeTime == 0)
            {
                fadeTime = 2.5f;
                audioSource.clip = audioClip;
                audioSource.Play();
                StartCoroutine(volumeUp());
            }
            else
            {
                StartCoroutine(VolumeDown(audioClip));
            }
        }

    }

    //音量淡出
    IEnumerator VolumeDown(AudioClip audioClip)
    {
        for (float time = 0;time < fadeTime; time += Time.deltaTime)
        {
            fadeVolume = Mathf.Lerp(1, 0, time / fadeTime);
            OnValueChanged();
            yield return null;
        }
        fadeVolume = 0;
        audioSource.clip = audioClip;
        audioSource.Play();
        StartCoroutine(volumeUp());
    }

    //音量淡入
    IEnumerator volumeUp()
    {
        for (float time = 0; time < fadeTime; time += Time.deltaTime)
        {
            fadeVolume = Mathf.Lerp(0, 1, time / fadeTime);
            OnValueChanged();
            yield return null;
        }
        fadeVolume = 1;
    }



}
