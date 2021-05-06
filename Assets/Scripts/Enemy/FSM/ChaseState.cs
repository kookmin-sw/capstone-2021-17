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
        enemy.SirenPlay();
        enemy.SetHasDestination(true);
        /*enemy.InitializeAll();          //변수 초기화
        enemy.hasDestination = true;    //걷는 애니메이션 실행*/
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
        enemy.MoveToTarget();
        enemy.ChangeToAttack();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
