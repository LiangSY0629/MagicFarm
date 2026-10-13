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

    public void SelectSeed(int choose)
    {
        
        SoundEffectManager.Instance.PlayAudio("Confirm");
        seedimage.sprite = seed[choose].GetComponent<Image>().sprite;
        seedChoose.SetActive(false);
        currentSeed = choose + 1;
    }

}
