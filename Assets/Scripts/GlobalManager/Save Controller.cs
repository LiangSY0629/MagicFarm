using System.IO;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    public static SaveController Instance;

    private string save_localtion;
    public string total_save;

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
        total_save = Path.Combine(Application.persistentDataPath, "totalData.json");

        GameManager.Instance.currentUTC = System.DateTime.UtcNow;
        TotalLoad();
        Debug.Log(GameManager.Instance.totalPlayGame);
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

    public void TotalSave()
    {
        GameManager.Instance.SetPlayTime();
        TotalData totalData = new TotalData
        {
            totalPlayTime = GameManager.Instance.totalPlayGame,
            currentUTC = System.DateTime.UtcNow,
        };

        File.WriteAllText(total_save, JsonUtility.ToJson(totalData));
    }


    public bool Loading()
    {
        return File.Exists(save_localtion);
    }

    public void LoadGame()
    {
        if (!Loading())
        {
            return;
        }

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

    }

    public void TotalLoad()
    {
        if (File.Exists(total_save))
        {
            TotalData totalData = JsonUtility.FromJson<TotalData>(File.ReadAllText(total_save));
            GameManager.Instance.totalPlayGame = totalData.totalPlayTime;
            
        }
    }


}
