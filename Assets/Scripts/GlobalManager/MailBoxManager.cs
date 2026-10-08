using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MailBoxManager : MonoBehaviour, IInteractable
{
    public static MailBoxManager Instance { get; set; }

    public bool haveMail = false;

    public GameObject mailPanel;
    public Transform mailTransform;

    [Header("设置mail的格式")]
    public List<Mail> mails;

    public GameObject mailPrefab;

    public int itemValue;


    [Header("设置大面板")]
    public GameObject orderPanel;
    public Image itemImage;
    public TMP_Text orderTitleText, orderNeedNumber, rewardNumberText;
    public int whichMail;
    public GameObject[] stars;

    public float mailInterval;
    InventoryController inventoryController;
    Animator anim;
    bool NewMail;
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
        inventoryController = FindObjectOfType<InventoryController>();
        anim = GetComponent<Animator>();
        NewMail = false;

    }


    void FixedUpdate()
    {
        //晚上停止计时；
        if (TimeController.Instance.isNight)
        {
            return;
        }

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
                if (!NewMail)
                {
                    NewMail = true;
                    anim.SetBool("NewMail", NewMail);
                }
                

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
            NewMail = false;
            anim.SetBool("NewMail", NewMail);
            
        }
        else
        {
            TipsPopupControler.Instance.SetTipsText("暂时还没有收到信件或订单");
        }

        SoundEffectManager.Instance.PlayAudio("Select");

    }

    //切换动画为有新订单；
    public void AnimatorMail()
    {
        anim.Play("MailHave", 0, 0);
    }

    //绑定信箱打开动画，动画结束自动运行；
    public void OpenMailBox()
    {
        mailPanel.SetActive(true);
        PauseController.SetPause(true);
    }

    //绑定关闭动画，判断是否还有订单未提交，
    public void TipsMail()
    {
        if(mails.Count > 0)
        {
            NewMail = true;
            anim.SetBool("NewMail", NewMail);
        }
        
    }

    public StatsManager.ItemTypes RandomType()
    {
        var types = Enum.GetValues(typeof(StatsManager.ItemTypes));
        return (StatsManager.ItemTypes)types.GetValue(UnityEngine.Random.Range(0, types.Length));

    }

    /// <summary>
    /// 生成一个随机的可获取的掉落物，一个随机的数量，并计算它的总价值
    /// </summary>
    public void SetMail()
    {
        
        Item item = null;
        int needNumber;
        int rewardNumber;
        float rewardFactor = 1;

        StatsManager.ItemTypes currentType;
        int currentLevel;
        int currentID;
        int currentQuality;

        currentType = RandomType();
        
        //如果可以找到对应的字典，就取出这个等级的字典；
        if (ItemDictionary.Instance.itemLevelDictionary.TryGetValue(currentType, out Dictionary<int,List<ItemSO>> LevelDictionary))
        {
            if(LevelDictionary.Count == 0)
            {
                return;
            }

            //用List保存所有的Level；
            List<int> levelList = new List<int>(LevelDictionary.Keys);

            //如果是植物，就用当前最大植物等级来随机等级；
            if (currentType == StatsManager.ItemTypes.Crop)
            {
                currentLevel = levelList[UnityEngine.Random.Range(0, StatsManager.Instance.seedLevel)];
                rewardFactor = 1;
            }
            //如果是动物，就用当前最大动物等级来随机等级；
            else if (currentType == StatsManager.ItemTypes.Animal)
            {
                currentLevel = levelList[UnityEngine.Random.Range(0, StatsManager.Instance.currentAnimalTypes)];
                rewardFactor = 2;
            }
            else currentLevel = 1;

            List<ItemSO> itemList = LevelDictionary[currentLevel];

            //随机当前等级下的掉落物；
            currentID = itemList[UnityEngine.Random.Range(0, LevelDictionary[currentLevel].Count)].ID;

            //随机可获得的品质；
            currentQuality = UnityEngine.Random.Range(1, StatsManager.Instance.maxQuality);

            item = ItemDictionary.Instance.GetItemPrefab(currentType, currentID, currentQuality).GetComponent<Item>();

            //随机一些订单系数
            needNumber = UnityEngine.Random.Range(6, 10) * StatsManager.Instance.orderLevel / currentLevel;
            //报酬 = 需求数量 * 物品价值 * 物品品质 * 奖励系数（植物为默认的1；动物为2） * 奖励等级 * （当前等级 - 1）的 4次方；
            rewardNumber = (int)(needNumber * itemValue * currentQuality * rewardFactor * StatsManager.Instance.rewardLevel * Mathf.Pow(4, currentLevel - 1));

            if(item == null)
            {
                SetMail();
                Debug.Log("重新生成");
                return;
            }

            //将获取到的各种属性赋值给Mail；
            Mail mail = Instantiate(mailPrefab, mailTransform).GetComponent<Mail>();

            string levelName;

            if (StatsManager.Instance.orderLevel == 1)
            {
                levelName = "初级";
            }
            else if (StatsManager.Instance.orderLevel == 2)
            {
                levelName = "中级";
            }
            else if (StatsManager.Instance.orderLevel == 3)
            {
                levelName = "高级";
            }
            else levelName = "特级";

            mail.titleText.text = $"{item.quality}阶{item.Name}的{levelName}订单";
            mail.rewardNumber = rewardNumber;
            mail.goldNumber.text = $"{rewardNumber}";
            mail.itemImage.sprite = item.GetComponent<Image>().sprite;
            mail.starNumber = StatsManager.Instance.orderLevel;
            mail.itemNeedNumber = needNumber;
            //保存item的信息，以便使用；
            mail.itemType = item.Type;
            mail.itemID = item.ID;
            mail.itemQuality = item.quality;

            mails.Add(mail);

        }

    }

    //关闭整个订单面板；
    public void CloseMailPanel()
    {
        PauseController.SetPause(false);
        mailPanel.SetActive(false);
        CloseOrder();
        SoundEffectManager.Instance.PlayAudio("Cancel");
        anim.Play("MailClose", 0, 0);
    }

    //只关闭打开的订单详情面板；
    public void CloseOrder()
    {
        orderPanel.SetActive(false);
        currentMail = null;
        SoundEffectManager.Instance.PlayAudio("Cancel");
    }

    /// <summary>
    /// 将mail的信息传入订单详情面板中；
    /// </summary>
    /// <param name="mail"></param>
    public void SetOrder(Mail mail)
    {
        orderPanel.SetActive(true);

        currentMail = mail;
        SoundEffectManager.Instance.PlayAudio("Select");

        //对详情面板进行赋值
        orderTitleText.text = mail.titleText.text;//标题
        rewardNumberText.text = mail.goldNumber.text;//奖励数字；
        itemImage.sprite = mail.itemImage.sprite;//掉落物图像；

        //获取当前库存的item数量
        ItemDictionary.Instance.itemNumberDictionary.TryGetValue((mail.itemType, mail.itemID, mail.itemQuality), out int currentHaveItem);

        orderNeedNumber.text = $"{currentHaveItem}/{mail.itemNeedNumber}";

        //生成当前订单的星级；
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
        //订单不为空；
        if (currentMail != null)
        {
            int currentHaveItem = ItemDictionary.Instance.itemNumberDictionary[(currentMail.itemType, currentMail.itemID, currentMail.itemQuality)];

            //库存足够就调用函数减少对应库存，从Mails这个List中移除对应的Mail，销毁当前mail，如果还有订单 就自动显示当前mails中的第一个mail的订单是详情面板；
            if (currentHaveItem >= currentMail.itemNeedNumber)
            {
                SoundEffectManager.Instance.PlayAudio("Confirm");

                inventoryController.LessItem(currentMail.itemType, currentMail.itemID, currentMail.itemQuality, currentMail.itemNeedNumber);
                TimeController.Instance.SetGold(currentMail.rewardNumber);
                TipsPopupControler.Instance.SetTipsText("种了么订单提交成功！");
                mails.Remove(currentMail);
                Destroy(currentMail.gameObject);

                if (mails.Count > 0)
                {
                    mails[0].SetOrder();
                }
                else
                {
                    orderPanel.SetActive(false);
                    currentMail = null;
                }

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
            return;
        }
    }
    
    /// <summary>
    /// 调用该函数获取保存着当前mail的属性的List；
    /// </summary>
    /// <returns></returns>
    public List<OrderSaveData> GetSaveData()
    {
        List<OrderSaveData> orderSaveData = new List<OrderSaveData>();

        foreach(Mail mail in mails)
        {
            orderSaveData.Add(new OrderSaveData
            {
                itemType = mail.itemType,
                itemID = mail.itemID,
                itemQuality = mail.itemQuality,
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

        //调用协程，在清空所有的Mail之后再生成新的Mail；
        StartCoroutine(SetMailDataIE(mailSaveData));
    }

    IEnumerator SetMailDataIE(List<OrderSaveData> saveData)
    {
        yield return new WaitForEndOfFrame();
        //先清空整个Mails；
        mails.Clear();

        //遍历整个List来生成Mail；
        foreach(OrderSaveData mailData in saveData)
        {
            Mail mail = Instantiate(mailPrefab, mailTransform).GetComponent<Mail>();

            mail.itemType = mailData.itemType;
            mail.itemID = mailData.itemID;
            mail.itemQuality = mailData.itemQuality;
            mail.itemImage.sprite = ItemDictionary.Instance.GetItemPrefab(mailData.itemType, mailData.itemID, mailData.itemQuality).GetComponent<Image>().sprite;
            mail.titleText.text = mailData.titleText;
            mail.goldNumber.text = mailData.goldNumber;
            mail.starNumber = mailData.starNumber;
            mail.itemNeedNumber = mailData.itemNeedNumber;
            mail.rewardNumber = mailData.rewardNumber;

            mails.Add(mail);
        }

        //判断该不该播放动画；
        TipsMail();
    }


}
