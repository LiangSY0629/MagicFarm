using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => MagicManager.Instance.MagicLevelUp(this));
    }

}