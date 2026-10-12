using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemLibrary", menuName = "Item/ItemLibrarySO")]
public class ItemLibrarySO : ScriptableObject
{
    public List<ItemSO> plantItems;
    public List<ItemSO> animalItems;

}

