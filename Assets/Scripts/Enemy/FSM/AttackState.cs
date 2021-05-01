using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AttackState : State
{
    public AttackState(Enemy enemy, StateMachine stateMachine, EnemyAnimation ani, NavMeshAgent navMeshAgent) : base(enemy, stateMachine, ani, navMeshAgent)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        
        enemy.InitializeVar();  //변수 초기화
        enemy.visibleTargets.Clear();   //타겟 초기화        
        enemy.siren.Stop(); //사이렌 멈춤        
        navMeshAgent.isStopped = true;  //NavMeshAgent 멈춤
        ani.PlayAttAnim();  //공격 애니메이션 재생
        stateMachine.ChangeState(enemy.idle);   //스테이트 변경
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
