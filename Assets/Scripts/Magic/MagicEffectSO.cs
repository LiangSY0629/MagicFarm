using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MagicEffectSO : ScriptableObject,IMagicAssert
{
    public abstract void MagicAssert(float value);
}
