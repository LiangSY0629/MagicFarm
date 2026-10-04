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


    public void ActivePlayer()
    {
        ActivateTab(0);
        if (tipsText.Count > 0)
        {
            int tip = Random.Range(0, tipsText.Count);
            tips.text = "Tips£º" + tipsText[tip];
        }

    }

    public void ActiveBag()
    {
        ActivateTab(1);
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
