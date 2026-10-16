using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


[System.Serializable]
public class SaveData
{

    public Vector3 player_position;

    public float currentOrderInterval;
    public float currentTime;

    public List<InventorySaveData> inventorySaveData;
    public List<AllItemNumberSaveData> allItemNumberSaveData;

    public List<ItemSaveData> itemSaveData;

    public List<PlantSaveData> plantSaveData;

    public List<TileSaveData> tileSaveData;

    public List<AnimalSaveData> animalSaveData;

    public List<OrderSaveData> orderSaveData;

    public List<AchievementSaveData> achievementSaveData;

    public StatsSaveData statsSaveData;

    public List<MagicSaveData> magicSaveData;
}



[System.Serializable]
public class InventorySaveData
{
    public ItemTypes itemType;
    public int itemID;
    public int itemQuality;
    public int slotIndex;
    public int itemNum;

}

[System.Serializable]
public class AllItemNumberSaveData
{
    public ItemTypes itemType;
    public int itemID;
    public int itemQuality;
    public int allItemNumber;
}


[System.Serializable]
public class ItemSaveData
{
    public ItemTypes itemType;
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
    public ItemTypes itemType;
    public int itemID;
    public int itemQuality;
    public string titleText, goldNumber;

    public float starNumber;
    public int itemNeedNumber;
    public int rewardNumber;
}

[System.Serializable]
public class AchievementSaveData
{
    public AchieveCondition condition;
    public int ID;
    public int currentValue;
    public bool isLock;
}

[System.Serializable]
public class StatsSaveData
{
    public int goldCount;
    public int achieveCount;

    public List<TypeSaveData> typeSaveDatas;
}


[System.Serializable]
public class TypeSaveData
{
    public MagicTypes type;
    public int levelAdd;
    public float percentAdd;
}


[System.Serializable]
public class MagicSaveData
{
    public string ID;
    public bool isUnlock;
    public int level;
}