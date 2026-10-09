using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Achieve Group SO", menuName = "Achievement Group")] 
public class AchieveGroupSO : ScriptableObject
{
    public StatsManager.AchieveCondition condition;

    public Sprite icon;
    public List<int> conditionNumbers;
    public bool isRegular;
    public int conditionValue;
    public int mutiplier;

    public List<AchievementGroup> achievements;

}

[System.Serializable]
public class AchievementGroup
{
    public int ID;
    public string achieveName;
    [TextArea] public string description;
}