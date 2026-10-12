using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicManager : MonoBehaviour
{
    public static MagicManager Instance;

    public Transform magicPanelTransform;
    public List<MagicSO> MagicSos;
    public Dictionary<string, MagicSO> magicSoDictionary;
    public Dictionary<string, MagicRunTime> magicRunDictionary;

    /// <summary>
    /// 定义的内部类，保存当前Magic的运行情况；
    /// </summary>
    public class MagicRunTime
    {
        public string ID;
        public bool isUnlock;
        public int level;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

    }

    private void Start()
    {

        magicSoDictionary = new Dictionary<string, MagicSO>();
        magicRunDictionary = new Dictionary<string, MagicRunTime>();


        //遍历List 将数据存入Dictionary中；
        foreach (MagicSO magic in MagicSos)
        {

            magicSoDictionary[magic.ID] = magic;

            MagicRunTime magicRun = new();
            magicRun.ID = magic.ID;

            magicRun.isUnlock = false;

            if(magicRun.ID == "default")
            {
                magicRun.isUnlock = true;
            }

            magicRun.level = 0;

            magicRunDictionary[magicRun.ID] = magicRun;

        }

        //遍历所有的按钮；通过所对应的RunTime的isUnlock来判断是否能点击，并根据是否点亮来进行更换图标；
        foreach(Transform transform in magicPanelTransform)
        {
            MagicNode magicNode = transform.GetComponent<MagicNode>();
            magicNode.ID = magicNode.magicSo.ID;
            magicNode.buttonIcon.sprite = magicNode.magicSo.icon;

            GetMagicRunTime(magicNode);

        }


    }


    /// <summary>
    /// 传入ID找到对应的Magic Runtime属性，
    /// </summary>
    /// <param name="magicNode"></param>
    public void GetMagicRunTime(MagicNode magicNode)
    {
        //判断MagicRunTime的状态，决定是否可以点击；
        if (magicRunDictionary.TryGetValue(magicNode.ID, out MagicRunTime magicRun))
        {
            magicNode.GetComponent<Button>().interactable = magicRun.isUnlock;

            if (magicRun.level > 0)
            {
                magicNode.buttonImage.sprite = magicNode.lightIcon;
            }
        }

    }

    /// <summary>
    /// 传入ID值进行索引，找到对应的Runtime更新状态，发出事件，更新ui；
    /// </summary>
    /// <param name="ID"></param>
    public void MagicLevelUp(string ID)
    {
        if(magicRunDictionary.TryGetValue(ID, out MagicRunTime magicRun))
        {
            if (magicRun.isUnlock)
            {
                magicRun.level++;

                if (magicRun.level == 1)
                {
                    foreach (string laterID in magicSoDictionary[ID].laterCondition)
                    {
                        if (magicRunDictionary.TryGetValue(laterID, out MagicRunTime laterMagicRun))
                        {
                            laterMagicRun.isUnlock = true;

                            Debug.Log("yes");
                        }



                    }

                    Debug.Log("level UP And Magic Unlock");

                }
                else
                {
                    Debug.Log("Only Level UP");
                }
            }
        }


    }

}
