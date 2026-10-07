using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Plant")]
public class PlantSO : ScriptableObject
{
    public int ID;
    public float growTime;
    public float waterTime;
    public List<Sprite> plantSprite;

}
