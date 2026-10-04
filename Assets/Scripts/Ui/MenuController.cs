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

    public void CloseMenu()
    {
        SoundEffectManager.Instance.PlayAudio("Cancel");
        menuCanvas.SetActive(!menuCanvas.activeSelf);
        uiEmotes.SetActive(!uiEmotes.activeSelf);
        PauseController.SetPause(menuCanvas.activeSelf);
    }

    public void SettingButton()
    {
        openMenu();
        SoundEffectManager.Instance.PlayAudio("Select");
        tabController.ActivePlayer();
    }

}
