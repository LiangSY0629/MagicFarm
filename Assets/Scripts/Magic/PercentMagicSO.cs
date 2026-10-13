using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Percent So", menuName = "Magic/PercentMagic")]
public class PercentMagicSO : MagicEffectSO
{
    public StatsManager.PercentMagics type;

    public override void MagicAssert()
    {
        throw new System.NotImplementedException();
    }
}
