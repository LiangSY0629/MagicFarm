using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static bool loadFromSave = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject.transform.parent);
        }
        else Destroy(gameObject);

    }

    public void LoadNewGame()
    {
        loadFromSave = false;
        FadeLoad();
    }

    public void LoadGameFromSave()
    {
        loadFromSave = true;
        FadeLoad();
    }


    async void FadeLoad()
    {
        await SceenFader.Instance.FadeIn();

        SceneManager.LoadScene("GameScene");

        await SceenFader.Instance.FadeOn();

        if(loadFromSave)
            SaveController.Instance.LoadGame();

        await SceenFader.Instance.FadeOut();
    }

    public void EnterMain()
    {
        loadFromSave = false;
        FadeEnter();
    }

    async void FadeEnter()
    {
        await SceenFader.Instance.FadeIn();

        SceneManager.LoadScene("MainScene");

        await SceenFader.Instance.FadeOn();

        await SceenFader.Instance.FadeOut();
    }

}
