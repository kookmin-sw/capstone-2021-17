using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{                
    //check enemy has Path
    [SerializeField]
    private Enemy checkAni;
    private Animator ani;    
    [SerializeField]
    private EnemyNetBehaviour enemyNet;
    void Awake()
    {
        ani = GetComponent<Animator>();
    }        

    public void SetBlnedTree(float runAmount)
    {
        ani.SetFloat("Blend", runAmount);
    }

    public void SetWalk()
    {
        ani.SetBool("Walk", true);
    }

    public void PlayWalkAnim()
    {
        if (enemyNet != null)
        {
            enemyNet.SetWalk();
        }
        else
        {
            SetWalk();
        }
    }
    public void StopWalkAnim()
    {
        if (enemyNet != null)
        {
            enemyNet.UnsetWalk();
        }
        else
        {
            UnsetWalk();
        }
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
        ani.SetBool("Attack", ani.GetBool("Attack")? false:true);
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

    public void StopDizzyAnim()
    {
        if (enemyNet != null)
        {
            enemyNet.UnsetDizzyAnim();
        }
        else
        {
            UnsetDizzyAnim();
        }
    }
    
    public void SetDizzyAnim()
    {
        ani.SetBool("Attack", false);
        ani.SetBool("Dizzy", true);
    }

    public void UnsetDizzyAnim()
    {
        ani.SetBool("Attack", false);
        ani.SetBool("Dizzy", false );
    }
}
