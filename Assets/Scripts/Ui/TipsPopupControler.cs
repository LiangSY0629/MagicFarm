using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TipsPopupControler : MonoBehaviour
{
    public static TipsPopupControler Instance;

    public GameObject PopupPrefab;
    public int maxPopup = 3;
    public float popupDuration = 2;

    readonly Queue<GameObject> activePopups = new();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    public void SetTipsText(string text)
    {
        GameObject popup = Instantiate(PopupPrefab, transform);

        popup.GetComponentInChildren<TMP_Text>().text = text;

        activePopups.Enqueue(popup);

        if (activePopups.Count > maxPopup)
        {
            Destroy(activePopups.Dequeue());
        }

        StartCoroutine(FabeOutAndDestroy(popup));
    }

    IEnumerator FabeOutAndDestroy(GameObject popup)
    {

        yield return new WaitForSeconds(popupDuration);

        if (popup == null)
        {
            yield break;
        }

        CanvasGroup canvas = popup.GetComponent<CanvasGroup>();
        Vector3 position = popup.transform.position;

        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            if (popup == null)
            {
                yield break;
            }
            position.y +=  i;
            popup.transform.position = position;
            canvas.alpha = 1f - i;
            yield return null;
        }

        Destroy(popup.gameObject);

    }


}
