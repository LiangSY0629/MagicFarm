using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class EmoteSwitch : MonoBehaviour
{
    public int emoteClick;
    public float emoteTime = 20.0f;

    float IdleEmoteTime;
    int emoteIdle;


    public Animator animEmote;
    public MenuController menu;

    private void Start()
    {
        IdleEmoteTime = emoteTime * 1.2f;
    }
    private void Update()
    {
        //倒计时；只要没打开设置页面就会倒计时，结束时切换表情；
        if (menu.menuCanvas.activeSelf == false)
        {
            if (IdleEmoteTime > 0)
            {
                IdleEmoteTime -= Time.deltaTime;
            }
            else
            {
                idleEmote();
            }
        }
        else
        {
            animEmote.SetFloat("IdleEmote", 0);
        }

    }

    //倒计时切换闲置表情
    private void idleEmote()
    {
        IdleEmoteTime = emoteTime * 1.2f;
        emoteIdle = Random.Range(1, 9);
        animEmote.SetFloat("IdleEmote", emoteIdle);

    }

    //点击切换受击表情
    public void clickEmote()
    {
        emoteClick = Random.Range(0,10);
        animEmote.SetFloat("Emote", emoteClick);
        SoundEffectManager.Instance.PlayAudio("Emotes");
        animEmote.SetBool("Click", true);

        EventController.TriggerComplete(3, 1);

    }

    //切换回闲置表情
    public void overClick()
    {
        animEmote.SetBool("Click", false);
        animEmote.SetFloat("IdleEmote", 0);
        IdleEmoteTime = emoteTime * 1.2f;
    }


    public void returnIdle()
    {
        animEmote.SetFloat("IdleEmote", 0);
        IdleEmoteTime = emoteTime * 1.2f;
    }

}
