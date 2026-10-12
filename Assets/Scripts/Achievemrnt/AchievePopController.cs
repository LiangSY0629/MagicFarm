using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Linq;

public class AchievePopController : MonoBehaviour
{
    public TMP_Text achieveText;
    public Image achieveImage;

    public float popupTime;
    public float waitTime;
    public int position;

    bool isPopup = false;
    readonly Queue<(string Name, Sprite icon)> waitPopup = new();

    /// <summary>
    /// 传入成就名，调用函数实现成就弹窗；
    /// </summary>
    /// <param name="achieveName"></param>
    public void AchievePopup(string achieveName, Sprite icon)
    {

        if (isPopup)
        {
            waitPopup.Enqueue((achieveName, icon));
            return;
        }

        achieveText.text = achieveName;
        achieveImage.sprite = icon;
        StartCoroutine(PopUpActive(achieveName));
    }

    IEnumerator PopUpActive(string achieveName)
    {
        isPopup = true;
        EventController.TriggerAchieveUp();
        Vector2 startPosition = transform.position;
        Vector2 currentPosition = startPosition;

        //先渐入
        for(float t = 0; t < popupTime; t += Time.deltaTime)
        {
            float y = Mathf.Lerp(startPosition.y, position, t / popupTime);
            currentPosition.y = y;
            transform.position = currentPosition;

            yield return null;
        }
        //停留一段时间；
        yield return new WaitForSeconds(waitTime);

        //再渐出；
        for(float t = 0; t < popupTime; t += Time.deltaTime)
        {
            float y = Mathf.Lerp(position, startPosition.y, t / popupTime);
            currentPosition.y = y;
            transform.position = currentPosition;

            yield return null;
        }

        isPopup = false;

        if(waitPopup.Count > 0)
        {
            var next = waitPopup.Dequeue();
            AchievePopup(next.Name, next.icon);
        }

    }

}
