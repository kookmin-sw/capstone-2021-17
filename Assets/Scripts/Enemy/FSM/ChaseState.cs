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
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        enemy.FindTargets();
        enemy.MoveToTarget();
        enemy.ChangeToAttack();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.SetHasDestination(false);
    }
}
