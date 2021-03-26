using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectTest : MonoBehaviour
{
    
    //check enemy has Path
    [SerializeField]
    private EnemyChase checkAnim;
    Animator ani;
    void Start()
    {
        ani = GetComponent<Animator>();
    }

    // 경로가 있으면 애니메이션 시작.
    void Update()
    {       
        if (checkAnim.hasP)
        {
            ani.SetBool("Walk", true);
        }
        else
        {
            ani.SetBool("Walk", false);
        }       
    }

    public void PlayAttAnim()
    {
        ani.SetTrigger("Attack");
    }
}
