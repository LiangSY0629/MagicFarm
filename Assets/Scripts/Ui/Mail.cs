using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mail : MonoBehaviour
{
    [Header("…Ë÷√–°√Ê∞Â")]
    public int itemID;
    public Image needImage1;
    public TMP_Text titleText, goldNumber;

    public int starNumber;
    public int itemNeedNumber;
    public int rewardNumber;


    public void SetOrder()
    {
        MailBoxManager.Instance.SetOrder(this);
    }

}
