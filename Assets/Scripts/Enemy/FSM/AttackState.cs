using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AttackState : State
{
    float timer;
    public AttackState(Enemy enemy, EnemyAnimation anim) : base(enemy, anim)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SirenStop();
        enemy.InitializeAll();
        anim.PlayAttAnim();      //공격 애니메이션 재생              
        enemy.SetNavMeshAgent();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        timer += Time.deltaTime;    //타이머로 딜레이 부여
        if (timer > 1.5f)
        {
            timer = 0.0f;
            anim.PlayAttAnim();
            enemy.ChangeToIdle();
        }
    }

    public override void Exit()
    {
        base.Exit();        
    }
}
