using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Percent So", menuName = "Magic/LevelMagic")]
public class LevelMagicSO : MagicEffectSO
{
    public StatsManager.LevelMagics type;

    public override void MagicAssert()
    {
        throw new System.NotImplementedException();
    }
}
