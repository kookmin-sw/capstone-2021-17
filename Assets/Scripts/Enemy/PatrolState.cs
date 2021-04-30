using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    public PatrolState(Enemy enemy, StateMachine stateMachine, EnemyAnimation ani) : base(enemy, stateMachine, ani)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.InitializeVar();
        enemy.navMeshAgent.speed = 0.5f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!enemy.isPatrol)
        {            
            //순찰중인지 판단
            enemy.isPatrol = true;
            enemy.turnOnSensor = true;
            //웨이 포인트 중 하나를 랜덤으로 접근
            int random = Random.Range(0, enemy.wayPoint.Length);
            enemy.patrolPos = enemy.wayPoint[random].position;
            //순찰 시작
            enemy.navMeshAgent.SetDestination(enemy.patrolPos);
            stateMachine.ChangeState(enemy.move);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }
}
