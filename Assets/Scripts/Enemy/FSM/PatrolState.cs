using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PatrolState : State
{
    public Vector3 patrolPos;
    private int randomIndex;
    System.Random random = new System.Random();
    public PatrolState(Enemy enemy, StateMachine stateMachine, EnemyAnimation ani, NavMeshAgent navMeshAgent) : base(enemy, stateMachine, ani, navMeshAgent)
    {
    }
    public float DistanceXZ(Vector3 posFirst, Vector3 posSecond)
    {
        posFirst.y = 0.0f;
        posSecond.y = 0.0f;

        return Vector3.Distance(posFirst, posSecond);
    }

    public override void Enter()
    {
        base.Enter();
        enemy.InitializeVar();  //변수 초기화
        enemy.turnOnSensor = true;  //센서 온
        enemy.hasDestination = true;    //걷는 애니메이션 실행
        navMeshAgent.speed = 0.5f;  // 속도 초기화
        randomIndex = random.Next() % enemy.wayPoint.Length;    //난수 설정
        patrolPos = enemy.wayPoint[randomIndex].position;   //순찰 포인트 설정
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        navMeshAgent.SetDestination(patrolPos); //순찰 포인트로 이동

        //일정 거리에 들어오면 다시 순찰 포인트 설정
        if (DistanceXZ(enemy.transform.position, patrolPos) <= enemy.minErrorWayPoint)
        {                        
            stateMachine.ChangeState(enemy.patrol); //스테이트 변경
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }
}
