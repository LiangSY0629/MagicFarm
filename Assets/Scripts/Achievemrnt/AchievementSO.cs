using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Achievement", menuName = "Achievement")]
public class AchievementSO : ScriptableObject
{
    public int ID;
    public string achieveName;
    public Sprite icon;
    public Sprite completeIcon;
    public int conditionNumber;
    [TextArea] public string achieveDescription;

}
