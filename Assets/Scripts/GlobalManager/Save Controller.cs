using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SaveController : MonoBehaviour
{
    public static SaveController Instance;

    private string save_localtion;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }


    void Start()
    {
        save_localtion = Path.Combine(Application.persistentDataPath, "saveData.json");

    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            player_position = GameObject.FindGameObjectWithTag("Player").transform.position,
            currentTime = TimeController.Instance.currentTime,
            currentOrderInterval = MailBoxManager.Instance.mailInterval,

            inventorySaveData = InventoryController.Instance.GetSaveSlot(),
            allItemNumberSaveData = InventoryController.Instance.GetItemNumber(),
            itemSaveData = ItemSaveController.Instance.GetItemData(),
            plantSaveData = PlantController.Instance.GetSaveData(),
            tileSaveData = TileMapManager.Instance.GetTileData(),
            animalSaveData = AnimalsManager.Instance.GetAnimalsSaveData(),
            orderSaveData = MailBoxManager.Instance.GetSaveData(),
            achievementSaveData = AchieveController.Instance.GetSaveData(),
            statsSaveData = StatsManager.Instance.GetSaveData(),
            magicSaveData = MagicManager.Instance.GetSaveData(),

        };

        File.WriteAllText(save_localtion, JsonUtility.ToJson(saveData));
    }

    public bool LoadGame()
    {

        if (File.Exists(save_localtion))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(save_localtion));

            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.player_position;
            TimeController.Instance.currentTime = saveData.currentTime;
            MailBoxManager.Instance.mailInterval = saveData.currentOrderInterval;

            InventoryController.Instance.SetInventoryData(saveData.inventorySaveData);
            InventoryController.Instance.SetItemNumber(saveData.allItemNumberSaveData);
            ItemSaveController.Instance.SetItemDate(saveData.itemSaveData);
            PlantController.Instance.SetPlant(saveData.plantSaveData);
            TileMapManager.Instance.SetTileData(saveData.tileSaveData);
            TimeController.Instance.SetTime();
            StatsManager.Instance.SetGoldCount(0);
            AnimalsManager.Instance.SetAnimalsSaveData(saveData.animalSaveData);
            MailBoxManager.Instance.SetMailData(saveData.orderSaveData);
            AchieveController.Instance.SetAchieveData(saveData.achievementSaveData);
            StatsManager.Instance.SetSaveData(saveData.statsSaveData);
            MagicManager.Instance.SetSaveData(saveData.magicSaveData);
            return true;
        }
        else
        {
            return false;
        }

    }

}
