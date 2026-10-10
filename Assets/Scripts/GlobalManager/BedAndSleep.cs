using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedAndSleep : MonoBehaviour, IInteractable
{
    public bool sleep;

    public bool CanInteracte()
    {
        return !sleep;
    }

    public void Interacte()
    {
        if (sleep || PauseController.IsGamePaused)
        {
            return;
        }

        SoundEffectManager.Instance.PlayAudio("Confirm");
        if (TimeController.Instance.isNight)
        {
            sleep = true;
            EventController.TriggerComplete(1, 1);
            SleepFade();
        }
        else
        {
            TipsPopupControler.Instance.SetTipsText("现在还不是晚上，不可以睡觉");
        }

    }

    //使用异步委托来淡入淡出转场；
    async void SleepFade()
    {
        await SceenFader.Instance.FadeIn();

        //睡眠后时间 = 黑夜时长 - （（最终时间 + 天亮时间）% 每日时间）；（180 - （（270 + 90）% 360）；
        TimeController.Instance.currentTime += 12 * TimeController.Instance.oneHour - ((TimeController.Instance.finalTime + (6 * TimeController.Instance.oneHour)) % TimeController.Instance.oneDay);
        sleep = false;

        await SceenFader.Instance.FadeOut();
    }


}
