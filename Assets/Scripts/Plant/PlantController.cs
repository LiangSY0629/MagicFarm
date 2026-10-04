using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlantController : MonoBehaviour
{

    PlantDictionary plantDictionary;

    private void Start()
    {
        plantDictionary = FindObjectOfType<PlantDictionary>();
    }

    public List<PlantSaveData> GetSaveData()
    {
        List<PlantSaveData> plantData = new List<PlantSaveData>();

        foreach (var plantdic in TileMapManager.Instance.tilePlantDictionary)
        {
            Plant plant = plantdic.Value;
            

            plantData.Add(new PlantSaveData
            {
                plantID = plant.ID,
                plantPosition = plantdic.Key,
                growth = plant.growth,
                growthTime = plant.currentGrowTime,
                water = plant.water,
                waterTime = plant.currentWaterTime,
                mature = plant.mature,

            });
        }


        return plantData;

    }


    public void SetPlant(List<PlantSaveData> plantSaveData)
    {
        if (plantSaveData == null)
        {
            return;
        }

        foreach (Transform transform in transform)
        {
            Destroy(transform.gameObject);
        }

        StartCoroutine(SetPlantObejct(plantSaveData));
    }

    IEnumerator SetPlantObejct(List<PlantSaveData> plantSaveData)
    {
        yield return null;

        TileMapManager.Instance.tilePlantDictionary = new Dictionary<Vector3Int, Plant>();

        foreach (PlantSaveData plantData in plantSaveData)
        {
            GameObject plantPrefab = plantDictionary.SetPlantPrefabs(plantData.plantID);

            if (plantPrefab != null)
            {
                GameObject newplant = Instantiate(plantPrefab, transform);
                newplant.transform.position = plantData.plantPosition + new Vector3(0.5f,0.5f,0);
                Plant plant = newplant.GetComponent<Plant>();
                plant.growth = plantData.growth;
                plant.currentGrowTime = plantData.growthTime;
                plant.water = plantData.water;
                plant.currentWaterTime = plantData.waterTime;
                plant.mature = plantData.mature;
                plant.PlantGrowth();
                plant.waterSprite.SetActive(!plant.water);
                TileMapManager.Instance.tilePlantDictionary[plantData.plantPosition] = plant;

            }

        }

    }

}
