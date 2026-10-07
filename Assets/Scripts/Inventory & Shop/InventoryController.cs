using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;

    private void Start()
    {

        for (int i = 0; i < slotCount; i++)
        {
            Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
        }
    }

    /// <summary>
    /// 根据拾取的掉落物预制体，在背包中进行存储；
    /// </summary>
    /// <param name="itemPrefab"></param>
    /// <returns></returns>
    public bool AddItem(GameObject itemPrefab)
    {
        Item pickItem = itemPrefab.GetComponent<Item>();
        int itemID = pickItem.ID;
        int itemQuality = pickItem.quality;
        StatsManager.ItemTypes itemType = pickItem.Type;

        //随机拾取到的数量；
        pickItem.RandomPickNum();

        if(!ItemDictionary.Instance.itemNumberDictionary.TryGetValue((itemType, itemID, itemQuality), out int itemNumber))
        {
            ItemDictionary.Instance.itemNumberDictionary[(itemType, itemID, itemQuality)] = 0;
        }
        

        //遍历当前背包里所有的格子；
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot != null)
            {
                //如果当前格子已有物品；
                if (slot.currentItem != null)
                {
                    Item item = slot.currentItem.GetComponent<Item>();

                    //如果Type、ID和Quality相同
                    if (item.Type == itemType && item.ID == itemID && item.quality == itemQuality)
                    {
                        //如果加上以后不会超过最大堆叠；
                        if (item.number <= (StatsManager.Instance.maxPileNumber - pickItem.pickNumber))
                        {
                            //先让预制体的number加上pick Number；再变动库存字典，让该种类、该ID、该quality加上pick Number；
                            item.number += pickItem.pickNumber;
                            ItemDictionary.Instance.itemNumberDictionary[(item.Type, item.ID, item.quality)] += pickItem.pickNumber;

                            //调用函数更新数字显示；
                            item.AddNumber();
                            return true;
                        }
                        //超过最大堆叠；
                        else
                        {
                            //先让pick number减去添加过的值，再重新找到下一个格子重新开始判断；
                            pickItem.pickNumber -= 99 - item.number;
                            ItemDictionary.Instance.itemNumberDictionary[(item.Type, item.ID, item.quality)] += 99 - item.number;
                            item.number = 99;
                        }
                    }
                    
                }
                //当前格子没有物品；
                else
                {
                    GameObject newItemObject = Instantiate(itemPrefab, slot.transform);
                    slot.currentItem = newItemObject;
                    newItemObject.GetComponent<Item>().number = pickItem.pickNumber;
                    newItemObject.GetComponent<Item>().AddNumber();
                    ItemDictionary.Instance.itemNumberDictionary[(itemType, itemID, itemQuality)] += pickItem.pickNumber;
                    return true;
                }

            }
            
        }

        return false;
    }

    /// <summary>
    /// 根据掉落物的种类、ID、品质和数量来检索和减去对应数量的库存；
    /// </summary>
    /// <param name="itemType"></param>
    /// <param name="ID"></param>
    /// <param name="quality"></param>
    /// <param name="number"></param>
    /// <returns></returns>
    public bool LessItem(StatsManager.ItemTypes Type, int ID, int quality, int number)
    {
        //先看是否能找到对应的value；
        if (ItemDictionary.Instance.itemNumberDictionary.TryGetValue((Type, ID, quality), out int itemNumber))
        {
            //如果找到但不够减去，就返回；
            if (itemNumber < number)
            {
                return false;
            }
            //够减去就减去；
            else
            {

                if (itemNumber == number)
                {
                    ItemDictionary.Instance.itemNumberDictionary.Remove((Type, ID, quality));
                }
                else
                {
                    ItemDictionary.Instance.itemNumberDictionary[(Type, ID, quality)] -= number;
                }
                
            }
        }
        else return false;

        //遍历所有格子；
        foreach(Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();

            //如果当前不是空格子，
            if (slot != null && slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();

                //如果种类、ID、quality全部相同；
                if (item.Type == Type && item.ID == ID && item.quality == quality)
                {
                    //如果数量足够被减去；
                    if (item.number > number)
                    {
                        item.number -= number;
                        item.AddNumber();
                        return true;
                    }
                    //如果刚好够减去；
                    else if (item.number == number)
                    {
                        Destroy(item.gameObject);
                        slot.currentItem = null;
                        return true;
                    }
                    //如果不够减去；
                    else if (item.number < number)
                    {
                        number -= item.number;
                        Destroy(item.gameObject);
                        slot.currentItem = null;

                    }

                }

            }

        }

        return false;

    }

    /// <summary>
    /// 保存当前不为空的格子的信息；
    /// </summary>
    /// <returns></returns>
    public List<InventorySaveData> GetSaveSlot()
    {
        List<InventorySaveData> invData = new List<InventorySaveData>();

        //遍历所有的格子；
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                //slot Index是格子索引，即当前item占用第几个格子；
                invData.Add(new InventorySaveData {
                    itemType = item.Type,
                    itemID = item.ID,
                    itemQuality = item.quality,
                    slotIndex = slotTransform.GetSiblingIndex(), 
                    itemNum = item.number });
            }
        }
        return invData;
    }


    /// <summary>
    ///保存当前掉落物ID的所有数量； 
    /// </summary>
    /// <returns></returns>
    public List<AllItemNumberSaveData> GetItemNumber()
    {
        List<AllItemNumberSaveData> numData = new List<AllItemNumberSaveData>();

        foreach (var number in ItemDictionary.Instance.itemNumberDictionary)
        {
            numData.Add(new AllItemNumberSaveData { 
                itemType = number.Key.Item1,
                itemID = number.Key.Item2,
                itemQuality = number.Key.Item3, 
                allItemNumber = number.Value });
        }

        return numData;
    }



    /// <summary>
    /// 从List中加载格子的信息；
    /// </summary>
    /// <param name="inventorySaveData"></param>
    public void SetInventoryData(List<InventorySaveData> inventorySaveData)
    {
        List<Slot> slotList = new List<Slot>();

        foreach(Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < slotCount; i++)
        {
            Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
            slotList.Add(slot);
        }

        //遍历List，将List里的信息赋值出来；
        foreach (InventorySaveData inventoryData in inventorySaveData)
        {
            if (inventoryData.slotIndex < slotCount)
            {
                Slot slot = slotList[inventoryData.slotIndex];

                GameObject itemPrefab = ItemDictionary.Instance.GetItemPrefab(inventoryData.itemType,inventoryData.itemID, inventoryData.itemQuality);

                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<Item>().number = inventoryData.itemNum;
                    item.GetComponent<Item>().AddNumber();
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    slot.currentItem = item;
                }
            }
        }

    }

    /// <summary>
    /// 加载队列中的所有数值，为字典赋值，更改库存；
    /// </summary>
    /// <param name="itemNumberSaveData"></param>
    public void SetItemNumber(List<AllItemNumberSaveData> itemNumberSaveData)
    {
        ItemDictionary.Instance.itemNumberDictionary.Clear();


        foreach(AllItemNumberSaveData itemNumber in itemNumberSaveData)
        {
            ItemDictionary.Instance.itemNumberDictionary[(itemNumber.itemType, itemNumber.itemID, itemNumber.itemQuality)] = itemNumber.allItemNumber;
        }

    }


}
