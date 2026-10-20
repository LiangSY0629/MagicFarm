using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isLoadGame = false;
    public Color fadeColor;

    public double totalPlayGame;
    public DateTime currentUTC;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    /// <summary>
    /// 更新总游戏时间；
    /// </summary>
    public void SetPlayTime()
    {
        totalPlayGame += (DateTime.UtcNow - currentUTC).TotalSeconds;
        currentUTC = DateTime.UtcNow;
    }

    /// <summary>
    /// 创建一个新游戏；
    /// </summary>
    public void CreateNewGame()
    {
        isLoadGame = false;
        EnterGame();
    }

    /// <summary>
    /// 加载游戏；
    /// </summary>
    public void LoadGame()
    {
        if (!SaveController.Instance.Loading())
        {
            return;
        }

        isLoadGame = true;
        EnterGame();
    }

    public void ReturnMainMenu()
    {
        isLoadGame = false;
        ExitGame();
    }

    public void CloseGame()
    {
        SaveController.Instance.TotalSave();    //关闭游戏时保存当前总时间；
        SoundEffectManager.Instance.PlayAudio("Confirm");
        Application.Quit();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif

    }


    async void EnterGame()
    {
        SoundEffectManager.Instance.PlayAudio("Confirm");
        SaveController.Instance.TotalSave();        //进入游戏时保存总时间；
        await SceneFader.Instance.FadeIn(fadeColor);

        SceneManager.LoadScene("MainScene");

        await SceneFader.Instance.FadeOn();

        if (isLoadGame)
        {
            SaveController.Instance.LoadGame();
        }

        await SceneFader.Instance.FadeOut();
    }

    async void ExitGame()
    {
        SaveController.Instance.TotalSave();        //返回主界面时保存总时间；
        await SceneFader.Instance.FadeIn(fadeColor);

        SceneManager.LoadScene("MainMenuScene");

        await SceneFader.Instance.FadeOn();

        await SceneFader.Instance.FadeOut();
    }


}
