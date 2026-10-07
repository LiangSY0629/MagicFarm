using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item")]
public class ItemSO: ScriptableObject
{
    public int ID;
    public string itemName;
    public Sprite icon;
    public int itemLevel;

    [TextArea] public string itemDescription;

}