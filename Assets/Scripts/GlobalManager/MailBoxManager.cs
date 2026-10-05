using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class MailBoxManager : MonoBehaviour, IInteractable
{
    public static MailBoxManager Instance { get; set; }

    public bool haveMail = false;

    public GameObject mailPanel;
    public Transform mailTransform;

    [Header("设置mail的格式")]
    public List<Mail> mails;

    public GameObject mailPrefab;

    [SerializeField]
    public Item[] plantItemPrefabs;
    [SerializeField]
    public Item[] animalItemPrefabs;
    public int itemValue;


    [Header("设置大面板")]
    public GameObject orderPanel;
    public Image needImage2;
    public TMP_Text orderTitleText, orderNeedNumber, rewardNumberText;
    public int whichMail;
    public GameObject[] stars;

    public float mailInterval;
    InventoryController inventoryController;
    ItemDictionary itemDictionary;
    Mail currentMail;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }


    void Start()
    {
        mails = new List<Mail>();
        mailInterval = StatsManager.Instance.orderInterval;
        itemDictionary = FindObjectOfType<ItemDictionary>();
        inventoryController = FindObjectOfType<InventoryController>();
    }


    void FixedUpdate()
    {
        if (mailInterval > 0)
        {
            mailInterval -= Time.deltaTime;
        }
        else
        {
            //mailInterval = Random.Range(StatsManager.Instance.orderInterval - 20, StatsManager.Instance.orderInterval + 20);
            mailInterval = StatsManager.Instance.orderInterval;

            if (mails.Count < StatsManager.Instance.maxMail)
            {
                SetMail();
                SoundEffectManager.Instance.PlayAudio("Confirm");
                TipsPopupControler.Instance.SetTipsText("您有新的种了么订单，请及时处理~");

            }
            else
            {
                TipsPopupControler.Instance.SetTipsText("信箱收到的订单已满，请尽快处理");
            }


        }
    }


    public bool CanInteracte()
    {
        return !mailPanel.activeSelf;
    }

    public void Interacte()
    {
        if (mailPanel.activeSelf || PauseController.IsGamePaused)
        {
            return;
        }


        if (mails.Count > 0)
        {
            haveMail = true;
        }
        else
        {
            haveMail = false;
        }


        if (haveMail && !mailPanel.activeSelf)
        {
            mailPanel.SetActive(true);
            PauseController.SetPause(true);
        }
        else
        {
            TipsPopupControler.Instance.SetTipsText("暂时还没有收到信件或订单");
        }

        SoundEffectManager.Instance.PlayAudio("Select");

    }


    /// <summary>
    /// 生成一个随机的可获取的掉落物，一个随机的数量，并计算它的总价值
    /// </summary>
    public void SetMail()
    {
        Debug.Log("开始生成");

        Item item = null;
        string itemName;
        int itemNeed;
        int rewardNumber;
        float rewardFactor;

        if (plantItemPrefabs.Length > 0 && animalItemPrefabs.Length > 0)
        {
            //先判断选中的是植物还是动物；
            int currentChoose = Random.Range(0, 2);

            //设定一个等级值，在判断当前选中的掉落物Item是什么等级；
            int currentLevel;

            //设定一个ID值，判断当前选中的是哪一个；
            int currentItemID;
            int itemID;


            if (currentChoose == 0)
            {
                currentLevel = Random.Range(0, StatsManager.Instance.seedLevel);

                currentItemID = Random.Range(0, StatsManager.Instance.EveryLevelSeed);

                itemID = currentItemID + (currentLevel * StatsManager.Instance.EveryLevelSeed);

                rewardFactor = Mathf.Pow(2, (currentLevel * 2));

                if (itemID < plantItemPrefabs.Length && itemID >= 0)
                {
                    item = plantItemPrefabs[itemID];
                }
                else
                {
                    Debug.Log("是植物物品不足");
                }

            }
            else
            {
                currentLevel = Random.Range(0, StatsManager.Instance.currentAnimalTypes);

                currentItemID = Random.Range(0, StatsManager.Instance.animalNumber);

                itemID = currentItemID + (currentLevel * StatsManager.Instance.animalNumber);

                rewardFactor = Mathf.Pow(2, (currentLevel * 2)) * 1.5f;

                if (itemID < animalItemPrefabs.Length)
                {
                    item = animalItemPrefabs[itemID];
                }
                else
                {
                    Debug.Log("是动物物品不足");
                }

            }

            if (item == null)
            {

                SetMail();
                return;
            }

            if (item.Name != null)
            {
                itemName = item.Name;
            }
            else itemName = "未知";


            itemNeed = Random.Range(6, 10) * StatsManager.Instance.orderLevel / (currentLevel + 1);

            rewardNumber = (int) (itemNeed * StatsManager.Instance.rewardLevel * itemValue * rewardFactor);

        }
        else
        {
            Debug.Log("未设置掉落物数组");
            return;
        }

        //将前面获得的属性复制到mail中；

        Mail mail = Instantiate(mailPrefab, mailTransform).GetComponent<Mail>();

        string levelName = "";

        if (StatsManager.Instance.orderLevel == 1)
        {
            levelName = "初级";
        }
        else if (StatsManager.Instance.orderLevel == 2)
        {
            levelName = "中级";
        }
        else levelName = "高级";

        mail.titleText.text = itemName + "的" + levelName + "订单";
        mail.rewardNumber = rewardNumber;
        mail.goldNumber.text = $"{rewardNumber}";
        mail.needImage1.sprite = item.GetComponent<Image>().sprite;
        mail.itemID = item.ID;
        mail.starNumber = StatsManager.Instance.orderLevel;
        mail.itemNeedNumber = itemNeed;

        mails.Add(mail);

    }


    public void CloseMailPanel()
    {
        PauseController.SetPause(false);
        mailPanel.SetActive(false);
        CloseOrder();
        SoundEffectManager.Instance.PlayAudio("Cancel");
    }


    public void CloseOrder()
    {
        orderPanel.SetActive(false);
        currentMail = null;
        SoundEffectManager.Instance.PlayAudio("Cancel");
    }

    /// <summary>
    /// 将mail的信息传入订单order中；
    /// </summary>
    /// <param name="mail"></param>
    public void SetOrder(Mail mail)
    {
        orderPanel.SetActive(true);

        currentMail = mail;
        SoundEffectManager.Instance.PlayAudio("Select");

        orderTitleText.text = mail.titleText.text;
        rewardNumberText.text = mail.goldNumber.text;
        needImage2.sprite = mail.needImage1.sprite;
        int currentHaveItem = itemDictionary.itemNumDictionary[mail.itemID];
        orderNeedNumber.text = $"{currentHaveItem}/{mail.itemNeedNumber}";

        for (int i = 0; i < stars.Length; i++)
        {
            if (i < mail.starNumber)
            {
                stars[i].SetActive(true);
            }
            else stars[i].SetActive(false);
        }

    }

    /// <summary>
    /// 提交订单操作，从库存中扣除对应的物品；
    /// </summary>
    public void SubmitOrder()
    {
        if (currentMail != null)
        {
            int currentHaveItem = itemDictionary.itemNumDictionary[currentMail.itemID];
            if (currentHaveItem >= currentMail.itemNeedNumber)
            {
                SoundEffectManager.Instance.PlayAudio("Confirm");

                //inventoryController.LessItem(currentMail.itemType, currentMail.itemID, currentMail.itemNeedNumber);
                TimeController.Instance.SetGold(currentMail.rewardNumber);
                TipsPopupControler.Instance.SetTipsText("种了么订单提交成功！");
                mails.Remove(currentMail);
                Destroy(currentMail.gameObject);
                mails[0].SetOrder();
            }
            else
            {
                SoundEffectManager.Instance.PlayAudio("Cancel");
                TipsPopupControler.Instance.SetTipsText("库存不足！");
                return;
            }

        }
        else
        {
            SoundEffectManager.Instance.PlayAudio("Cancel");
            TipsPopupControler.Instance.SetTipsText("尚未选择订单");
        }
    }
    

    public List<OrderSaveData> GetSaveData()
    {
        List<OrderSaveData> orderSaveData = new List<OrderSaveData>();

        foreach(Mail mail in mails)
        {
            orderSaveData.Add(new OrderSaveData
            {
                itemID = mail.itemID,
                titleText = mail.titleText.text,
                goldNumber = mail.goldNumber.text,
                starNumber = mail.starNumber,
                itemNeedNumber = mail.itemNeedNumber,
                rewardNumber = mail.rewardNumber,
            });
        }

        return orderSaveData;
    }


    /// <summary>
    /// 传入list，将存档中的信息加载；
    /// </summary>
    /// <param name="mailSaveData"></param>
    public void SetMailData(List<OrderSaveData> mailSaveData)
    {
        foreach (Mail mail in mails)
        {
            Destroy(mail.gameObject);
        }

        StartCoroutine(SetMailDataIE(mailSaveData));
    }

    IEnumerator SetMailDataIE(List<OrderSaveData> saveData)
    {
        yield return new WaitForSeconds(0.1f);
        mails.Clear();

        foreach(OrderSaveData mailData in saveData)
        {
            Mail mail = Instantiate(mailPrefab, mailTransform).GetComponent<Mail>();

            mail.itemID = mailData.itemID;
            //mail.needImage1.sprite = itemDictionary.GetItemPrefab(mailData.itemID).GetComponent<Image>().sprite;
            mail.titleText.text = mailData.titleText;
            mail.goldNumber.text = mailData.goldNumber;
            mail.starNumber = mailData.starNumber;
            mail.itemNeedNumber = mailData.itemNeedNumber;
            mail.rewardNumber = mailData.rewardNumber;

            mails.Add(mail);
        }

    }


}
