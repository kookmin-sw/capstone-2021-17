using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ChaseState : State
{
    AudioSource siren;
    public ChaseState(Enemy enemy, StateMachine stateMachine, EnemyAnimation ani, NavMeshAgent navMeshAgent) : base(enemy, stateMachine, ani, navMeshAgent)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        siren = enemy.siren;    //사이렌 설정
        enemy.InitializeVar();  //변수 초기화
        enemy.turnOnSensor = true;  //센서 온
        enemy.hasDestination = true;    //걷는 애니메이션 실행
        navMeshAgent.speed += navMeshAgent.speed * 0.005f;  //속도 지속적 증가
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!siren.isPlaying)   //사이렌이 울리는 중이 아니면 실행
        {
            siren.Play();
        }
        
        navMeshAgent.SetDestination(enemy.target.position);    //타겟으로 이동
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
