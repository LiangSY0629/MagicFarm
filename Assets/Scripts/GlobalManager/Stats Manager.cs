using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance { get;set;}


    [Header("Player Stats")]
    public float speed;//角色速度；  
    //public int square = 1;//攻击范围；   1
    public Stat square = new(1);

    [Header("Item Stats")]
    public Stat maxPickNumber = new(1);
    public int maxPileNumber = 99;//最大可堆叠数量；
    public Stat maxQuality = new(1);

    [Header("Animals Stats")]
    public Stat currentAnimalTypes = new(1);
    public Stat animalEatTime = new(1);
    public int animalNumber;//动物种类数量；
    public int maxAnimalTypes;//动物品种总数；

    [Header("Plant Stats")]
    public int maxSeed;//种子总数；  
    public Stat seedLevel = new(1);
    public Stat plantGrowth = new(1);
    public Stat plantWater = new(1);
    public int EveryLevelSeed;//每个等级的种子数量；


    [Header("Mail Stats")]
    public int maxMail;//最大可容纳订单数；         ？
    public Stat orderLevel = new(1);
    public Stat rewardLevel = new(1);
    public Stat orderInterval = new(30);
    public int goldCount;//金币总量；


    [Header("Achieve Stats")]
    public int achieveCompleteCount;//当前解锁的成就数量；

    [Header("Magic Stats")]
    public int magicStone = 2;

    [Header("绑定个人信息面板")]
    public TMP_Text achieveText;
    public TMP_Text goldText, timeText;

    public Dictionary<LevelMagics, Stat> levelEffectDictionary;
    public Dictionary<PercentMagics, Stat> percentEffectDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        EventController.OnAchieveUp += AchieveCountUp;

        levelEffectDictionary = new Dictionary<LevelMagics, Stat>();
        percentEffectDictionary = new Dictionary<PercentMagics, Stat>();

        levelEffectDictionary[LevelMagics.CropLevel] = seedLevel;
        levelEffectDictionary[LevelMagics.AnimalLevel] = currentAnimalTypes;
        levelEffectDictionary[LevelMagics.OrderLevel] = orderLevel;
        levelEffectDictionary[LevelMagics.PickLevel] = maxPickNumber;
        levelEffectDictionary[LevelMagics.QualityLevel] = maxQuality;

        percentEffectDictionary[PercentMagics.CropGrowth] = plantGrowth;
        percentEffectDictionary[PercentMagics.CropWater] = plantWater;
        percentEffectDictionary[PercentMagics.AnimalEat] = animalEatTime;
        percentEffectDictionary[PercentMagics.OrderGlod] = rewardLevel;
        percentEffectDictionary[PercentMagics.OrderInterval] = orderInterval;

    }

    /// <summary>
    /// 成就解锁数量+1；
    /// </summary>
    /// <param name="number"></param>
    public void AchieveCountUp(int number)
    {
        achieveCompleteCount += number;
        achieveText.text = "获得成就数：" + achieveCompleteCount;
    }

    ///// <summary>
    ///// 调用函数获取需要存档的变量信息；
    ///// </summary>
    ///// <returns></returns>
    //public StatsSaveData GetSaveData()
    //{
    //    StatsSaveData statsData = new StatsSaveData();

    //    statsData.currentGlodCount = goldCount;
    //    statsData.square = square.finalValue;
    //    statsData.achieveCompleteCount = achieveCompleteCount;

    //    return statsData;
    //}

    ///// <summary>
    ///// 传入存档的变量为当前变量赋值；
    ///// </summary>
    ///// <param name="statsSaveData"></param>
    //public void SetSaveData(StatsSaveData statsSaveData)
    //{
    //    goldCount = statsSaveData.currentGlodCount;
    //    square = statsSaveData.square;
    //    achieveCompleteCount = statsSaveData.achieveCompleteCount;
    //    achieveText.text = "获得成就数：" + achieveCompleteCount;
    //}


    public void LevelStatUp(LevelMagics type, float value)
    {
        levelEffectDictionary[type].LevelUp(value);
        Debug.Log(levelEffectDictionary[type].levelAdd);
    }

    public void PercentStatUp(PercentMagics type, float value)
    {
        percentEffectDictionary[type].PercentUp(value);
        Debug.Log(percentEffectDictionary[type].percentAdd);
    }


    /// <summary>
    /// 传入需要更改的glodCount，进行更改并同步到UI；
    /// </summary>
    /// <param name="number"></param>
    public void SetGoldCount(int number)
    {
        Debug.Log(number);
        goldCount += number;
        goldText.text = "当前金币数：" + goldCount;
        if(number > 0)
            EventController.TriggerComplete(AchieveCondition.Gold, number);

        TimeController.Instance.glodText.text = goldCount.ToString();
    }

}

[System.Serializable]
public class Stat//创建一个属性类；
{
    public float baseValue;
    public int levelAdd;
    public float percentAdd;
    public float finalValue => (baseValue + levelAdd) * percentAdd;

    public Stat(float value)
    {
        baseValue = value;
        levelAdd = 0;
        percentAdd = 1f;
    }

    /// <summary>
    /// 提升等级；
    /// </summary>
    /// <param name="value"></param>
    public void LevelUp(float value)
    {
        levelAdd += Mathf.RoundToInt(value);
    }

    /// <summary>
    /// 提升百分比；
    /// </summary>
    /// <param name="value"></param>
    public void PercentUp(float value)
    {
        percentAdd += value;
    }

}