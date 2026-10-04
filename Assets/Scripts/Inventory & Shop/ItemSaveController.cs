using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSaveController : MonoBehaviour
{
    //保存当前场景下的掉落物；
    ItemDictionary itemDictionary;
    private void Start()
    {
        itemDictionary = transform.GetComponent<ItemDictionary>();
    }


    public List<ItemSaveData> GetItemData()
    {
        List<ItemSaveData> itemSaveData = new List<ItemSaveData>();

        foreach(Transform itemTransform in transform)
        {
            Item item = itemTransform.GetComponent<Item>();
            itemSaveData.Add(new ItemSaveData { itemID = item.ID, itemPosition = itemTransform.position });
        }

        return itemSaveData;
    }

    //加载保存的掉落物到当前场景；
    public void SetItemDate(List<ItemSaveData> itemSaveData)
    {
        if (itemSaveData == null)
        {
            return;
        }
        else
        {
            foreach(Transform itemTransform in transform)
            {
                Destroy(itemTransform.gameObject);
            }


            foreach(ItemSaveData itemData in itemSaveData)
            {
                GameObject itemPrefab = itemDictionary.GetItemPrefabs(itemData.itemID);
                if (itemPrefab != null)
                {
                    GameObject newItem = Instantiate(itemPrefab, transform);
                    newItem.transform.position = itemData.itemPosition;
                }

            }
        }
    }
}
