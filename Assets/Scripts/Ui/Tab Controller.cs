using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    public Image[] tab_images;
    public GameObject tabPoint;
    public GameObject[] pages;
    public TMP_Text tips;
    public List<string> tipsText;

    bool first = true;

    /// <summary>
    /// 打开个人面板；
    /// </summary>
    public void ActivePlayer()
    {
        ActivateTab(0);
        RangeTips();
    }

    /// <summary>
    /// 打开背包面板；
    /// </summary>
    public void ActiveBag()
    {
        ActivateTab(1);
        RangeTips();
    }


    public void ActiveShop()
    {
        ActivateTab(2);
        RangeTips();
    }

    /// <summary>
    /// 打开魔法面板；
    /// </summary>
    public void ActiveMagic()
    {
        ActivateTab(3);
        RangeTips();
    }

    /// <summary>
    /// 打开成就面板；
    /// </summary>
    public void ActiveAchieve()
    {
        ActivateTab(4);
        RangeTips();
    }

    /// <summary>
    /// 打开设置面板；
    /// </summary>
    public void ActiveSetting()
    {
        ActivateTab(5);
        RangeTips();
    }


    /// <summary>
    /// 刷新tips；
    /// </summary>
    public void RangeTips()
    {
        if (tipsText.Count > 0)
        {
            int tip = Random.Range(0, tipsText.Count);
            tips.text = "Tips：" + tipsText[tip];
        }
    }


    public void ActivateTab(int tab_no)
    {
        if (!first)
        {
            SoundEffectManager.Instance.PlayAudio("Select");
        }
        else first = false;

        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
            tab_images[i].color = Color.white * 0.5f + Color.grey * 0.5f;
        }
        pages[tab_no].SetActive(true);
        tab_images[tab_no].color = Color.white;

        
        tabPoint.transform.SetParent(tab_images[tab_no].transform);
        tabPoint.transform.localPosition = Vector3.zero;

    }


}
