using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MagicManager : MonoBehaviour
{
    public static MagicManager Instance;

    public Transform magicPanelTransform;
    public List<MagicSO> MagicSos;
    public Dictionary<string, List<MagicSO>> postconditionIdDictionary;
    public Dictionary<string, MagicRunTime> magicRunDictionary;
    public Dictionary<string, MagicNode> magicNodeDictionary;
    public Dictionary<(string, string), GameObject> magicLineDictionary;

    public Sprite lockSprite,unlockSprite;
    public Transform lineTransform;
    public GameObject linePrefab;
    public Color lockColor, lightColor;


    [Header("设置描述面板")]
    public GameObject descriptionPanel;
    public TMP_Text titleText, descriptionText, preconditionText, levelText;
    public Button levelUpButton;
    public string currentID;

    public GameObject remindPanel;
    public TMP_Text needText;

    /// <summary>
    /// 定义的内部类，保存当前Magic的运行情况；
    /// </summary>
    public class MagicRunTime
    {
        public string ID;
        public bool isUnlock;
        public int level;
        public int maxLevel;

    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

    }

    private void Start()
    {
        postconditionIdDictionary = new Dictionary<string, List<MagicSO>>();
        magicRunDictionary = new Dictionary<string, MagicRunTime>();
        magicNodeDictionary = new Dictionary<string, MagicNode>();
        magicLineDictionary = new Dictionary<(string, string), GameObject>();

        //遍历List 将数据存入Dictionary中；
        foreach (MagicSO magic in MagicSos)
        {
            MagicRunTime magicRun = new();

            magicRun.ID = magic.ID;
            magicRun.level = 0;
            magicRun.maxLevel = magic.maxLevel;

            if (magic.precondition.Length == 0)
            {
                magicRun.isUnlock = true;
            }
            else magicRun.isUnlock = false;

            magicRunDictionary[magicRun.ID] = magicRun;
            

            foreach(string preID in magic.precondition) //存一个反向表，跟据当前magic SO的前置条件，把该magic SO存为它们的后置节点；
            {

                if (!postconditionIdDictionary.ContainsKey(preID))
                    postconditionIdDictionary[preID] = new List<MagicSO>();

                postconditionIdDictionary[preID].Add(magic);
            }

        }


        //遍历所有的按钮；通过所对应的RunTime的isUnlock来判断是否能点击，并根据是否点亮来进行更换图标；
        foreach(Transform transform in magicPanelTransform)
        {
            MagicNode magicNode = transform.GetComponent<MagicNode>();
            magicNode.ID = magicNode.magicSo.ID;
            magicNode.magicSo.magicPosition = transform.position;
            magicNode.buttonIcon.sprite = magicNode.magicSo.icon;
            magicNode.levelText = transform.GetComponentInChildren<TMP_Text>();
            magicNode.buttonImage = transform.GetComponent<Image>();
            magicNodeDictionary[magicNode.ID] = magicNode;

            SetMagicNodeText(magicNode.ID);
        }


        foreach (MagicSO magic in MagicSos)
        {
            if (postconditionIdDictionary.ContainsKey(magic.ID))
            {
                foreach (MagicSO postMagic in postconditionIdDictionary[magic.ID])
                {
                    magicLineDictionary[(magic.ID, postMagic.ID)] = SetLine(magic.magicPosition, postMagic.magicPosition);
                }
            }


        }

        levelUpButton = descriptionPanel.GetComponentInChildren<Button>();
        levelUpButton.onClick.AddListener(() => OpenRemindPanel());

        remindPanel.SetActive(false);
    }

    /// <summary>
    /// 传入ID，在详情面板显示信息；
    /// </summary>
    /// <param name="ID"></param>
    public void SetDescriptionPanel(string ID)
    {
        currentID = ID;
        MagicSO magic = magicNodeDictionary[ID].magicSo;

        titleText.text = magic.magicName;
        descriptionText.text = magic.description;
        preconditionText.text = "前置魔法：";
        if (magic.precondition.Length > 0)
        {
            foreach (string preID in magic.precondition)
            {
                preconditionText.text += "\n" + magicNodeDictionary[preID].magicSo.magicName;
            }
        }
        else preconditionText.text += "\n无";

        MagicRunTime magicRun = magicRunDictionary[ID];
        SetLevelText(magicRun);

        SoundEffectManager.Instance.PlayAudio("Select");
    }

    /// <summary>
    /// 传入RunTime信息，更新详情面板的level信息；
    /// </summary>
    /// <param name="magicRun"></param>
    public void SetLevelText(MagicRunTime magicRun)
    {
        if (magicRun.level >= magicRun.maxLevel)
        {
            levelUpButton.interactable = false;
            levelText.text = "已满级";
        }
        else
        {
            levelUpButton.interactable = magicRun.isUnlock;
            levelText.text = "当前等级：" + magicRun.level + "/" + magicRun.maxLevel;
        }
    }


    /// <summary>
    /// 打开remind面板；
    /// </summary>
    public void OpenRemindPanel()
    {
        remindPanel.SetActive(true);
        needText.text = "升级消耗：\n魔法天赋石 * 1" + "（" + StatsManager.Instance.magicStone + "）" ;

        needText.text += "\n金币 * " + magicNodeDictionary[currentID].magicSo.upNeed + "（" + StatsManager.Instance.goldCount + "）";
        SoundEffectManager.Instance.PlayAudio("Confirm");
    }

    /// <summary>
    /// 关闭remind面板；
    /// </summary>
    public void CloseRemindPanel()
    {
        remindPanel.SetActive(false);
        SoundEffectManager.Instance.PlayAudio("Cancel");
    }

    /// <summary>
    /// 传入ID值进行索引，找到对应的Runtime更新状态，发出事件，更新ui；
    /// </summary>
    /// <param name="ID"></param>
    public void MagicLevelUp()
    {

        if(StatsManager.Instance.magicStone >= 1 && StatsManager.Instance.goldCount >= magicNodeDictionary[currentID].magicSo.upNeed)
        {
            StatsManager.Instance.magicStone -= 1;
            StatsManager.Instance.SetGoldCount(-magicNodeDictionary[currentID].magicSo.upNeed);
            TipsPopupControler.Instance.SetTipsText("升级成功!");
            SoundEffectManager.Instance.PlayAudio("Magic");
        }
        else
        {
            TipsPopupControler.Instance.SetTipsText("当前材料不足以升级！");
            SoundEffectManager.Instance.PlayAudio("Cancel");
            return;
        }


        //是否存在magic Run；
        if (magicRunDictionary.TryGetValue(currentID, out MagicRunTime magicRun))
        {
            if (magicRun.isUnlock) //检测是不是解锁状态，
            {
                if (magicRun.level < magicRun.maxLevel) //当前等级未达到最大等级，升一级，
                {
                    magicRun.level++;

                    foreach (MagicEffect magicEffect in magicNodeDictionary[currentID].magicSo.magicEffectSos)
                    {
                        magicEffect.MagicEffectSo.MagicAssert(magicEffect.value);
                    }

                    SetMagicNodeText(currentID);
                    SetLevelText(magicRun);
                    EventController.TriggerComplete(AchieveCondition.Skill, 1);
                    //Debug.Log(magicRun.level);
                }

                if (magicRun.level >= magicRun.maxLevel) //当前等级已达到最大等级，
                {
                    magicRun.level = magicRun.maxLevel;

                    SetMagicNodeText(currentID);
                    SetLevelText(magicRun);
                    //Debug.Log("Max Level");

                    SetRunTime(currentID); //调用函数更新节点状态；

                }

            }

            remindPanel.SetActive(false);
        }
        else return;


    } //函数结束；


    /// <summary>
    /// 传入已满级的ID，判断其他ID所处的节点是否要解锁；
    /// </summary>
    /// <param name="ID"></param>
    public void SetRunTime(string ID)
    {
        if (!postconditionIdDictionary.ContainsKey(ID))
            return;

        foreach (MagicSO magicSo in postconditionIdDictionary[ID])  //遍历该天赋节点的后置节点，
        {
            bool isReady = true;  //设置一个bool值 判断是否存在节点未准备就绪；

            foreach (string preID in magicSo.precondition)  //遍历后置节点的前置节点；
            {
                //如果找不到这个前置节点 或 这个前置节点未达到最大等级（没准备就绪），就赋值为false，否则不变化；
                if (!magicRunDictionary.TryGetValue(preID, out MagicRunTime magicRun) || magicRun.level != magicRun.maxLevel)
                {
                    isReady = false;
                }
            }

            if (isReady) //如果经过遍历后仍准备就绪
            {
                magicRunDictionary[magicSo.ID].isUnlock = true; //为这个RunTime解锁；

                if (magicNodeDictionary.TryGetValue(magicSo.ID, out MagicNode magicNode))  //查找这个ID的节点，为其更新；
                {
                    SetMagicNodeText(magicSo.ID);
                }

            }

        }//遍历结束；

        SetLineColor(ID);

    } //函数结束；
    

    /// <summary>
    /// 根据传入ID对应的RunTime状态 更改line的颜色
    /// </summary>
    /// <param name="ID"></param>
    public void SetLineColor(string ID)
    {
        if (!postconditionIdDictionary.ContainsKey(ID))
            return;

        foreach(MagicSO magicSo in postconditionIdDictionary[ID])  //遍历所有的后置节点 如果该节点满级，就变亮，否则变暗；
        {
            if (magicRunDictionary[ID].level < magicRunDictionary[ID].maxLevel)
            {
                magicLineDictionary[(ID, magicSo.ID)].GetComponent<Image>().color = lockColor;
            }
            else magicLineDictionary[(ID, magicSo.ID)].GetComponent<Image>().color = lightColor;
        }

    }


    /// <summary>
    /// 传入要更新的ID，进行更新；
    /// </summary>
    /// <param name="ID"></param>
    public void SetMagicNodeText(string ID)
    {
        if (!magicNodeDictionary.TryGetValue(ID, out MagicNode magicNode)) //没找到节点直接返回；
            return;
        else
        {
            MagicRunTime magicRun = magicRunDictionary[ID];
            

            if (!magicRun.isUnlock)  //如果没解锁，就说被锁着；
            { 
                magicNode.levelText.text = "Locked";
                magicNode.buttonImage.sprite = lockSprite;   //更新图标
            }
            else
            {
               
                if (magicRun.level < magicRun.maxLevel)
                {
                    magicNode.levelText.text = magicRun.level + "/" + magicRun.maxLevel;
                    magicNode.buttonImage.sprite = unlockSprite;  //更新图标
                }

                else
                {
                    magicNode.levelText.text = "MAX";
                    magicNode.buttonImage.sprite = magicNode.magicSo.lightIcon;  //更新图标；
                }

            }
        }

    }

    /// <summary>
    /// 传入两个位置，在两个位置之间画一条线；
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    private GameObject SetLine(Vector2 from, Vector2 to)
    {
        Vector2 dircetion = to - from;
        float lenth = dircetion.magnitude;
        float angle = Mathf.Atan2(dircetion.y, dircetion.x) * Mathf.Rad2Deg;  //计算两点夹角;

        GameObject line = Instantiate(linePrefab, lineTransform);
        RectTransform linePosition = line.GetComponent<RectTransform>();
        linePosition.position = from + dircetion / 2;
        linePosition.sizeDelta = new Vector2(lenth, 2);
        linePosition.localRotation = Quaternion.Euler(0, 0, angle);
        return line;
    }


    /// <summary>
    /// 获取可供保存的信息；
    /// </summary>
    /// <returns></returns>
    public List<MagicSaveData> GetSaveData()
    {
        List<MagicSaveData> data = new List<MagicSaveData>();

        foreach(var dic in magicRunDictionary)
        {
            data.Add(new MagicSaveData
            {
                ID = dic.Key,
                isUnlock = dic.Value.isUnlock,
                level = dic.Value.level,
            });
        }

        return data;

    }


    /// <summary>
    /// 将保存的信息赋值到运行状态，并更新当前的ui状态；
    /// </summary>
    /// <param name="magicSaveData"></param>
    public void SetSaveData(List<MagicSaveData> magicSaveData)
    {

        foreach(MagicSaveData data in magicSaveData)
        {
            MagicRunTime magicRun = magicRunDictionary[data.ID];

            magicRun.isUnlock = data.isUnlock;
            magicRun.level = data.level;

            SetLineColor(magicRun.ID);

            SetMagicNodeText(data.ID);
        }

        if(magicRunDictionary.TryGetValue(currentID, out MagicRunTime magicRun1))
            SetLevelText(magicRun1);  //更新详情面板正在显示的节点的状态，

    }



}
