using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{

    public MoveState(Enemy enemy, StateMachine stateMachine, EnemyAnimation ani) : base(enemy, stateMachine, ani)
    {
    }

    public override void Enter()
    {
        base.Enter();
        siren = enemy.siren;
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        //순찰중이면
        if (enemy.isPatrol)
        {
            //경로를 갖고있고
            enemy.hasDestination = true;
            //경로에 도달하면
            /*순찰 지점에 정확하게 도착하는 걸 조건으로 걸면 
             * 제대로 동작이 되지 않는 경우가 많아 
             * 오차 범위를 설정해서 순찰을 진행하도록 한다.
             */
            if (enemy.DistanceXZ(enemy.transform.position, enemy.patrolPos) <= enemy.minErrorWayPoint)
            {
                //초기화
                enemy.hasDestination = false;
                enemy.isPatrol = false;
                stateMachine.ChangeState(enemy.patrol);
            }
        }
        else
        {
            if (!siren.isPlaying)
            {
                siren.Play();
            }
            //계속해서 경로를 설정해서 플레이어가 움직여도 그 경로를 다시 설정한다.
            enemy.navMeshAgent.SetDestination(enemy.target.position);
            //타겟을 설정했으므로 타겟 설정 변수 초기화
            /* NavMesh위에 플레이어가 있지 않을때
            if (!enemy.hasPath)
            {
                state = State.Idle;
            }
            */
            
        }
    }

}
