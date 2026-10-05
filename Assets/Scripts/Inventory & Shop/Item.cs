using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Item : MonoBehaviour
{
    //由Item SO进行赋值的变量
    public int ID;
    public string Name;
    public int level;
    public int quality;
    public Sprite image;
    public Sprite Sprite;
    public StatsManager.ItemTypes Type;

    //后期再生成的变量；
    public int number = 1;
    public int pickNumber;

    public bool isFlying = false;
    float flyTime = .6f;
    float jumpHeight = 1f;

    InventoryController inventoryController;

    private void Start()
    {
        inventoryController = FindObjectOfType<InventoryController>();
        image = GetComponent<Image>().sprite;
        Sprite = GetComponent<SpriteRenderer>().sprite;
    }

    /// <summary>
    /// 传入Item SO和quality，对item执行赋值操作；
    /// </summary>
    /// <param name="itemType"></param>
    /// <param name="itemSO"></param>
    /// <param name="itemQuality"></param>
    public void CreateItem(StatsManager.ItemTypes itemType, ItemSO itemSO, int itemQuality)
    {
        ID = itemSO.ID;
        Name = itemSO.itemName;
        image = itemSO.icon;
        Sprite = itemSO.icon;
        level = itemSO.itemLevel;
        quality = itemQuality;
        Type = itemType;
    }

    /// <summary>
    /// 更新物品栏的数量图标显示；
    /// </summary>
    public void AddNumber()
    {
        GetComponentInChildren<TMP_Text>().text = "" + number;
    }

    //拾取时随机数量
    public void RandomPickNum()
    {
        pickNumber = Random.Range(1, StatsManager.Instance.maxPickNumber);
    }

    //调用ui弹窗，将物品信息返回ui弹窗函数
    public  void PickUp()
    {
        Sprite sprite = GetComponent<Image>().sprite;
        if (ItemPickUpController.Instance != null)
        {
            ItemPickUpController.Instance.ShowItemPickUp(Name, sprite, pickNumber, quality);
            SoundEffectManager.Instance.PlayAudio("PickUp");
        }
    }

    //通过代码实现物品先跳跃一下，再飞向角色；
    public void StartFlyAnim(Transform playerTransform, GameObject collision)
    {
        if (isFlying)
        {
            return;
        }

        StartCoroutine(FlyToPlayer(playerTransform, collision));

    }

    IEnumerator FlyToPlayer(Transform playerTransform, GameObject collision)
    {
        isFlying = true;

        Vector3 itemPosition = transform.position;

        for(float time = 0f; time < flyTime; time += Time.deltaTime)
        {

            float t = time / flyTime;
            Vector3 linePosition = Vector3.Lerp(itemPosition, playerTransform.position, t);
            float jump = Mathf.Sin(1.5f * time * Mathf.PI) * jumpHeight;

            linePosition.y += jump;
            transform.position = linePosition;

            yield return null;
           
        }

        //调用库存添加函数，传入Game Object作为库存面板中显示的game object；
        bool itemAdded = inventoryController.AddItem(collision);

        if (itemAdded)
        {
            PickUp();
            Destroy(collision);
        }
    }



    public void Harvest(float jumpTime)
    {
        StartCoroutine(ItemJumpFormPlant(jumpTime));
    }


    IEnumerator ItemJumpFormPlant(float jumpTime)
    {
        isFlying = true;

        Vector3 itemPosition = transform.position;
        Vector3 distance = Random.insideUnitCircle.normalized * Random.Range(0.5f, 1.5f);
        Vector3 endDistance = itemPosition + distance;

        for (float i = 0; i < jumpTime; i += Time.deltaTime)
        {

            Vector3 flyLine = Vector3.Lerp(itemPosition, endDistance, i / jumpTime);
            flyLine.y += Mathf.Sin(i * Mathf.PI / jumpTime) * 0.75f;
            transform.position = flyLine;


            yield return null;
        }

        isFlying = false;
    }



}
