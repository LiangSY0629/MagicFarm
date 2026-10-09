using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Achievement")]
public class AchievementSO : ScriptableObject
{
    public int ID;
    public string achieveName;
    public Sprite icon;
    public StatsManager.AchieveCondition condition;
    public int conditionNumber;
    [TextArea] public string achieveDescription;

}
