using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIFadeInOut : MonoBehaviour
{

    public static UIFadeInOut instance;

    [SerializeField]
    private Image FadeImg;
    private void Awake()
    {
        instance = this;
    }

    float fadeoutTime;
    float fadeInTime;
    float waitTime;

    public UnityEvent OnFadeOut;
    public UnityEvent OnFadeIn;

    public void FadeoutandIn(float fadeoutTime, float fadeInTime, float waitTime)
    {
        if (isPlaying == true)
        {
            return;
        }
        this.fadeoutTime = fadeoutTime;
        this.fadeInTime = fadeInTime;
        this.waitTime = waitTime;

        StartCoroutine("fadeplay");
    }

    bool isPlaying = false;

    IEnumerator fadeplay()
    {
        
        isPlaying = true;

        float time = 0f;

        while (FadeImg.color.a< 1f)

        {
            time += Time.deltaTime / fadeoutTime;

            float alpha = Mathf.Lerp(0, 1, time);

            Color c = FadeImg.color;
            FadeImg.color = new Color(c.r, c.g, c.b, alpha);

            yield return null;
        }
        OnFadeOut.Invoke();

        yield return new WaitForSeconds(waitTime);

        time = 0f;

        while(FadeImg.color.a > 0f)
        {
            time += Time.deltaTime / fadeoutTime;

            float alpha = Mathf.Lerp(1, 0, time);

            Color c = FadeImg.color;
            FadeImg.color = new Color(c.r, c.g, c.b, alpha);

            yield return null;
        }

        OnFadeIn.Invoke();

        isPlaying = false;
    }

}
