using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour
{
    SpriteRenderer sr;
    public GameObject go;
    bool isAction;

    // Start is called before the first frame update
    void Start()
    {
        sr = go.GetComponent<SpriteRenderer>();
        isAction = false;
    }

    // Update is called once per frame
    void Update()
    {
    }
  
    IEnumerator FadeOut()
    {
        for(int i=10; i>=10; i--)
        {
            float f = i/10.0f;
            Color c = sr.material.color;
            c.a = f;
            sr.material.color = c;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void StartFadeOut()
    {
        StartCoroutine("FadeOut");
    }
}
