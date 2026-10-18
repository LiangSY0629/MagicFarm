using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.GPUDriven;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MagicNode : MonoBehaviour
{
    public string ID;
    public MagicSO magicSo;
    public Image buttonIcon;
    public Image buttonImage;
    public Button button;
    public TMP_Text levelText;


    private void OnEnable()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => MagicManager.Instance.SetDescriptionPanel(ID));
    }

    private void Start()
    {
        buttonImage = GetComponent<Image>();
        levelText = GetComponentInChildren<TMP_Text>();
    }

    private void OnDisable()
    {
        if(button!=null)
            button.onClick.RemoveAllListeners();
    }

}