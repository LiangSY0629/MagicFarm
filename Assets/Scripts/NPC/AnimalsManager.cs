using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimalsManager : MonoBehaviour
{
    public static AnimalsManager Instance;

    [Header("设置动物管理面板")]
    public bool panelActive;

    public GameObject animalPanel;
    public Button[] animalButtons;

    public Button[] feedButtons;
    public GameObject feedChooseText;
    public Button feedButton;

    [Header("设置管理的动物")]
    public List<AnimalController> animals;
    public int animalID;

    public InventoryController inventoryController;

    FeedInformation feedChoose;
    int feedID;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        inventoryController = FindObjectOfType<InventoryController>();

        foreach(Transform transform in transform)
        {
            if (transform.GetComponent<AnimalController>() != null)
            {
                animals.Add(transform.GetComponent<AnimalController>());
            }
        }

        for(int i = 0; i < animals.Count; i++)
        {
            animals[i].ID = i;
        }
    }


    public void OpenPanel()
    {
        PauseController.SetPause(true);
        animalPanel.SetActive(true);
        panelActive = true;
        animals[animalID].SetButtonSprite();

        for(int i = 0; i < StatsManager.Instance.maxAnimalTypes; i++)
        {
            if (i < StatsManager.Instance.currentAnimalTypes)
            {
                animalButtons[i].gameObject.SetActive(true);
            }
            else animalButtons[i].gameObject.SetActive(false);

        }

        feedButton.gameObject.SetActive(false);
        feedChooseText.SetActive(false);

    }

    public void ClosePanel()
    {
        animalPanel.SetActive(false);
        panelActive = false;
        PauseController.SetPause(false);
        SoundEffectManager.Instance.PlayAudio("Cancel");
    }

    //显示当前选择的饲料品种；
    public void PrepareFeed(int ID)
    {
        feedButton.gameObject.SetActive(true);
        feedChooseText.SetActive(true);
        if (ID < animals[animalID].feeds.Count)
        {
            feedChoose = animals[animalID].feeds[ID];
            feedID = ID;
        }

        animals[animalID].needNutrition = (int)(Mathf.Pow(2, (animals[animalID].animalType * animals[animalID].need) + 1));

        feedChooseText.GetComponent<TMP_Text>().text = "已选择：" + feedButtons[ID].GetComponentInChildren<TMP_Text>().text;

        SoundEffectManager.Instance.PlayAudio("Select");
    }

    //切换动物品种
    public void SwitchAnimalType(int types)
    {
        if (animals[animalID].eating)
        {
            TipsPopupControler.Instance.SetTipsText("动物正在进食，请不要切换品种！");
            return;
        }

        animals[animalID].SetAnimalType(types);
        SoundEffectManager.Instance.PlayAudio("Select");

    } 
    
    /// <summary>
    /// 按下喂养按钮，将当前饲料的属性赋值出去；
    /// </summary>
    public void PressedFeedButton()
    {

        SoundEffectManager.Instance.PlayAudio("Confirm");


        if (TimeController.Instance.isNight)
        {
            TipsPopupControler.Instance.SetTipsText("天黑后还是不要喂动物比较好！");
            return;
        }


        if (animals[animalID].eating)
        {
            TipsPopupControler.Instance.SetTipsText("动物正在进食，请不要再次喂食！它吃不下的！！！");
            return;
        }

        int nutrition = (int)(Mathf.Pow(2, feedID * 2)) * feedChoose.feedNumber;

        if (animals[animalID].needNutrition > nutrition)
        {
            TipsPopupControler.Instance.SetTipsText("当前饲料的等级太低了，动物们吃不饱，换个高级的吧！");
            return;
        }

        if (inventoryController.LessItem(feedChoose.feedID, feedChoose.feedNumber))
        {
            
            animals[animalID].SetFeed(feedChoose , nutrition);
        }
        else
        {
            TipsPopupControler.Instance.SetTipsText("你没有足够多的食物来喂动物们！去种一点吧！");
        }

    }

    /// <summary>
    /// 获取动物存档信息List
    /// </summary>
    /// <returns></returns>
    public List<AnimalSaveData> GetAnimalsSaveData()
    {
        List<AnimalSaveData> animalSaveData = new List<AnimalSaveData>(); 

        foreach(AnimalController animal in animals)
        {
             animalSaveData.Add(animal.GetSaveData());
        }

        return animalSaveData;
    }

    /// <summary>
    /// 传入保存的动物信息List，将信息传给各个动物管理器；
    /// </summary>
    /// <param name="animalSaveData"></param>
    public void SetAnimalsSaveData(List<AnimalSaveData> animalSaveData)
    {

        foreach(AnimalSaveData animalData in animalSaveData)
        {

            animals[animalData.animalID].SetData(animalData);

        }

    }



}
