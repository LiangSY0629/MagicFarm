using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MagicManager : MonoBehaviour
{
    public static MagicManager Instance;

    public Transform magicPanelTransform;
    public List<MagicSO> MagicSos;
    public Dictionary<string, List<MagicSO>> postconditionIdDictionary;
    public Dictionary<string, MagicRunTime> magicRunDictionary;
    public Dictionary<string, MagicNode> magicNodeDictionary;

    /// <summary>
    /// 定义的内部类，保存当前Magic的运行情况；
    /// </summary>
    public class MagicRunTime
    {
        public string ID;
        public bool isUnlock;
        public int level;
        public int maxLevel;

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
        postconditionIdDictionary = new Dictionary<string, List<MagicSO>>();
        magicRunDictionary = new Dictionary<string, MagicRunTime>();
        magicNodeDictionary = new Dictionary<string, MagicNode>();

        //遍历List 将数据存入Dictionary中；
        foreach (MagicSO magic in MagicSos)
        {
            MagicRunTime magicRun = new();

            magicRun.ID = magic.ID;
            magicRun.level = 0;
            magicRun.maxLevel = magic.maxLevel;

            if (magic.precondition.Length == 0)
            {
                magicRun.isUnlock = true;
            }
            else magicRun.isUnlock = false;

            magicRunDictionary[magicRun.ID] = magicRun;
            

            foreach(string preID in magic.precondition) //存一个反向表，跟据当前magic SO的前置条件，把该magic SO存为它们的后置节点；
            {

                if (!postconditionIdDictionary.ContainsKey(preID))
                    postconditionIdDictionary[preID] = new List<MagicSO>();

                postconditionIdDictionary[preID].Add(magic);
            }

        }

        //遍历所有的按钮；通过所对应的RunTime的isUnlock来判断是否能点击，并根据是否点亮来进行更换图标；
        foreach(Transform transform in magicPanelTransform)
        {
            MagicNode magicNode = transform.GetComponent<MagicNode>();
            magicNode.ID = magicNode.magicSo.ID;
            magicNode.buttonIcon.sprite = magicNode.magicSo.icon;

            magicNodeDictionary[magicNode.ID] = magicNode;

            SetMagicNodeText(magicNode.ID);
        }


    }


    /// <summary>
    /// 传入ID值进行索引，找到对应的Runtime更新状态，发出事件，更新ui；
    /// </summary>
    /// <param name="ID"></param>
    public void MagicLevelUp(MagicNode magicNode)
    {
        //是否存在magic Run；
        if(magicRunDictionary.TryGetValue(magicNode.ID, out MagicRunTime magicRun))
        {
            if (magicRun.isUnlock) //检测是不是解锁状态，
            {
                if(magicRun.level < magicRun.maxLevel) //当前等级未达到最大等级，升一级，
                {
                    magicRun.level++;
                    
                    foreach(MagicEffect magicEffect in magicNode.magicSo.magicEffectSos)
                    {
                        magicEffect.MagicEffectSo.MagicAssert(magicEffect.value);
                    }

                    SetMagicNodeText(magicNode.ID);
                    Debug.Log(magicRun.level);
                }

                if(magicRun.level >= magicRun.maxLevel) //当前等级已达到最大等级，
                {
                    magicRun.level = magicRun.maxLevel;

                    SetMagicNodeText(magicNode.ID);
                    Debug.Log("Max Level");

                    UpdateRunTime(magicNode.ID); //调用函数更新节点状态；

                }
              
            }
        }


    } //函数结束；

    /// <summary>
    /// 传入已满级的ID，判断其他ID所处的节点是否要解锁；
    /// </summary>
    /// <param name="ID"></param>
    public void UpdateRunTime(string ID)
    {
        if (!postconditionIdDictionary.ContainsKey(ID))
            return;

        foreach (MagicSO magicSo in postconditionIdDictionary[ID])  //遍历该天赋节点的后置节点，
        {
            bool isReady = true;  //设置一个bool值 判断是否存在节点未准备就绪；

            foreach (string preID in magicSo.precondition)  //遍历后置节点的前置节点；
            {
                //如果找不到这个前置节点 或 这个前置节点未达到最大等级（没准备就绪），就赋值为false，否则不变化；
                if (!magicRunDictionary.TryGetValue(preID, out MagicRunTime magicRun) || magicRun.level != magicRun.maxLevel)
                {
                    isReady = false;
                }
            }

            if (isReady) //如果经过遍历后仍准备就绪
            {
                magicRunDictionary[magicSo.ID].isUnlock = true; //为这个RunTime解锁；

                if (magicNodeDictionary.TryGetValue(magicSo.ID, out MagicNode magicNode))  //查找这个ID的节点，为其更新；
                {
                    SetMagicNodeText(magicSo.ID);
                }

            }

        }//遍历结束；

    } //函数结束；


    /// <summary>
    /// 传入要更新的ID，进行更新；
    /// </summary>
    /// <param name="ID"></param>
    public void SetMagicNodeText(string ID)
    {
        if (!magicNodeDictionary.TryGetValue(ID, out MagicNode magicNode)) //没找到节点直接返回；
            return;
        else
        {
            MagicRunTime magicRun = magicRunDictionary[ID];

            if (!magicNode.button.interactable)
            {
                magicNode.button.interactable = magicRun.isUnlock;
            }

            if (!magicRun.isUnlock)  //如果没解锁，就说被锁着；
                magicNode.levelText.text = "Locked";
            else
            {

                if (magicRun.level < magicRun.maxLevel)
                    magicNode.levelText.text = magicRun.level + "/" + magicRun.maxLevel;
                else
                {
                    magicNode.levelText.text = "MAX";
                    magicNode.buttonImage.sprite = magicNode.lightIcon;  //更新图标；
                }

            }
        }

    }

}
