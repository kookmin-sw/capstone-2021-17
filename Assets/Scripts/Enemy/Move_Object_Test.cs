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
        Check_HasP = GameObject.FindGameObjectWithTag("Player").GetComponent<Enemy_Chase>();
    }

    // Update is called once per frame
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
