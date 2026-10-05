using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance { get;set;}


    [Header("Combat Stats")]
    public int damage;
    public Tools current_tool = Tools.None;

    [Header("Player Stats")]
    public float speed;
    public int square = 1;

    [Header("Item Stats")]
    public int maxPickNumber = 1;
    public int maxPileNumber = 99;
    public int maxQuality = 1;

    [Header("Animals Stats")]
    public int currentAnimalTypes;
    public int animalNumber;
    public int maxAnimalTypes;

    [Header("Plant Stats")]
    public int maxSeed;
    public int seedLevel;
    public int EveryLevelSeed;



    [Header("Mail Stats")]
    public int maxMail;
    public int orderLevel;
    public float rewardLevel;
    public float orderInterval;
    public int goldCount;

    public enum Tools
    {
        None,
        Axe,
        Hoe,
        Kettle,
        Seed,
    }

    public enum ItemTypes
    {
        Crop,
        Animal,
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

