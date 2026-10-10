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
    public int truthDay = 0;
    public int truthHour = 0;
    public int truthMin = 0;
    public bool isNight = false;

    public float finalTime;
    public float dayTime;
    float updateTime = 0.1f;
    float waitTruthTime = 0;
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
        waitTruthTime -= Time.deltaTime;

        finalTime = startTime + currentTime;

        dayTime = finalTime % oneDay;

        //不需要及时更新放在这里面节省部分性能；
        if (updateTime <= 0)
        {
            updateTime = 0.2f;
            SetTime();

            if (!isNight && (hour >= 18 || hour < 6))
            {
                isNight = true;
                TipsPopupControler.Instance.SetTipsText("晚上了，去屋子里的床上睡一觉吧！");
            }

            if (isNight && hour >= 6 && hour < 18)
            {
                isNight = false;

                //如果天数小于10，每更新一次黑夜状态（也就是过了一天）就会添加一点成就进度；
                if (day <= 10)
                {
                    EventController.TriggerComplete(2, 1);
                }
                
            }

            //每隔60s调用一下函数更新一下显示时间；
            if(waitTruthTime <= 0)
            {
                waitTruthTime = 60;
                SetTruthTime();
            }


            //防止修改时间后灯光无法匹配当前时间；
            if ((dayTime > nightTime + targetTime || dayTime < lightTime - targetTime) && light.intensity > 0.5)//大于天黑时间或小于天亮时间；
            {
                light.color = nightColor;
                light.intensity = 0.5f;
            }
            else if (dayTime > lightTime + targetTime && dayTime < nightTime - targetTime && light.intensity != 1)//大于天亮时间并小于天黑时间；
            {
                light.color = dayColor;
                light.intensity = 1;
            }

        }

        //判断当前时间来改变目前的灯光效果（即使改变时间，只要还在范围内就能切换对应的效果）
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

    public void SetTruthTime()
    {
        truthMin++;
        if(truthMin >= 60)
        {
            truthMin = 0;
            truthHour++;
        }

        if(truthHour >= 24)
        {
            truthHour = 0;
            truthDay++;
        }

        StatsManager.Instance.timeText.text = $"游戏时间：{truthDay:D2}天{truthHour:D2}小时{truthMin:D2}分钟";

    }


    /// <summary>
    /// 传入int 数字，直接添加到当前金币总量中；
    /// </summary>
    /// <param name="number"></param>
    public void SetGold(int number)
    {
        StatsManager.Instance.goldCount += number;
        StatsManager.Instance.glodText.text = $"当前金币数：{(StatsManager.Instance.goldCount)}";
        EventController.TriggerComplete(StatsManager.AchieveCondition.Glod, number);
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
