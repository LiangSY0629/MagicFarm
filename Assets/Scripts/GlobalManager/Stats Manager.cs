using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance { get;set;}


    [Header("Combat Stats")]
    public int damage;
    public Tools current_tool = Tools.None;

    [Header("Player Stats")]
    public float speed;//角色速度；
    public int square = 1;//攻击范围；

    [Header("Item Stats")]
    public int maxPickNumber = 1;//最大可拾取数量
    public int maxPileNumber = 99;//最大可堆叠数量；
    public int maxQuality = 1;//最大可获得品质；

    [Header("Animals Stats")]
    public int currentAnimalTypes;//当前最高动物等级；
    public int animalNumber;//动物种类数量；
    public int maxAnimalTypes;//动物品种总数；

    [Header("Plant Stats")]
    public int maxSeed;//种子总数；
    public int seedLevel;//当前最高种子等级；
    public int EveryLevelSeed;//每个等级的种子数量；


    [Header("Mail Stats")]
    public int maxMail;//最大可容纳订单数；
    public int orderLevel;//订单等级；
    public float rewardLevel;//奖励等级；
    public float orderInterval;//订单冷却；
    public int goldCount;//金币总量；


    [Header("Achieve And Magic Stats")]
    public int achieveCompleteCount;//当前解锁的成就数量；

    [Header("绑定个人信息面板")]
    public TMP_Text achieveText;
    public TMP_Text glodText, timeText;

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

    /// <summary>
    /// 成就解锁的量化条件；
    /// </summary>
    public enum AchieveCondition
    {
        None,
        Harvest,
        Feed,
        Order,
        Glod,
        StarItem,
        Skill,
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

    private void Start()
    {
        EventController.OnAchieveUp += AchieveCountUp;
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

    /// <summary>
    /// 调用函数获取需要存档的变量信息；
    /// </summary>
    /// <returns></returns>
    public StatsSaveData GetSaveData()
    {
        StatsSaveData statsData = new StatsSaveData();

        statsData.currentGlodCount = goldCount;
        statsData.square = square;
        statsData.achieveCompleteCount = achieveCompleteCount;

        return statsData;
    }

    /// <summary>
    /// 传入存档的变量为当前变量赋值；
    /// </summary>
    /// <param name="statsSaveData"></param>
    public void SetSaveData(StatsSaveData statsSaveData)
    {
        goldCount = statsSaveData.currentGlodCount;
        square = statsSaveData.square;
        achieveCompleteCount = statsSaveData.achieveCompleteCount;
        achieveText.text = "获得成就数：" + achieveCompleteCount;

    }

}

