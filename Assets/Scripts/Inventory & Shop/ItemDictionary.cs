using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    public List<Item> itemPrefabs;
    Dictionary<int, GameObject> itemDictionary;
    public Dictionary<int, int> itemNumDictionary;

    private void Awake()
    {
        itemDictionary = new Dictionary<int, GameObject>();
        itemNumDictionary = new Dictionary<int, int>();

        for (int i = 0; i < itemPrefabs.Count; i++)
        {

            if (itemPrefabs[i] != null)
            {
                itemPrefabs[i].ID = i + 1;
            }
        }

        foreach(Item item in itemPrefabs)
        {
            itemDictionary[item.ID] = item.gameObject;
            itemNumDictionary[item.ID] = 0;
        }

       
    }

    /// <summary>
    /// 根据ID找到对应的掉落物预制体；
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public GameObject GetItemPrefabs(int itemID)
    {
        itemDictionary.TryGetValue(itemID, out GameObject Prefabs);
        if (Prefabs == null)
        {
            Debug.Log("404 no found");
        }
        return Prefabs;
    }

}
