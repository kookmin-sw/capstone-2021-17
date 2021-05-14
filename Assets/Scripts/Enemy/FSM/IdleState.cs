using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class IdleState : State
{
    float timer;

    public IdleState(Enemy enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.InitializeAll();  //변수 초기화
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        timer += Time.deltaTime;    //타이머로 딜레이 부여      
        if (timer > 2f && enemy.IsLatestStateAtt() )
        {
            enemy.SetCollider();
            enemy.MemoState();            
        }
        if (timer > 5f)
        {
            /*if (enemy.IsLatestStateAtt() || enemy.IsLatestStateDizzy())
            {
                enemy.SetNavMeshAgent();
            }*/
            timer = 0.0f;            
            enemy.ChangeToPatrol();
        }
    }

    public override void Exit()
    {
        base.Exit();
        
    }
}
