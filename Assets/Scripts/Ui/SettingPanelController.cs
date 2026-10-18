using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanelController : MonoBehaviour
{
    public GameObject SaveGamePanel;
    public GameObject LoadGamePanel;
    public GameObject enterPanel;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider MusicSlider;
    float sfxVolumeChange;
    float musicVolumeChange;
    bool sfxChange = false;
    bool musicChange = false;


    private void Start()
    {
        SaveGamePanel.SetActive(false);
        LoadGamePanel.SetActive(false);
        enterPanel.SetActive(false);

        sfxSlider.onValueChanged.AddListener(delegate { OnSfxValueChanged(); });
        MusicSlider.onValueChanged.AddListener(delegate { OnMusicValueChanged(); });
    }

    /// <summary>
    /// 打开Save Panel;
    /// </summary>
    public void OpenSave()
    {
        SaveGamePanel.SetActive(true);
        SoundEffectManager.Instance.PlayAudio("Select");
    }

    /// <summary>
    /// 关闭Save Panel；
    /// </summary>
    public void CancelSave()
    {
        SaveGamePanel.SetActive(false);
        SoundEffectManager.Instance.PlayAudio("Cancel");
    }

    /// <summary>
    /// 打开Load Panel；
    /// </summary>
    public void OpenLoad()
    {
        LoadGamePanel.SetActive(true);
        SoundEffectManager.Instance.PlayAudio("Select");
    }

    /// <summary>
    /// 关闭Load Panel；
    /// </summary>
    public void CancelLoad()
    {
        LoadGamePanel.SetActive(false);
        SoundEffectManager.Instance.PlayAudio("Cancel");
    }


    /// <summary>
    /// 确认要存档，调用存档管理器存档；
    /// </summary>
    public void ConfirmSave()
    {
        SaveController.Instance.SaveGame();
        SaveGamePanel.SetActive(false);
        TipsPopupControler.Instance.SetTipsText("存档成功！");
        SoundEffectManager.Instance.PlayAudio("Confirm");
    }

    /// <summary>
    /// 确认要加载，调用存档管理器加载；
    /// </summary>
    public void ConfirmLoad()
    {
        if (SaveController.Instance.Loading())
        {
            SaveController.Instance.LoadGame();
            TipsPopupControler.Instance.SetTipsText("加载成功！");
            LoadGamePanel.SetActive(false);
            SoundEffectManager.Instance.PlayAudio("Confirm");
        }
        else
        {
            TipsPopupControler.Instance.SetTipsText("没有可供加载的存档，请先保存！");
            SoundEffectManager.Instance.PlayAudio("Confirm");
        }

    }

    /// <summary>
    /// 按下退出按钮；
    /// </summary>
    public void EnterButton()
    {
        enterPanel.SetActive(true);
        SoundEffectManager.Instance.PlayAudio("Select");
    }

    public void ReturnMainScene()
    {
        SaveController.Instance.SaveGame();
        GameManager.Instance.ReturnMainMenu();
        enterPanel.SetActive(false);
        SoundEffectManager.Instance.PlayAudio("Confirm");
    }

    public void ReturnWithoutSave()
    {
        GameManager.Instance.ReturnMainMenu();
        enterPanel.SetActive(false);
        SoundEffectManager.Instance.PlayAudio("Confirm");
    }

    public void CloseGame()
    {
        SaveController.Instance.SaveGame();
        enterPanel.SetActive(false);
        SoundEffectManager.Instance.PlayAudio("Confirm");
        GameManager.Instance.CloseGame();
    }

    public void CancelEnter()
    {
        enterPanel.SetActive(false);
        SoundEffectManager.Instance.PlayAudio("Cancel");
    }


    /// <summary>
    /// 当音效滑块发生变化时，如果变化大于3%，就触发一次音效；
    /// </summary>
    public void OnSfxValueChanged()
    {
        if (!sfxChange)
        {
            sfxVolumeChange = sfxSlider.value;
            sfxChange = true;
        }
        else
        {
            if (Mathf.Abs(sfxVolumeChange - sfxSlider.value) > 0.03f)
            {
                SoundEffectManager.Instance.PlayAudio("Select");
                sfxChange = false;
            }
        }
        SoundEffectManager.SetVolume(sfxSlider.value * 0.8f);
    }

    /// <summary>
    /// 当音乐滑块发生变化时，如果变化大于3%，就触发一次音效；
    /// </summary>
    public void OnMusicValueChanged()
    {
        if (!musicChange)
        {
            musicVolumeChange = MusicSlider.value;
            musicChange = true;
        }
        else
        {
            if (Mathf.Abs(musicVolumeChange - MusicSlider.value) > 0.03f)
            {
                SoundEffectManager.Instance.PlayAudio("Select");
                musicChange = false;
            }
        }
        MusicEffectManager.Instance.SetVolume(MusicSlider.value);
    }

}
