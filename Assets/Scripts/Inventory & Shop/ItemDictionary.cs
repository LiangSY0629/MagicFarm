using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    public static ItemDictionary Instance { get; set; }

    public int itemMaxQuality = 3;
    public GameObject itemPublicPrefab;
    public ItemLibrarySO itemlibrarySO;
    //掉落物的种类和ID，掉落物信息；
    public Dictionary<(StatsManager.ItemTypes, int), ItemSO> itemDictionary;
    //掉落物种类，ID，品质 查询当前库存；
    public Dictionary<(StatsManager.ItemTypes, int, int), int> itemNumberDictionary;
    //掉落物种类，掉落物等级和当前等级的掉落物总数；
    public Dictionary<StatsManager.ItemTypes, Dictionary<int, List<ItemSO>>> itemLevelDictionary;
    public Dictionary<int, int> itemNumDictionary;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);

        //初始化字典
        itemDictionary = new Dictionary<(StatsManager.ItemTypes, int), ItemSO>();
        itemNumberDictionary = new Dictionary<(StatsManager.ItemTypes, int, int), int>();
        itemLevelDictionary = new Dictionary<StatsManager.ItemTypes, Dictionary<int, List<ItemSO>>>();

        //遍历队列，按顺序赋值List里的元素，并将元素添加到字典中
        for (int i = 0; i < itemlibrarySO.plantItems.Count; i++)
        {
            if (itemlibrarySO.plantItems[i] != null)
            {
                itemlibrarySO.plantItems[i].ID = i + 1;
                itemDictionary[(StatsManager.ItemTypes.Crop, i + 1)] = itemlibrarySO.plantItems[i];

            }
        }

        //同上；
        for (int i = 0; i < itemlibrarySO.animalItems.Count; i++)
        {
            if (itemlibrarySO.animalItems[i] != null)
            {
                itemlibrarySO.animalItems[i].ID = i + 1;
                itemDictionary[(StatsManager.ItemTypes.Animal, i + 1)] = itemlibrarySO.animalItems[i];

            }
        }

        //使用LINQ指令，将原有List里的元素按照Level分组，再根据每组的个数添加为字典；
        itemLevelDictionary[StatsManager.ItemTypes.Crop] = itemlibrarySO.plantItems.GroupBy(x => x.itemLevel).ToDictionary(y => y.Key, y => y.ToList());
        itemLevelDictionary[StatsManager.ItemTypes.Animal] = itemlibrarySO.animalItems.GroupBy(x => x.itemLevel).ToDictionary(y => y.Key, y => y.ToList());

    }

    /// <summary>
    /// 传入种类，ID，品质，找到对应的ItemSO，并将ItemSO的数据传入公共预制体，返回公共预制体；
    /// </summary>
    /// <param name="itemType"></param>
    /// <param name="itemID"></param>
    /// <param name="itemQuality"></param>
    /// <returns></returns>
    public GameObject GetItemPrefab(StatsManager.ItemTypes itemType, int itemID, int itemQuality)
    {
        itemDictionary.TryGetValue((itemType, itemID), out ItemSO itemSO);
        if (itemSO == null)
        {
            return null;
        }
        GameObject itemPrefab = itemPublicPrefab;
        Item item = itemPrefab.GetComponent<Item>();
        item.CreateItem(itemType, itemSO, itemQuality);

        return itemPrefab;
    }

    //public List<Item> itemPrefabs;
    //Dictionary<int, GameObject> itemDictionary;
    //public Dictionary<int, int> itemNumDictionary;

    //private void Awake()
    //{
    //    itemDictionary = new Dictionary<int, GameObject>();
    //    itemNumDictionary = new Dictionary<int, int>();

    //    for (int i = 0; i < itemPrefabs.Count; i++)
    //    {

    //        if (itemPrefabs[i] != null)
    //        {
    //            itemPrefabs[i].ID = i + 1;
    //        }
    //    }

    //    foreach(Item item in itemPrefabs)
    //    {
    //        itemDictionary[item.ID] = item.gameObject;
    //        itemNumDictionary[item.ID] = 0;
    //    }


    //}

    /// <summary>
    /// 根据ID找到对应的掉落物预制体；
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
    //public GameObject GetItemPrefabs(int itemID)
    //{
    //    GameObject Prefabs = null;
    //    itemDictionary.TryGetValue(itemID, out GameObject Prefabs);
    //    if (Prefabs == null)
    //    {
    //        Debug.Log("404 no found");
    //    }
    //    return Prefabs;
    //}

}
