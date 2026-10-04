using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class SoundEffectLibrary : MonoBehaviour
{
    [SerializeField] public SoundEffctGroup[] soundEffectGroups;
    private Dictionary<string, List<AudioClip>> soundDictionary;

    private void Awake()
    {
        //实例化字典
        InitializeDictionary();
    }

    //把音效或音乐信息添加进字典
    private void InitializeDictionary()
    {
        soundDictionary = new Dictionary<string, List<AudioClip>>();
        foreach(SoundEffctGroup soundEffectGroup in soundEffectGroups)
        {
            soundDictionary[soundEffectGroup.name] = soundEffectGroup.audioClips;
        }
    }

    //多个音效同时被选中时随机播放；
    public AudioClip GetRedomClip(string soundName)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            List<AudioClip> audioClips = soundDictionary[soundName];
            if (audioClips.Count > 0)
            {
                return audioClips[Random.Range(0, audioClips.Count)];
            }
            
        }
        return null;
    }

}



[System.Serializable]
public struct SoundEffctGroup
{
    public string name;
    public List<AudioClip> audioClips;
}