using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject uiEmotes;
    public TabController tabController;
    public GameObject teachingPanel;

    public float openTeachingTime = 5f;
    public float updateTime = 30f;
    bool openFirst = false;

    void Start()
    {
        menuCanvas.SetActive(false);
        teachingPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //简易判断是不是第一次进入游戏，如果是，倒计时结束后打开教学面板；
        if (!GameManager.Instance.isLoadGame && openTeachingTime > 0 && !openFirst)
        {
            openTeachingTime -= Time.deltaTime;
        }

        if(openTeachingTime <= 0 && !openFirst)
        {
            teachingPanel.SetActive(true);
            openFirst = true;
            PauseController.SetPause(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuCanvas.activeSelf == false)
            {
                openMenu();
                SoundEffectManager.Instance.PlayAudio("Select");
                tabController.ActivePlayer();
            }
            else
            {
                CloseMenu();
            }

        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            openMenu();
            SoundEffectManager.Instance.PlayAudio("Select");
            tabController.ActiveBag();
        }

        if(updateTime > 0)
        {
            updateTime -= Time.deltaTime;
        }
        else
        {
            SaveController.Instance.TotalSave();
            if (menuCanvas.activeSelf)
            {
                SetTotalTime();
            }
            updateTime = 30f;
        }

    }

    public void openMenu()
    {
        if (menuCanvas.activeSelf == false && PauseController.IsGamePaused)
        {
            return;
        }
        menuCanvas.SetActive(!menuCanvas.activeSelf);
        uiEmotes.SetActive(!uiEmotes.activeSelf);
        PauseController.SetPause(menuCanvas.activeSelf);

        SetTotalTime();
    }

    /// <summary>
    /// 调用函数更新总时间的显示；
    /// </summary>
    public void SetTotalTime()
    {
        Debug.Log("yes");
        GameManager.Instance.SetPlayTime();
        TimeController.Instance.currentGameTime = GameManager.Instance.totalPlayGame;
        TimeController.Instance.SetTruthTime();
    }


    /// <summary>
    /// 按下关闭按钮；
    /// </summary>
    public void CloseMenu()
    {
        SoundEffectManager.Instance.PlayAudio("Cancel");
        menuCanvas.SetActive(!menuCanvas.activeSelf);
        uiEmotes.SetActive(!uiEmotes.activeSelf);
        PauseController.SetPause(menuCanvas.activeSelf);
    }

    /// <summary>
    /// 按下设置按钮；
    /// </summary>
    public void SettingButton()
    {
        openMenu();
        SoundEffectManager.Instance.PlayAudio("Select");
        tabController.ActiveSetting();
    }

    /// <summary>
    /// 按下成就按钮；
    /// </summary>
    public void AchieveButton()
    {
        openMenu();
        SoundEffectManager.Instance.PlayAudio("Select");
        tabController.ActiveAchieve();
    }

    /// <summary>
    /// 按下玩家按钮；
    /// </summary>
    public void PlayerButton()
    {
        openMenu();
        SoundEffectManager.Instance.PlayAudio("Select");
        tabController.ActivePlayer();
    }

    /// <summary>
    /// 按下购物按钮；
    /// </summary>
    public void ShoppingButton()
    {
        openMenu();
        SoundEffectManager.Instance.PlayAudio("Select");
        tabController.ActiveShop();
    }

    /// <summary>
    /// 按下魔法按钮；
    /// </summary>
    public void MagicButton()
    {
        openMenu();
        SoundEffectManager.Instance.PlayAudio("Select");
        tabController.ActiveMagic();
    }

    /// <summary>
    /// 按下教学按钮；
    /// </summary>
    public void TeachingButton()
    {
        teachingPanel.SetActive(true);
        PauseController.SetPause(true);
        SoundEffectManager.Instance.PlayAudio("Select");
    }

    /// <summary>
    /// 关闭教学手册;
    /// </summary>
    public void CloseTeachingPanel()
    {
        teachingPanel.SetActive(false);
        PauseController.SetPause(false);
        SoundEffectManager.Instance.PlayAudio("Cancel");
    }

}
