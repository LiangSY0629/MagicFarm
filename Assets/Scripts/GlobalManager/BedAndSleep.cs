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
            PlayerSleep();
        }
        else
        {
            TipsPopupControler.Instance.SetTipsText("现在还不是晚上，不可以睡觉");
        }

    }

    void PlayerSleep()
    {

        TimeController.Instance.currentTime += 30 * TimeController.Instance.oneHour - TimeController.Instance.dayTime;
        sleep = false;
    }



}
