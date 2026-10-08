using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SceenFader : MonoBehaviour
{
    public static SceenFader Instance;
    public CanvasGroup canvas;
    public float fadeTime;

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
        canvas = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// 创建异步任务；
    /// </summary>
    /// <param name="Alpha"></param>
    /// <returns></returns>
    async Task Fade(float Alpha)
    {
        float start = canvas.alpha;
        for(float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            canvas.alpha = Mathf.Lerp(start, Alpha, (t * 1.1f) / fadeTime);
            await Task.Yield();
        }

        canvas.alpha = Alpha;

    }

    /// <summary>
    /// 逐渐黑屏
    /// </summary>
    /// <returns></returns>
    public async Task FadeIn()
    {
        await Fade(1);
    }

    /// <summary>
    /// 逐渐从黑屏中恢复；
    /// </summary>
    /// <returns></returns>
    public async Task FadeOut()
    {
        await Fade(0);
    }

}
