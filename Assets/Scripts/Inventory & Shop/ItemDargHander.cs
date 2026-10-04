using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDargHander : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform originalParent;
    CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.75f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;

        Slot secondSlot = eventData.pointerEnter?.GetComponentInParent<Slot>();
        Slot firstSlot = originalParent.GetComponent<Slot>();


        if (secondSlot != null)
        {
            if (secondSlot.currentItem != null)
            {
                firstSlot.currentItem = secondSlot.currentItem;
                secondSlot.currentItem.transform.SetParent(originalParent,false);

                secondSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            }
            else
            {
                firstSlot.currentItem = null;
            }
            transform.SetParent(secondSlot.transform,false);
            secondSlot.currentItem = gameObject;

        }
        else
        {
            transform.SetParent(originalParent,false);
        }

        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

    }

}
