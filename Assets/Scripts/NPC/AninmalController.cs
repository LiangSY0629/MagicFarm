using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AnimalController : MonoBehaviour,IInteractable
{
    [Header("设置动物的贴图以及食物的属性")]
    public Sprite[] animalSprites;
    public List<FeedInformation> feeds;
    public List<GameObject> animals;
    public int ID;

    public GameObject chest;
    public int animalType;

    ItemDictionary itemDictionary;

    [Header("设置动物所需营养系数和产物系数")]
    public GameObject[] productPrefab;
    public int need = 2;
    public int product = 1;
    public bool eating = false;
    public int needNutrition;

    float eatingTime;
    int finalProduct;
    bool sleep = false;


    private void Start()
    {
        foreach(Transform transform in transform)
        {
            animals.Add(transform.gameObject);
        }

        itemDictionary = FindObjectOfType<ItemDictionary>();

    }


    private void Update()
    {
        //天黑后停止生产产物，天亮后继续；
        if (!TimeController.Instance.isNight)
        {
            if (eating)
            {
                eatingTime -= Time.deltaTime;

                if (eatingTime <= 0)
                {
                    SetProduct();
                }

            }

            if (sleep)
            {
                foreach(GameObject animal in animals)
                {
                    animal.GetComponent<CowMovement>().SetAwake();
                }
                sleep = false;
            }

        }
        else
        {
            if (!sleep)
            {
                foreach (GameObject animal in animals)
                {
                    animal.GetComponent<CowMovement>().SetSleep();  
                }
                sleep = true;
            }

        }

    }

    public bool CanInteracte()
    {
        return !AnimalsManager.Instance.panelActive;
    }

    public void Interacte()
    {
        if (AnimalsManager.Instance.panelActive || PauseController.IsGamePaused)
        {
            return;
        }

        if (!AnimalsManager.Instance.panelActive)
        {
            SoundEffectManager.Instance.PlayAudio("Confirm");
            AnimalsManager.Instance.animalID = ID;

            AnimalsManager.Instance.OpenPanel();
        }

    }

    /// <summary>
    /// 更改动物管理面板中动物品种的贴图和饲料种类的贴图；
    /// </summary>
    public void SetButtonSprite()
    {
        for(int i = 0; i< animalSprites.Length; i++)
        {
            AnimalsManager.Instance.animalButtons[i].GetComponent<Image>().sprite = animalSprites[i];
        }

        for (int i = 0; i < feeds.Count; i++)
        {
            AnimalsManager.Instance.feedButtons[i].GetComponent<Image>().sprite = feeds[i].feedIcon;
        }

    }


    /// <summary>
    /// 更改当前动物的种类；
    /// </summary>
    /// <param name="type"></param>
    public void SetAnimalType(int type)
    {
        animalType = type;
        needNutrition = (int)(Mathf.Pow(2, (animalType * need) + 1));

        foreach (GameObject animal in animals)
        {
            animal.GetComponent<CowMovement>().SetType(type);
        }

    }

    /// <summary>
    /// 根据当前饲料种类来判断提供的营养，消耗需要的时间，最终的产物；
    /// </summary>
    public void SetFeed(FeedInformation feed , int nutrition)
    {

        if (nutrition >= needNutrition)
        {
            finalProduct = (nutrition / needNutrition) * product;
        }
        else
        {

            return;
        }

        foreach (GameObject animal in animals)
        {
            animal.GetComponent<CowMovement>().SetEat(feed.eatTime);
        }

        eatingTime = feed.eatTime;
        eating = true;

    }

    private void SetProduct()
    {

        foreach(GameObject animal in animals)
        {
            animal.GetComponent<CowMovement>().SetProduct();
        }

        chest.GetComponent<Animator>().SetBool("Open", true);

        for(int i = 0; i < finalProduct; i++ )
        {
            GameObject milkPrefab = Instantiate(productPrefab[animalType], itemDictionary.transform);
            milkPrefab.transform.position = chest.transform.position;
            Item milk = milkPrefab.GetComponent<Item>();
            milk.Harvest(0.3f);
        }

        eating = false;

    }


    public AnimalSaveData GetSaveData()
    {
        AnimalSaveData SaveData = new AnimalSaveData();

        SaveData.animalID = ID;
        SaveData.animalType = animalType;
        SaveData.eating = eating;
        if (eating)
        {
            SaveData.eatingTime = eatingTime;
            SaveData.finalProduct = finalProduct;
        }

        return SaveData;
    }


    public void SetData(AnimalSaveData animalData)
    {

        ID = animalData.animalID;
        SetAnimalType(animalData.animalType);

        eating = animalData.eating;

        if (animalData.eating)
        {
            animalData.eatingTime = eatingTime;
            animalData.finalProduct = finalProduct;
        }

    }


}

[System.Serializable]
public class FeedInformation
{
    public Sprite feedIcon;
    public int feedID;
    public int feedNumber;
    public float eatTime;

}