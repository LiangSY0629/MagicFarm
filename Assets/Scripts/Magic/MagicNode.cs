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
    public Sprite lightIcon;
    public Image buttonIcon;
    public Image buttonImage;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => MagicManager.Instance.MagicLevelUp(ID));
    }

}