using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[System.Serializable]
public class SaveData
{

    public Vector3 player_position;
    public int square;

    public float currentTime;
    public float currentOrderInterval;

    public List<InventorySaveData> inventorySaveData;
    public List<AllItemNumberSaveData> allItemNumberSaveData;

    public List<ItemSaveData> itemSaveData;

    public List<PlantSaveData> plantSaveData;

    public List<TileSaveData> tileSaveData;

    public List<AnimalSaveData> animalSaveData;

    public List<OrderSaveData> orderSaveData;

}

[System.Serializable]
public class InventorySaveData
{
    public StatsManager.ItemTypes itemType;
    public int itemID;
    public int itemQuality;
    public int slotIndex;
    public int itemNum;

}

[System.Serializable]
public class AllItemNumberSaveData
{
    public StatsManager.ItemTypes itemType;
    public int itemID;
    public int itemQuality;
    public int allItemNumber;
}


[System.Serializable]
public class ItemSaveData
{
    public StatsManager.ItemTypes itemType;
    public int itemID;
    public int itemQuality;
    public Vector3 itemPosition;

}



[System.Serializable]
public class PlantSaveData
{
    public bool isPlant;
    public int plantID;
    public Vector3Int plantPosition;
    public int growth;
    public float growthTime;
    public bool water;
    public float waterTime;
    public bool mature;

}

[System.Serializable]
public class TileSaveData
{
    public Vector3Int tilePosition;
    public bool tilePlant;
}


[System.Serializable]
public class AnimalSaveData
{
    public int animalID;
    public int animalType;
    public bool eating;
    public float eatingTime;
    public int finalProduct;

}

[System.Serializable]
public class OrderSaveData
{
    public int itemID;
    public string titleText, goldNumber;

    public int starNumber;
    public int itemNeedNumber;
    public int rewardNumber;
}
