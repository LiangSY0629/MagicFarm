using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance;

    public new Light2D light;

    [Header("设置时间基本信息")]

    public int startTime;
    public int oneDay;
    public int oneHour;

    public TMP_Text timeText;
    public TMP_Text goldText;

    [Header("设置颜色")]
    public float targetTime;
    public Color dayColor;
    public Color yellowColor;
    public Color nightColor;

    [Header("不要改这个")]
    public float currentTime;
    public int day;
    public int hour;
    public int minute;
    public bool isNight = false;

    public float finalTime;
    public float dayTime;
    float updateTime = 0.1f;
    int nightTime;
    int lightTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (startTime != 0)
        {
            SetTime();
        }

        SetGold(StatsManager.Instance.goldCount);

        nightTime = 18 * oneHour;
        lightTime = 6 * oneHour;

    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        updateTime -= Time.deltaTime;

        finalTime = startTime + currentTime;

        dayTime = finalTime % oneDay;

        if (updateTime <= 0)
        {
            updateTime = 0.2f;
            SetTime();

            if (!isNight && hour >= 18)
            {
                isNight = true;
            }

            if (isNight && hour >= 6 && hour < 18)
            {
                isNight = false;
            }

        }

        if (dayTime >= nightTime - targetTime && dayTime <= nightTime + targetTime)
        {
            SetNight();

        }
        else if (dayTime >= lightTime - targetTime && dayTime <= lightTime + targetTime)
        {
            SetDay();
        }

    }

    /// <summary>
    /// 根据当前的finalTime来设置当前的时间；
    /// </summary>
    public void SetTime()
    {

        day = (int)finalTime / oneDay;
        hour = (int)dayTime / oneHour;
        minute = (int)dayTime % oneHour;

        timeText.text = $"DAY {day:D3} {hour:D2}:{minute:D2}";

    }

    /// <summary>
    /// 传入int 数字，直接添加到当前金币总量中；
    /// </summary>
    /// <param name="number"></param>
    public void SetGold(int number)
    {
        StatsManager.Instance.goldCount += number;
        goldText.text = $"{(StatsManager.Instance.goldCount):D2}";
    }


    void SetNight()
    {
        float colorTime = (dayTime - nightTime + targetTime);
        float t = colorTime / targetTime;

        if (t <= 1)
        {
            light.color = Color.Lerp(dayColor, yellowColor, t);
            light.intensity = Mathf.Lerp(1, 0.75f, t);
        }
        else 
        {
            light.color = Color.Lerp(yellowColor, nightColor, t - 1);
            light.intensity = Mathf.Lerp(0.75f, 0.5f, t - 1);
        }

    }

    void SetDay()
    {
        float colorTime = (dayTime - lightTime + targetTime);
        float t = colorTime / targetTime;

        if (t <= 1)
        {
            light.color = Color.Lerp(nightColor, yellowColor, t);
            light.intensity = Mathf.Lerp(0.5f, 0.75f, t);
        }
        else
        {
            light.color = Color.Lerp(yellowColor, dayColor, t - 1);
            light.intensity = Mathf.Lerp(0.75f, 1, t - 1);
        }

    }


}
