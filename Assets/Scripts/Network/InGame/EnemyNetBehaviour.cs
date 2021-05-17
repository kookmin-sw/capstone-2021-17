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

    [SerializeField]
    Enemy enemy;

    [ServerCallback]
    public void SetWalk()
    {
        enemyAnimation.SetWalk();
    }

    [ServerCallback]
    public void UnsetWalk()
    {
        enemyAnimation.UnsetWalk();
    }

    [ServerCallback]
    public void SetAttAnim()
    {
        enemyAnimation.SetAttAnim();        
    }

    [ServerCallback]
    public void SetDizzyAnim()
    {
        enemyAnimation.SetDizzyAnim();
    }

    public void SirenPlay()
    {
        RpcSirenPlay(); 
    }

    

    [ClientRpc]
    void RpcSirenPlay()
    {
        enemy.SirenPlaySync();
        Debug.Log("RPCSIRENPLAY");
    }

    public void SirenStop()
    {
        RpcSirenStop();
    }

    [ClientRpc]
    void RpcSirenStop()
    {
        enemy.SirenStopSync();
    }


}
