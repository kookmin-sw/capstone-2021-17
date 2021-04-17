using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{    
    //check enemy has Path
    [SerializeField]
    private EnemyControl checkAnim;
    Animator ani;

    [SerializeField]
    private EnemyNetBehaviour enemyNet;
    void Awake()
    {
        ani = GetComponent<Animator>();
    }

    // 경로가 있으면 애니메이션 시작.
    void Update()
    {       
        if (checkAnim.hasDestination)
        {
            if(enemyNet != null)
            {
                enemyNet.SetWalk();
            }
            else
            {
                SetWalk();
            }
        }
        else
        {
            if(enemyNet != null)
            {
                enemyNet.UnSetWalk();
            }
            else
            {
                UnsetWalk();
            }            
        }       
    }

    public void SetWalk()
    {
        ani.SetBool("Walk", true);
    }

    public void UnsetWalk()
    {
        ani.SetBool("Walk", false);
    }

    public void PlayAttAnim()
    {
        if (enemyNet != null)
        {
            enemyNet.SetAttAnim();
        }
        else
        {
            SetAttAnim();
        }
    }
    public void SetAttAnim()
    {
        ani.SetTrigger("Attack");
    }
    public void PlayDizzyAnim()
    {
        if(enemyNet != null)
        {
            enemyNet.SetDizzyAnim();
        }
        else
        {
            SetDizzyAnim();
        }
    }
    
    public void SetDizzyAnim()
    {
        ani.SetTrigger("Dizzy");
    }
}
