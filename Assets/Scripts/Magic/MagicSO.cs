using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Magic So", menuName = "Magic/MagicSO")]
public class MagicSO : ScriptableObject
{
    public string ID;
    public Sprite icon;
    public string[] laterCondition;
    public int upNeed;
    [TextArea] public string description;

    public List<MagicEffect> magicEffectSos;
}

[System.Serializable]
public struct MagicEffect
{
    public float value;
    public MagicEffectSO MagicEffectSo;
}