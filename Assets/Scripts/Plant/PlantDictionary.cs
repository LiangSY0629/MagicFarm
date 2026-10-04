using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlantDictionary : MonoBehaviour
{
    public List<Plant> plantPrefabs;
    Dictionary<int, GameObject> plantDictionary;


    private void Awake()
    {
        plantDictionary = new Dictionary<int, GameObject>();

        for (int i = 0; i < plantPrefabs.Count;i++)
        {
            if (plantPrefabs[i] != null)
            {
                plantPrefabs[i].GetComponent<Plant>().ID = i + 1;
            }
        }

        foreach(Plant plant in plantPrefabs)
        {
            plantDictionary[plant.ID] = plant.gameObject;
        }

    }


    public GameObject SetPlantPrefabs(int plantID)
    {
        plantDictionary.TryGetValue(plantID, out GameObject plantPrefab);
        if (plantPrefab == null)
        {
            return null;
        }
        return plantPrefab;
    }

}
