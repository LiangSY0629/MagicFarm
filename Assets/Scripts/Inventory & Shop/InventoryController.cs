using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryController : MonoBehaviour
{
    ItemDictionary itemDictionary;

    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;

    private void Start()
    {
        itemDictionary = FindObjectOfType<ItemDictionary>();

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
        Item newItem = itemPrefab.GetComponent<Item>();
        int itemID = newItem.ID;
        newItem.RandomPickNum();

        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            

            if (slot != null)
            {
                
                if (slot.currentItem != null)
                {
                    Item item = slot.currentItem.GetComponent<Item>();
                    if (item.ID == itemID && item.number <= (99 - newItem.pickNumber))
                    {
                        item.number += newItem.pickNumber;
                        itemDictionary.itemNumDictionary[itemID] += newItem.pickNumber;
                        item.AddNumber();
                        return true;
                    }
                    else if (item.ID == itemID && item.number > (99 - newItem.pickNumber))
                    {
                        newItem.pickNumber -= 99 - item.number;
                        itemDictionary.itemNumDictionary[itemID] += 99 - item.number;
                        item.number = 99;
                    }
                }
                else
                {
                    GameObject newItemObject = Instantiate(itemPrefab, slot.transform);
                    slot.currentItem = newItemObject;
                    newItemObject.GetComponent<Item>().number = newItem.pickNumber;
                    newItemObject.GetComponent<Item>().AddNumber();
                    itemDictionary.itemNumDictionary[itemID] += newItem.pickNumber;
                    return true;
                }

            }
            
        }

        return false;
    }

    /// <summary>
    /// 根据掉落物的ID和数量来检索和减去对应数量的库存；
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="number"></param>
    public bool LessItem(int ID ,int number)
    {
        if (itemDictionary.itemNumDictionary[ID] < number)
        {
            return false;
        }

        itemDictionary.itemNumDictionary[ID] -= number;

        foreach(Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot != null && slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();

                if (item.ID == ID)
                {
                    if (item.number > number)
                    {
                        item.number -= number;
                        item.AddNumber();
                        return true;
                    }
                    else if (item.number == number)
                    {
                        Destroy(item.gameObject);
                        slot.currentItem = null;
                        return true;
                    }
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


    public List<InventorySaveData> GetSaveSlot()
    {
        List<InventorySaveData> invData = new List<InventorySaveData>();

        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                invData.Add(new InventorySaveData { itemID = item.ID, slotIndex = slotTransform.GetSiblingIndex(), itemNum = item.number });
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

        foreach (var number in itemDictionary.itemNumDictionary)
        {
            numData.Add(new AllItemNumberSaveData { itemID = number.Key,allItemNumber = number.Value });
        }

        return numData;
    }



    //List从中加载itemID，slotID
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

        foreach (InventorySaveData inventoryData in inventorySaveData)
        {
            if (inventoryData.slotIndex < slotCount)
            {
                Slot slot = slotList[inventoryData.slotIndex];

                //GameObject itemPrefab = itemDictionary.GetItemPrefabs(inventoryData.itemID);
                //if (itemPrefab != null)
                //{
                //    GameObject item = Instantiate(itemPrefab, slot.transform);
                //    item.GetComponent<Item>().number = inventoryData.itemNum;
                //    item.GetComponent<Item>().AddNumber();
                //    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                //    slot.currentItem = item;
                //}
            }
        }

    }

    /// <summary>
    /// 加载队列中的所有数值，为字典赋值，更改库存；
    /// </summary>
    /// <param name="itemNumberSaveData"></param>
    public void SetItemNumber(List<AllItemNumberSaveData> itemNumberSaveData)
    {
        itemDictionary.itemNumDictionary.Clear();


        foreach(AllItemNumberSaveData itemNumber in itemNumberSaveData)
        {
            itemDictionary.itemNumDictionary[itemNumber.itemID] = itemNumber.allItemNumber;
        }

    }


}
