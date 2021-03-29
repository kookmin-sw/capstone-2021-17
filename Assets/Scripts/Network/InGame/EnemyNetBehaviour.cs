using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnemyNetBehaviour : NetworkBehaviour
{
    [SerializeField]
    EnemyAnimation enemyAnimation;

    [SerializeField]
    EnemyChase enemyChase;

    [SerializeField]
    NetworkAnimator netAni;

    [Server]
    public void SetWalk()
    {
        enemyAnimation.SetWalk();
    }

    [Server]
    public void UnSetWalk()
    {
        enemyAnimation.UnsetWalk();
    }

    [Server]
    public void SetAttAnim()
    {
        netAni.SetTrigger("Attack");
    }

    [Server]
    public void SetDizzyAnim()
    {
        netAni.SetTrigger("Dizzy");
    }


    
}
