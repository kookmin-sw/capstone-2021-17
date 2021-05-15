using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingPlayerManager : MonoBehaviour
{//엔딩씬의 캐릭터 움직임 제어
    private Animator ending_anim;
    Vector3 target;

    void Start()
    {
        ending_anim.SetBool("isDead", false);
    }

    void Awake()
    {
        ending_anim = GetComponent<Animator>();
    }

    public void isLive()
    {//엔딩 시 살아있을 경우 모션
        ending_anim.SetBool("isDead", false);
        target = new Vector3(0, 0, -6.4f);
        //transform.Translate(new Vector3(0, 0.4f, 0));
        Debug.Log("click live");
    }

    public void isDead()
    {//엔딩 시 죽어있을 경우 모션
        target = new Vector3(0, -0.4f, -6.4f);
        ending_anim.SetBool("isDead", true);
        transform.Translate(new Vector3(0, -0.4f, 0));
        Debug.Log("click dead");
    }
}
