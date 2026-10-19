using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using TMPro;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance { get;set;}


    [Header("Player Stats")]
    public float speed;//角色速度；  
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
    public int EveryLevelSeed;  //每个等级的种子数量；


    [Header("Mail Stats")]
    public int maxMail;  //最大可容纳订单数；
    public Stat orderLevel = new(1);
    public Stat rewardLevel = new(1);
    public Stat orderInterval = new(30);
    public int goldCount;   //金币总量；


    [Header("Achieve Stats")]
    public int achieveCompleteCount;   //当前解锁的成就数量；

    [Header("Magic Stats")]
    public int level = 0;
    public int expNeed = 100;
    public int currentExp = 0;
    public int magicStone = 2;

    (int, float)[] expCurve =
    {
        (0, 1.20f),
        (20, 1.12f),
        (40, 1.05f),
    };

    [Header("绑定个人信息面板")]
    public TMP_Text achieveText;
    public TMP_Text goldText, timeText;

    public Dictionary<MagicTypes, Stat> magicStatDictionary;

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

    private void OnEnable()
    {
        EventController.OnAchieveUp += SetAchieveCount;
    }

    private void Start()
    {
        magicStatDictionary = new Dictionary<MagicTypes, Stat>();

        //把所有需要修改的属性添加进dictionary；
        magicStatDictionary[MagicTypes.CropLevel] = seedLevel;
        magicStatDictionary[MagicTypes.AnimalLevel] = currentAnimalTypes;
        magicStatDictionary[MagicTypes.OrderLevel] = orderLevel;
        magicStatDictionary[MagicTypes.PickLevel] = maxPickNumber;
        magicStatDictionary[MagicTypes.QualityLevel] = maxQuality;
        magicStatDictionary[MagicTypes.SquareLevel] = square;

        magicStatDictionary[MagicTypes.CropGrowth] = plantGrowth;
        magicStatDictionary[MagicTypes.CropWater] = plantWater;
        magicStatDictionary[MagicTypes.AnimalEat] = animalEatTime;
        magicStatDictionary[MagicTypes.OrderInterval] = orderInterval;
        magicStatDictionary[MagicTypes.OrderGlod] = rewardLevel;
    }

    /// <summary>
    /// 成就解锁数量+1；
    /// </summary>
    /// <param name="number"></param>
    public void SetAchieveCount(int number)
    {
        achieveCompleteCount += number;
        achieveText.text = "获得成就数：" + achieveCompleteCount;
    }

    /// <summary>
    /// 调用函数获取需要存档的变量信息；
    /// </summary>
    /// <returns></returns>
    public StatsSaveData GetSaveData()
    {
        StatsSaveData data = new StatsSaveData();

        data.goldCount = goldCount;
        data.achieveCount = achieveCompleteCount;
        data.magicStone = magicStone;
        data.level = level;
        data.exp = currentExp;

        List<TypeSaveData> saveData = new List<TypeSaveData>();
        foreach(var dic in magicStatDictionary)
        {
            saveData.Add(new TypeSaveData
            {
                type = dic.Key,
                levelAdd = dic.Value.levelAdd,
                percentAdd = dic.Value.percentAdd,
            });
        }
        data.typeSaveDatas = saveData;

        return data;
    }

    /// <summary>
    /// 传入存档的变量为当前变量赋值；
    /// </summary>
    /// <param name="statsSaveData"></param>
    public void SetSaveData(StatsSaveData statsSaveData)
    {
        goldCount = statsSaveData.goldCount;
        SetGoldCount(0);
        achieveCompleteCount = statsSaveData.achieveCount;
        SetAchieveCount(0);
        level = statsSaveData.level;
        currentExp = statsSaveData.exp;
        MailBoxManager.Instance.SetSliderValue(0);
        MailBoxManager.Instance.SetLevelText();
        magicStone = statsSaveData.magicStone;

        foreach(TypeSaveData data in statsSaveData.typeSaveDatas)
        {
            Stat stat = magicStatDictionary[data.type];
            stat.levelAdd = data.levelAdd;
            stat.percentAdd = data.percentAdd;
        }


    }



    public void LevelStatUp(MagicTypes type, float value)
    {
        magicStatDictionary[type].LevelUp(value);
    }

    public void PercentStatUp(MagicTypes type, float value)
    {
        magicStatDictionary[type].PercentUp(value);
    }


    /// <summary>
    /// 传入需要更改的glodCount，进行更改并同步到UI；
    /// </summary>
    /// <param name="number"></param>
    public void SetGoldCount(int number)
    {
        goldCount += number;
        goldText.text = "当前金币数：" + goldCount;
        if(number > 0)
            EventController.TriggerComplete(AchieveCondition.Gold, number);

        TimeController.Instance.glodText.text = goldCount.ToString();
    }

    /// <summary>
    /// 更新当前所需的经验值；
    /// </summary>
    /// <returns></returns>
    public void GetEmpiricalValue()
    {
        int finExp = 100;
        foreach(var speed in expCurve)
        {
            if(level >= speed.Item1)
            {
                finExp *= (int)Mathf.Pow(speed.Item2, level - speed.Item1);
            }
        }

        expNeed = finExp;
    }


    private void OnDisable()
    {
        if (EventController.Instance != null)
        {
            EventController.OnAchieveUp -= SetAchieveCount;
        }
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