using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Object_Test : MonoBehaviour
{
    
    //check enemy has Path
    Enemy_Chase Check_HasP;
    Animator ani;
    void Start()
    {
        ani = GetComponent<Animator>();
        Check_HasP = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy_Chase>();
    }

    // 경로가 있으면 애니메이션 시작.
    void Update()
    {
        if (Check_HasP.hasP)
        {
            ani.SetBool("Walk", true);
        }
        else
        {
            ani.SetBool("Walk", false);
        }
    }
}
