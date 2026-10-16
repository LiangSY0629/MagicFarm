using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool loadFromSave = false;

    public void LoadNewGame()
    {
        loadFromSave = false;
        SceneManager.LoadScene("GameScene");
    }

    public void LoadGameFromSave()
    {
        loadFromSave = true;
        SceneManager.LoadScene("GameScene");
    }

}
