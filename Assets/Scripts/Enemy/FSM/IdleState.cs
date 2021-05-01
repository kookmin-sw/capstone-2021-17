using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class IdleState : State
{
    float timer;
    public IdleState(Enemy enemy, StateMachine stateMachine, EnemyAnimation ani, NavMeshAgent navMeshAgent) : base(enemy, stateMachine, ani, navMeshAgent)
    { 
    }
        
    public override void Enter()
    {
        base.Enter();
        enemy.InitializeVar();  //변수 초기화
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        timer += Time.deltaTime;    //타이머로 딜레이 부여
        if (timer > 5f)
        {
            timer = 0.0f;
            navMeshAgent.isStopped = false; //NavMeshAgent 재실행
            stateMachine.ChangeState(enemy.patrol); //스테이트 변경
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
