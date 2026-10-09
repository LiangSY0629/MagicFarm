using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public static EventController instance { get; private set; }

    public static event Action<StatsManager.AchieveCondition, int> OnAchieveConditionComplete;

    public static event Action<int, int> OnAchieveComplete;

    /// <summary>
    /// 传入成就Condition，数量；
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="number"></param>
    public static void TriggerComplete(StatsManager.AchieveCondition condition, int number)
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

}
