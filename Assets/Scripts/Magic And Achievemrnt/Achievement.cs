using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Achievement : MonoBehaviour
{
    public int ID;
    public TMP_Text achieveName;
    public Image slotIcon;
    public Image achieveIcon;
    public StatsManager.AchieveCondition condition;
    public int conditionNumber;
    public bool isLock = true;


    /// <summary>
    /// 传入SO资源来为achieve赋值；
    /// </summary>
    /// <param name="achieveSO"></param>
    public void SetAchievement(AchievementSO achieveSO)
    {
        ID = achieveSO.ID;
        achieveName.text = achieveSO.achieveName;
        achieveIcon.sprite = achieveSO.icon;
        condition = achieveSO.condition;
        conditionNumber = achieveSO.conditionNumber;
        isLock = true;

    }

}
