using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item")]
public class ItemSO: ScriptableObject
{
    public int ID;
    public string itemName;
    public Sprite icon;
    public int itemQuality;
    public int itemLevel;

    [TextArea] public string itemDescription;

}


[CreateAssetMenu(fileName ="New ItemLibrary")]
public class ItemLibrarySO: ScriptableObject
{
    public List<ItemSO> plantItems;
    public List<ItemSO> animalItems;

}
