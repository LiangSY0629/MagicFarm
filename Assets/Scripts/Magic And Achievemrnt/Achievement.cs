using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Achievement : MonoBehaviour
{
    [Header("公共需求")]
    public int ID;
    public TMP_Text achieveText;
    public Image slotIcon;
    public Image achieveIcon;
    public int conditionNumber;
    public int currentValue;
    public bool isLock = true;

    [Header("仅有等级成就所需")]
    public StatsManager.AchieveCondition condition;

    /// <summary>
    /// 传入SO资源来为achieve赋值；
    /// </summary>
    /// <param name="achieveSO"></param>
    public void SetAchievement(AchievementSO achieveSO)
    {
        ID = achieveSO.ID;
        achieveText.text = achieveSO.achieveName;
        achieveIcon.sprite = achieveSO.icon;
        if (achieveSO.conditionNumber > 0)
        {
            conditionNumber = achieveSO.conditionNumber;
        }
        else conditionNumber = 1;
        currentValue = 0;
        isLock = true;
    }

    /// <summary>
    /// 传入SO资源和索引值来为achieve赋值；
    /// </summary>
    ///<param name="GroupSO"></param>
    ///<param name="i"></param>
    public void SetAchievement(AchieveGroupSO groupSO, int i)
    {
        ID = i;
        achieveText.text = groupSO.achievements[i].achieveName;
        achieveIcon.sprite = groupSO.icon;
        condition = groupSO.condition;

        //如果有规律，所需数量 = 基础值 * （乘数 * ID）；
        if (groupSO.isRegular)
        {
            conditionNumber = groupSO.conditionValue * (groupSO.mutiplier * i);
        }
        //无规律用内置数组赋值；
        else
        {
            conditionNumber = groupSO.conditionNumbers[i];
        }

        currentValue = 0;
        isLock = true;

    }

}
