using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnemyNetBehaviour : NetworkBehaviour
{
    [SerializeField]
    EnemyAnimation enemyAnimation;

    [SerializeField]
    NetworkAnimator netAni;

    [Server]
    public void SetWalk()
    {
        enemyAnimation.SetWalk();
    }

    [Server]
    public void UnsetWalk()
    {
        enemyAnimation.UnsetWalk();
    }

    [Server]
    public void SetAttAnim()
    {
        enemyAnimation.SetAttAnim();        
    }

    [Server]
    public void SetDizzyAnim()
    {
        enemyAnimation.SetDizzyAnim();
    }

}
