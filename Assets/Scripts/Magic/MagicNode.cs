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
    public Sprite lightIcon; //点亮时的图标；
    public Image buttonIcon;
    public Image buttonImage;
    public Button button;
    public TMP_Text levelText;

    private void Start()
    {
        button = GetComponent<Button>();
        levelText = GetComponentInChildren<TMP_Text>();
        button.onClick.AddListener(() => MagicManager.Instance.MagicLevelUp(this));
    }

}