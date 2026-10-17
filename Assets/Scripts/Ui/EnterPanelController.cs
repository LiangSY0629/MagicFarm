using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterPanelController : MonoBehaviour
{
    public Button[] buttons;

    private void Start()
    {
        buttons = GetComponentsInChildren<Button>();

        buttons[0].onClick.AddListener(() => GameManager.Instance.EnterMain());
        buttons[2].onClick.AddListener(() => ClosePanel());
    }


    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
