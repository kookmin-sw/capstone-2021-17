using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PatrolState : State
{    
    private float minErrorWayPoint = 0.5f;   //���� �����Ÿ��� �ּ� ����
    public PatrolState(Enemy enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.InitializeAll();          
        enemy.PlayWalkAnimation();
        enemy.MoveToWayPoint();        
    }

    public override void Exit()
    {
        base.Exit();
        enemy.StopWalkAnimation();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        enemy.FindTargets();
        //���� �Ÿ��� ������ �ٽ� ���� ����Ʈ ����
        if (enemy.DistanceXZ() <= minErrorWayPoint)
        {
            enemy.ChangeToPatrol();
        }
    }
}
