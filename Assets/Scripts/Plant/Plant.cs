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

    ItemSaveController items;


    private void Awake()
    {
        growth = 0;
        currentGrowTime = 0;
        currentWaterTime = 0;

        water = false;
    }

    private void Start()
    {
        items = FindObjectOfType<ItemSaveController>();
    }

    public void PlantGrowth()
    {
        if (growth < plantSprite.Count)
        {
            GetComponent<SpriteRenderer>().sprite = plantSprite[growth];
        }
        else return;
        
    }

    private void Update()
    {
        //天黑后停止生长；
        if (TimeController.Instance.isNight)
        {
            return;
        }

        if (mature)
        {
            return;
        }


        if (growth + 1 < plantSprite.Count)
        {
            if (currentGrowTime < growTime)
            {
                currentGrowTime += Time.deltaTime;
            }
            else
            {
                currentGrowTime = 0;
                growth += 1;
                PlantGrowth();
                
            }

            if (water)
            {
                if (currentWaterTime < waterTime)
                {
                    currentWaterTime += Time.deltaTime;
                }
                else
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
        currentWaterTime = 0;
        currentGrowTime += growTime * 0.5f;
    }

    public void HarvestCrop()
    {
        int itemQuality = Random.Range(1, StatsManager.Instance.maxQuality + 1);
        GameObject item = Instantiate(ItemDictionary.Instance.GetItemPrefab(StatsManager.ItemTypes.Crop, ID, itemQuality), items.transform);
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
        if (growth <= 0)
        {
            GetComponent<SpriteRenderer>().sprite = plantSO.plantSprite[0];
        }
        else GetComponent<SpriteRenderer>().sprite = plantSO.plantSprite[growth];
        
        growTime = plantSO.growTime;
        waterTime = plantSO.waterTime;

    }

}
