using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isLoadGame = false;
    public Color fadeColor;

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
        await SceneFader.Instance.FadeIn(fadeColor);

        SceneManager.LoadScene("MainMenuScene");

        await SceneFader.Instance.FadeOn();

        await SceneFader.Instance.FadeOut();
    }


}
