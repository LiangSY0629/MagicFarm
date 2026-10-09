using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

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
                    achieve.slotIcon.sprite = achieveSlotSprite[1];
                    achievePopup.AchievePopup(achieve.achieveText.text);

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
            achieve.condition += number;

            if (achieve.currentValue >= achieve.conditionNumber)
            {
                achieve.currentValue = achieve.conditionNumber;
                achieve.isLock = false;
                achieve.slotIcon.sprite = achieveSlotSprite[1];
                achievePopup.AchievePopup(achieve.achieveText.text);
            }

        }

    }

}
