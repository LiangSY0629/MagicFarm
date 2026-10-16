using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Percent So", menuName = "Magic/PercentMagic")]
public class PercentMagicSO : MagicEffectSO
{
    public MagicTypes type;

    public override void MagicAssert(float value)
    {
        StatsManager.Instance.PercentStatUp(type, value);
    }
}
