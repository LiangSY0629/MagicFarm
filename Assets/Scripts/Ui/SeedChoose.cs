using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class SeedChoose : MonoBehaviour
{
    public Image seedimage;
    public GameObject seedChoose;
    public GameObject[] seed;
    public int currentSeed;


    private void Start()
    {
        StatsManager.Instance.maxSeed = seed.Length;

        for (int i = StatsManager.Instance.seedLevel * StatsManager.Instance.EveryLevelSeed; i < seed.Length; i++)
        {
            seed[i].SetActive(false);
        }


    }

    public void SelectSeed(int choose)
    {
        
        SoundEffectManager.Instance.PlayAudio("Confirm");
        seedimage.sprite = seed[choose].GetComponent<Image>().sprite;
        seedChoose.SetActive(false);
        currentSeed = choose + 1;
    }

}
