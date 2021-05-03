using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ChaseState : State
{
    public ChaseState(Enemy enemy) : base(enemy)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        enemy.InitializeAll();          //변수 초기화
        enemy.hasDestination = true;    //걷는 애니메이션 실행
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        enemy.SirenPlay();
        enemy.FindTargets();
        enemy.MoveToTarget();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
