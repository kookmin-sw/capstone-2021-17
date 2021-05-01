using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class DizzyState : State
{
    public DizzyState(Enemy enemy, StateMachine stateMachine, EnemyAnimation ani, NavMeshAgent navMeshAgent) : base(enemy, stateMachine, ani, navMeshAgent)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.siren.Stop();    //사이렌 멈춤        

        enemy.InitializeVar();  //변수 초기화
        enemy.visibleTargets.Clear();   //타겟 초기화

        ani.PlayDizzyAnim();    //Dizzy 애니메이션 실행
        stateMachine.ChangeState(enemy.idle);   //스테이트 변경
    }

    public override void Exit()
    {
        base.Exit();
    }
   
}
