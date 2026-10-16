using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Percent So", menuName = "Magic/LevelMagic")]
public class LevelMagicSO : MagicEffectSO
{
    public MagicTypes type;

    public override void MagicAssert(float value)
    {
        StatsManager.Instance.LevelStatUp(type, value);
    }
}
