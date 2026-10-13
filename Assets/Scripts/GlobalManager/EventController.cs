using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{

    //achieve相关的事件
    public static event Action<AchieveCondition, int> OnAchieveConditionComplete;
    public static event Action<int, int> OnAchieveComplete;
    public static event Action<int> OnAchieveUp;
    /// <summary>
    /// 传入成就Condition，数量；
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="number"></param>
    public static void TriggerComplete(AchieveCondition condition, int number)
    {
        OnAchieveConditionComplete?.Invoke(condition, number);
    }

    /// <summary>
    /// 传入需要解锁的成就的ID
    /// </summary>
    /// <param name="ID"></param>
    public static void TriggerComplete(int ID, int number)
    {
        OnAchieveComplete?.Invoke(ID, number);
    }

    /// <summary>
    /// 调用一次就会添加一次成就数量；
    /// </summary>
    public static void TriggerAchieveUp()
    {
        OnAchieveUp?.Invoke(1);
    }

    //Magic相关事件;
    public static event Action<string> OnLevelUp;
    
    /// <summary>
    /// 传入需要更改信息的ID；
    /// </summary>
    /// <param name="ID"></param>
    public static void TriggerLevelUp(string ID)
    {
        OnLevelUp?.Invoke(ID);
    }

}
