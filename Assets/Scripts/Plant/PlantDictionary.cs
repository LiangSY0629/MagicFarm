using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlantDictionary : MonoBehaviour
{
    public GameObject plantPrefab;
    public List<PlantSO> plantLibrary;
    public Dictionary<int, PlantSO> plantDictionary;

    private void Awake()
    {
        plantDictionary = new Dictionary<int, PlantSO>();

        for(int i = 0; i < plantLibrary.Count; i++)
        {
            plantLibrary[i].ID = i + 1;
        }

        foreach(PlantSO plant in plantLibrary)
        {
            plantDictionary[plant.ID] = plant;
        }
    }

    /// <summary>
    /// 传入ID获取plant SO，并根据Plant SO对公共植物预制体进行赋值；
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public GameObject SetPlantPrefab(int ID)
    {
        plantDictionary.TryGetValue(ID, out PlantSO plantSO);

        GameObject plant = plantPrefab;
        if (plantSO != null)
        {
            plant.GetComponent<Plant>().SetPlantSO(plantSO);
        }
        else
        {
            return null;
        }

        return plantPrefab;
    }

    //public GameObject SetPlantPrefabs(int plantID)
    //{
    //    plantDictionary.TryGetValue(plantID, out GameObject plantPrefab);
    //    if (plantPrefab == null)
    //    {
    //        return null;
    //    }
    //    return plantPrefab;
    //}

}
