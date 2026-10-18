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
    public GameObject feedGameObject;
    public TMP_Text feedChooseText,productText;
    public TMP_Text feedNumberText;
    public Button feedButton;

    [Header("设置管理的动物")]
    public List<AnimalController> animals;
    public int animalID;

    FeedInformation feedChoose;
    int currentFeedNumber;
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

    /// <summary>
    /// 打开动物管理面板；
    /// </summary>
    public void OpenPanel()
    {
        PauseController.SetPause(true);
        animalPanel.SetActive(true);
        panelActive = true;
        animals[animalID].SetButtonSprite();

        for(int i = 0; i < StatsManager.Instance.maxAnimalTypes; i++)
        {
            if (i < StatsManager.Instance.currentAnimalTypes.finalValue)
            {
                animalButtons[i].gameObject.SetActive(true);
            }
            else animalButtons[i].gameObject.SetActive(false);

        }

        feedGameObject.SetActive(false);

    }

    /// <summary>
    /// 关闭动物管理面板；
    /// </summary>
    public void ClosePanel()
    {
        animalPanel.SetActive(false);
        panelActive = false;
        PauseController.SetPause(false);
        SoundEffectManager.Instance.PlayAudio("Cancel");
    }

    /// <summary>
    /// 显示当前选择的饲料品种；
    /// </summary>
    /// <param name="ID"></param>
    public void PrepareFeed(int ID)
    {
        feedGameObject.SetActive(true);   //显示喂养的文本及按钮；

        if (ID < animals[animalID].feeds.Count)    //若当前选的饲料可以被索引找到，就赋值
        {
            feedChoose = animals[animalID].feeds[ID];
            feedID = ID;
            currentFeedNumber = feedChoose.feedNumber;
        }
        //计算当前动物品种所需的营养；
        animals[animalID].needNutrition = (int)(Mathf.Pow(2, (animals[animalID].animalType * animals[animalID].need) + 1));
        feedChooseText.text = "已选择：" + feedButtons[ID].GetComponentInChildren<TMP_Text>().text;

        SetFeedText();   //刷新文本内容；
        SoundEffectManager.Instance.PlayAudio("Select");
    }

    public void AddFeedNumber()
    {
        currentFeedNumber++;
        SetFeedText();
        SoundEffectManager.Instance.PlayAudio("Select");
    }

    public void LessFeedNumber()
    {
        if (currentFeedNumber > 0)
        {
            currentFeedNumber--;
            SetFeedText();
        }
        else
        {
            currentFeedNumber = 0;
            TipsPopupControler.Instance.SetTipsText("喂养数量不可以为零");
        }
        SoundEffectManager.Instance.PlayAudio("Select");
    }

    /// <summary>
    ///刷新文本内容；
    /// </summary>
    public void SetFeedText()
    {
        int nutrition = (int)(Mathf.Pow(2, feedID * 2)) * currentFeedNumber;
        productText.text = "预计产物数量：*" + animals[animalID].FigureProduct(nutrition);
        feedNumberText.text = currentFeedNumber.ToString();
    }

    /// <summary>
    /// 切换动物品种
    /// </summary>
    /// <param name="types"></param>
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

        int nutrition = (int)(Mathf.Pow(2, feedID * 2)) * currentFeedNumber;

        if (animals[animalID].needNutrition > nutrition)
        {
            TipsPopupControler.Instance.SetTipsText("当前饲料的营养值太低了，动物们吃不饱，换个高级的吧！");
            return;
        }

        if (InventoryController.Instance.LessItem(ItemTypes.Crop, feedChoose.feedID, 1, currentFeedNumber))
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
