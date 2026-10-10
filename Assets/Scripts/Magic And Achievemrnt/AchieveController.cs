using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static AchievementSaveData;

public class AchieveController : MonoBehaviour
{
    public static AchieveController Instance;

    public GameObject achievePanel;
    public GameObject achievePrefab;//公共预制体
    public Sprite[] achieveSlotSprite;//成就框图像；
    public AchievePopController achievePopup;

    [Header("置入SO信息")]
    public AchieveGroupSO[] achieveGroupSOs;
    public AchievementSO[] achieveSOs;

    public Dictionary<StatsManager.AchieveCondition, List<Achievement>> achieveGroupDictionary; //有等级的成就字典
    public Dictionary<int, Achievement> achieveDictionary; //无等级的成就字典

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {

        achieveGroupDictionary = new Dictionary<StatsManager.AchieveCondition, List<Achievement>>();
        achieveDictionary = new Dictionary<int, Achievement>();

        EventController.OnAchieveComplete += CompleteAchievement;
        EventController.OnAchieveConditionComplete += CompleteAchievement;

        //遍历有等级成就，生成对应的achieveSlot；
        foreach (AchieveGroupSO achieveGroup in achieveGroupSOs)
        {
            List<Achievement> achievements = new List<Achievement>();

            for(int i = 0; i < achieveGroup.achievements.Count; i++)
            {
                achieveGroup.achievements[i].ID = i;

                Achievement achieve = Instantiate(achievePrefab, achievePanel.transform).GetComponent<Achievement>();
                achieve.SetAchievement(achieveGroup, i);
                achieve.slotIcon.sprite = achieveSlotSprite[0];

                achievements.Add(achieve);
            }
            achieveGroupDictionary[achieveGroup.condition] = achievements;

        }

        //遍历无等级成就，生成对应的achieveSlot，赋值并加入字典；
        for (int i = 0; i < achieveSOs.Length; i++)
        {
            achieveSOs[i].ID = i + 1;

            Achievement achieve = Instantiate(achievePrefab, achievePanel.transform).GetComponent<Achievement>();
            achieve.SetAchievement(achieveSOs[i]);
            achieve.slotIcon.sprite = achieveSlotSprite[0];

            achieveDictionary[i + 1] = achieve;
        }


    }

    /// <summary>
    /// 完成具有condition的成就；
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="number"></param>
    void CompleteAchievement(StatsManager.AchieveCondition condition, int number)
    {
        foreach(Achievement achieve in achieveGroupDictionary[condition])
        {
            if (achieve.isLock)
            {
                achieve.currentValue += number;

                if (achieve.currentValue >= achieve.conditionNumber)
                {
                    achieve.currentValue = achieve.conditionNumber;
                    achieve.isLock = false;
                    achieve.achieveIcon.sprite = achieve.completeIcon;
                    achieve.slotIcon.sprite = achieveSlotSprite[1];
                    achievePopup.AchievePopup(achieve.achieveText.text, achieve.completeIcon);

                }
            }

        }

    }

    /// <summary>
    /// 完成不具有condition的成就；
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="number"></param>
    void CompleteAchievement(int ID, int number)
    {
        Achievement achieve = achieveDictionary[ID];

        if (achieve.isLock)
        {
            achieve.currentValue += number;

            if (achieve.currentValue >= achieve.conditionNumber)
            {
                achieve.currentValue = achieve.conditionNumber;
                achieve.isLock = false;
                achieve.achieveIcon.sprite = achieve.completeIcon;
                achieve.slotIcon.sprite = achieveSlotSprite[1];
                achievePopup.AchievePopup(achieve.achieveText.text, achieve.completeIcon);
            }

        }

    }

    /// <summary>
    /// 调用函数返回achievement的save data;
    /// </summary>
    /// <returns></returns>
    public List<AchievementSaveData> GetSaveData()
    {
        List<AchievementSaveData> achieveData = new List<AchievementSaveData>();

        //遍历有等级字典；
        foreach(var achieveGroup in achieveGroupDictionary)
        {
            foreach(Achievement achieve in achieveGroup.Value)
            {
                achieveData.Add(new AchievementSaveData
                {
                    condition = achieveGroup.Key,
                    ID = achieve.ID,
                    currentValue = achieve.currentValue,
                    isLock = achieve.isLock,
                });

            }

        }

        //遍历无等级字典；
        foreach(var achieve in achieveDictionary)
        {
            achieveData.Add(new AchievementSaveData
            {
                condition = StatsManager.AchieveCondition.None,
                ID = achieve.Key,
                currentValue = achieve.Value.currentValue,
                isLock = achieve.Value.isLock,
            });

        }

        return achieveData;
    }
    

    public void SetAchieveData(List<AchievementSaveData> achieveSaveData)
    {
        List<Achievement> achieveGroup = new List<Achievement>();

        //遍历整个List；
        foreach(AchievementSaveData  achieveData in achieveSaveData)
        {
            Achievement achieve;
            //先为有等级字典赋值；
            if (achieveData.condition != StatsManager.AchieveCondition.None)
            {
                achieveGroup = achieveGroupDictionary[achieveData.condition];
                achieve = achieveGroup[achieveData.ID];
            }
            //索引无等级字典；
            else
            {
                achieve = achieveDictionary[achieveData.ID];
            }

            achieve.currentValue = achieveData.currentValue;
            achieve.isLock = achieveData.isLock;

            if (achieveData.isLock)
            {
                achieve.slotIcon.sprite = achieveSlotSprite[0];
                achieve.achieveIcon.sprite = achieve.startIcon;
            }
            else
            {
                achieve.slotIcon.sprite = achieveSlotSprite[1];
                achieve.achieveIcon.sprite = achieve.completeIcon;
            }


        }

    }

}
