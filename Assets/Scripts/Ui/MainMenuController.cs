using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button[] buttons;

    private void Start()
    {
        buttons = GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(() => GameManager.Instance.CreateNewGame());
        buttons[1].onClick.AddListener(() => GameManager.Instance.LoadGame());
        buttons[2].onClick.AddListener(() => GameManager.Instance.CloseGame());

        if (!SaveController.Instance.Loading())
        {
            buttons[1].gameObject.SetActive(false);
        }

    }

    private void OnDisable()
    {
        buttons[0].onClick.RemoveAllListeners();
        buttons[1].onClick.RemoveAllListeners();
        buttons[2].onClick.RemoveAllListeners();
    }

}
