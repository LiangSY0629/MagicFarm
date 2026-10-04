using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemController : MonoBehaviour
{
    InventoryController inventoryController;

    private void Start()
    {
        inventoryController = FindAnyObjectByType<InventoryController>();
    }

    //检测是否进入到可拾取物的碰撞范围
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            if (item != null)
            {
                //调用可拾取物的拾取动画
                item.StartFlyAnim(this.transform, collision.gameObject);
            }
        }
    }

}



