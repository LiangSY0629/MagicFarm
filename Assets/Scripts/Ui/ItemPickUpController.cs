using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickUpController : MonoBehaviour
{
    public static ItemPickUpController Instance { get; private set; }

    public GameObject popupPrefab;
    public int maxPopups = 6;
    public float popupDuration;

    readonly Queue<GameObject> activePopups = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 显示掉落物弹窗；
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="itemSprite"></param>
    /// <param name="number"></param>
    public void ShowItemPickUp(string itemName, Sprite itemSprite, int number)
    {
        GameObject popup = Instantiate(popupPrefab, transform);

        popup.GetComponentInChildren<TMP_Text>().text = itemName + "×" + number;
        Image image = popup.transform.Find("ItemImage").GetComponent<Image>();

        if (image != null)
        {
            image.sprite = itemSprite;
        }

        activePopups.Enqueue(popup);

        if (activePopups.Count > maxPopups)
        {
            Destroy(activePopups.Dequeue());
        }

        StartCoroutine(FabeOutAndDestory(popup));
    }

    IEnumerator FabeOutAndDestory(GameObject popup)
    {
        yield return new WaitForSeconds(popupDuration);

        //如果被挤掉，就停止协程；
        if (popup == null)
        {
            yield break;
        }
        CanvasGroup canvasGroup = popup.GetComponent<CanvasGroup>();
        for (float timePassed = 0f; timePassed < 1f; timePassed += Time.deltaTime)
        {
            if (popup == null)
            {
                yield break;
            }

            //没被顶掉就慢慢淡化；
            canvasGroup.alpha = 1f - timePassed;
            yield return null;

        }

        Destroy(popup);

    }


}
