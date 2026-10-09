using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchieveController : MonoBehaviour
{
    public static AchieveController Instance;

    public GameObject achievePanel;
    public GameObject achievePrefab;//公共预制体
    public Sprite[] achieveSlotSprite;//成就框图像；
    public List<AchievementSO> achieveSOList;//按顺序存储所有的achievementSO
    public Dictionary<int, Achievement> achieveDictionary;//创建字典来查找并解锁achievement；

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        achieveDictionary = new Dictionary<int, Achievement>();

        //遍历整个achieve List，为ID赋值并生成achieveSlot；为achieve赋值并加入dictionary；
        for(int i = 0; i < achieveSOList.Count; i++)
        {
            achieveSOList[i].ID = i + 1;
            Achievement achieve = Instantiate(achievePrefab, achievePanel.transform).GetComponent<Achievement>();
            achieve.SetAchievement(achieveSOList[i]);
            achieve.slotIcon.sprite = achieveSlotSprite[0];

            achieveDictionary[i + 1] = achieve;
        }

    }



}
