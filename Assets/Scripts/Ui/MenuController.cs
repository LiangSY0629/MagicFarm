using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject uiEmotes;
    public TabController tabController;
    void Start()
    {
        menuCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
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



}
