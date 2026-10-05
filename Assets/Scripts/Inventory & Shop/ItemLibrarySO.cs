using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemLibrary")]
public class ItemLibrarySO : ScriptableObject
{
    public List<ItemSO> plantItems;
    public List<ItemSO> animalItems;

}

