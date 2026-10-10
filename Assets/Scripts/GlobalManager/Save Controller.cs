using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SaveController : MonoBehaviour
{

    private string save_localtion;

    private InventoryController inventoryController;
    ItemSaveController itemSaveController;
    PlantController plantController;

    public GameObject SaveGamePanel;
    public GameObject LoadGamePanel;

    void Start()
    {
        SaveGamePanel.SetActive(false);
        LoadGamePanel.SetActive(false);

        save_localtion = Path.Combine(Application.persistentDataPath, "saveData.json");
        inventoryController = FindObjectOfType<InventoryController>();
        itemSaveController = FindObjectOfType<ItemSaveController>();
        plantController = FindObjectOfType<PlantController>();

    }
    #region 控制提醒面板开关。
    public void OpenSave()
    {
        SaveGamePanel.SetActive(!SaveGamePanel.activeSelf);
        SoundEffectManager.Instance.PlayAudio("Select");
    }

    public void CancelSave()
    {
        SaveGamePanel.SetActive(false);
        SoundEffectManager.Instance.PlayAudio("Cancel");
    }

    public void OpenLoad()
    {
        LoadGamePanel.SetActive(!LoadGamePanel.activeSelf);
        SoundEffectManager.Instance.PlayAudio("Select");
    }

    public void CancelLoad()
    {
        LoadGamePanel.SetActive(false);
        SoundEffectManager.Instance.PlayAudio("Cancel");
    }

    #endregion
    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            player_position = GameObject.FindGameObjectWithTag("Player").transform.position,
            square = StatsManager.Instance.square,
            currentTime = TimeController.Instance.currentTime,
            currentOrderInterval = MailBoxManager.Instance.mailInterval,

            inventorySaveData = inventoryController.GetSaveSlot(),
            allItemNumberSaveData = inventoryController.GetItemNumber(),
            itemSaveData = itemSaveController.GetItemData(),
            plantSaveData = plantController.GetSaveData(),
            tileSaveData = TileMapManager.Instance.GetTileData(),
            animalSaveData = AnimalsManager.Instance.GetAnimalsSaveData(),
            orderSaveData = MailBoxManager.Instance.GetSaveData(),
            achievementSaveData = AchieveController.Instance.GetSaveData(),

        };

        File.WriteAllText(save_localtion, JsonUtility.ToJson(saveData));
        SoundEffectManager.Instance.PlayAudio("Confirm");
        SaveGamePanel.SetActive(!SaveGamePanel.activeSelf);
        TipsPopupControler.Instance.SetTipsText("存档成功！");
    }

    public void LoadGame()
    {

        if (File.Exists(save_localtion))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(save_localtion));

            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.player_position;
            StatsManager.Instance.square = saveData.square;
            TimeController.Instance.currentTime = saveData.currentTime;
            MailBoxManager.Instance.mailInterval = saveData.currentOrderInterval;

            inventoryController.SetInventoryData(saveData.inventorySaveData);
            inventoryController.SetItemNumber(saveData.allItemNumberSaveData);
            itemSaveController.SetItemDate(saveData.itemSaveData);
            plantController.SetPlant(saveData.plantSaveData);
            TileMapManager.Instance.SetTileData(saveData.tileSaveData);
            TimeController.Instance.SetTime();
            AnimalsManager.Instance.SetAnimalsSaveData(saveData.animalSaveData);
            MailBoxManager.Instance.SetMailData(saveData.orderSaveData);
            AchieveController.Instance.SetAchieveData(saveData.achievementSaveData);

            TipsPopupControler.Instance.SetTipsText("加载成功！");
        }
        else
        {
            TipsPopupControler.Instance.SetTipsText("没有可供加载的存档，请先保存！");
        }
        SoundEffectManager.Instance.PlayAudio("Confirm");
        LoadGamePanel.SetActive(!LoadGamePanel.activeSelf);
    }

}
