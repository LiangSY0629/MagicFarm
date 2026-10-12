using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Magic So", menuName = "Magic/MagicSO")]
public class MagicSO : ScriptableObject
{
    public string ID;
    public Sprite icon;
    public int upNeed;
    public int maxLevel;
    [TextArea] public string description;
    public string[] precondition;
    public List<MagicEffect> magicEffectSos;
}

[System.Serializable]
public struct MagicEffect
{
    public float value;
    public MagicEffectSO MagicEffectSo;
}