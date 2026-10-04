using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolController : MonoBehaviour
{
    public GameObject seedChoose;
    public GameObject toolChoose;

    //按工具栏按钮选择当前工具
    private void Start()
    {
        seedChoose.SetActive(false);
    }
    public void PressedAxe()
    {
        StatsManager.Instance.current_tool = StatsManager.Tools.Axe;
    }
    public void PressedHoe()
    {
        StatsManager.Instance.current_tool = StatsManager.Tools.Hoe;
    }
    public void PressedKettle()
    {
        StatsManager.Instance.current_tool = StatsManager.Tools.Kettle;
    }
    public void PressedBag()
    {
        StatsManager.Instance.current_tool = StatsManager.Tools.None;
    }
    public void PressedSeed()
    {
        StatsManager.Instance.current_tool = StatsManager.Tools.Seed;
        seedChoose.SetActive(!seedChoose.activeSelf);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            
            //检测是否点击了空白处
            if (!EventSystem.current.IsPointerOverGameObject())
            {

                if (seedChoose.activeSelf)
                {
                    seedChoose.SetActive(false);
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
