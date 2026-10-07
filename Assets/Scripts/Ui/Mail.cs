using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mail : MonoBehaviour
{
    [Header("…Ë÷√–°√Ê∞Â")]
    public StatsManager.ItemTypes itemType;
    public int itemID;
    public int itemQuality;
    public Image itemImage;
    public TMP_Text titleText, goldNumber;

    public int starNumber;
    public int itemNeedNumber;
    public int rewardNumber;


    public void SetOrder()
    {
        MailBoxManager.Instance.SetOrder(this);
    }

}
