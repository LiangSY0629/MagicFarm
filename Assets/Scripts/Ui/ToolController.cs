using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolController : MonoBehaviour
{
    public static ToolController Instance;

    public GameObject seedPanel;
    public GameObject toolChoose;
    public SeedChoose seedChoose;
    public Tools currentTool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    //按工具栏按钮选择当前工具
    private void Start()
    {
        seedPanel.SetActive(false);
    }
    public void PressedAxe()
    {
        currentTool = Tools.Axe;
    }
    public void PressedHoe()
    {
        currentTool = Tools.Hoe;
    }
    public void PressedKettle()
    {
        currentTool = Tools.Kettle;
    }
    public void PressedBag()
    {
        currentTool = Tools.None;
    }
    public void PressedSeed()
    {
        currentTool = Tools.Seed;
        seedPanel.SetActive(!seedPanel.activeSelf);

        StatsManager.Instance.maxSeed = seedChoose.seed.Length;

        for (int i = Mathf.RoundToInt(StatsManager.Instance.seedLevel.finalValue * StatsManager.Instance.EveryLevelSeed); i < seedChoose.seed.Length; i++)
        {
            seedChoose.seed[i].SetActive(false);
        }
        for(int i = 0; i < StatsManager.Instance.maxSeed; i++)
        {
            if (i <= StatsManager.Instance.seedLevel.finalValue * StatsManager.Instance.EveryLevelSeed)
            {
                seedChoose.seed[i].SetActive(true);
            }
            else seedChoose.seed[i].SetActive(false);
        }

    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            
            //检测是否点击了空白处
            if (!EventSystem.current.IsPointerOverGameObject())
            {

                if (seedPanel.activeSelf)
                {
                    seedPanel.SetActive(false);
                    SoundEffectManager.Instance.PlayAudio("Cancel");
                }
                else SoundEffectManager.Instance.PlayAudio("Select");
            }

            if (toolChoose != null)
            {
                //将当前的按钮置于激活状态
                EventSystem.current.SetSelectedGameObject(toolChoose);
            }

        } 

        
    }

    public void selectTool(Button btn)
    {
        SoundEffectManager.Instance.PlayAudio("Select");
        if (toolChoose == btn)
        {
            return;
        }
        if (toolChoose != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        toolChoose = btn.gameObject;

        EventSystem.current.SetSelectedGameObject(toolChoose);
        
    }

}
