using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Plant : MonoBehaviour
{
    ItemDictionary itemDictionary;

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
        itemDictionary = FindObjectOfType<ItemDictionary>();
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
        GameObject item = Instantiate(itemDictionary.GetItemPrefab(StatsManager.ItemTypes.Crop, ID, itemQuality), items.transform);
        item.transform.position = transform.position;
        item.GetComponent<Item>().Harvest(jumpTime);
        Destroy(gameObject);

    }


}
