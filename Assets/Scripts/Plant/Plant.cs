using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Plant : MonoBehaviour
{

    [Header("设置基本信息")]
    public List<Sprite> plantSprite;
    public int ID;
    //public string Name;
    public int growth = 0;
    public GameObject waterSprite;

    [Header("不要改这个")]
    public float currentGrowTime = 0;
    public float currentWaterTime = 0;
    public bool water;
    public bool mature = false;

    [Header("设置成长时间")]
    public float growTime = 30;
    public float waterTime = 5;
    public float jumpTime = 0.5f;


    private void Awake()
    {
        growth = 0;
        water = false;
    }

    public void PlantGrowth()
    {
        if (growth < plantSprite.Count)
        {
            GetComponent<SpriteRenderer>().sprite = plantSprite[growth];
        }
        else return;
        
    }


    /// <summary>
    /// 传入plant生长的时间，更新图像以及浇水等；
    /// </summary>
    /// <param name="upTime"></param>
    public void UpdatePlant(float upTime)
    {
        if(growth +1 < plantSprite.Count)
        {
            currentGrowTime += upTime;

            int currentGrowth = 0;
            currentGrowth = (int)(currentGrowTime / (growTime * StatsManager.Instance.plantGrowth.finalValue));

            if(currentGrowth != growth)
            {
                growth = currentGrowth;
                PlantGrowth();
            }

            if (water)
            {
                if (currentWaterTime > 0)
                {
                    currentWaterTime -= upTime;
                }
                
                if(currentWaterTime <= 0)
                {
                    water = false;
                    waterSprite.SetActive(!water);

                }
            }
        }
        else
        {
            mature = true;
            water = true;
            waterSprite.SetActive(!water);
        }


    }


    /// <summary>
    /// 浇水时触发，重置浇水时间，并增加植物生长进度；
    /// </summary>
    public void IsWatering()
    {
        water = true;
        waterSprite.SetActive(!water);
        currentWaterTime = waterTime * StatsManager.Instance.plantWater.finalValue;
        currentGrowTime += growTime * StatsManager.Instance.plantGrowth.finalValue * 0.5f;
    }

    public void HarvestCrop()
    {
        int value = Mathf.RoundToInt(StatsManager.Instance.maxQuality.finalValue);
        int itemQuality = Random.Range(1, value + 1);
        GameObject item = Instantiate(ItemDictionary.Instance.GetItemPrefab(ItemTypes.Crop, ID, itemQuality), ItemSaveController.Instance.transform);
        item.transform.position = transform.position;
        item.GetComponent<Item>().Harvest(jumpTime);
        Destroy(gameObject);

    }

    /// <summary>
    /// 传入plant SO调用该函数为这个脚本赋值；
    /// </summary>
    /// <param name="plantSO"></param>
    public void SetPlantSO(PlantSO plantSO)
    {
        ID = plantSO.ID;
        plantSprite = plantSO.plantSprite;
        currentGrowTime = 0;
        currentWaterTime = 0;
        growth = 0;
        mature = false;

        if (growth <= 0)
        {
            GetComponent<SpriteRenderer>().sprite = plantSO.plantSprite[0];
        }
        else GetComponent<SpriteRenderer>().sprite = plantSO.plantSprite[growth];

        growTime = plantSO.growTime;
        waterTime = plantSO.waterTime;

    }

}
