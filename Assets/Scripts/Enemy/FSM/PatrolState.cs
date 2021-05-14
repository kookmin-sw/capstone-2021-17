using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PatrolState : State
{    
    private float minErrorWayPoint = 0.5f;   //순찰 지점거리의 최소 오차
    public PatrolState(Enemy enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.InitializeAll();          //변수 초기화        
        enemy.SetHasDestination(true);
        enemy.MoveToWayPoint();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.SetHasDestination(false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();        
        enemy.FindTargets();
        //일정 거리에 들어오면 다시 순찰 포인트 설정
        if (enemy.DistanceXZ() <= minErrorWayPoint)
        {
            enemy.ChangeToPatrol();
        }
    }

}
