using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DizzyState : State
{
    public DizzyState(Enemy enemy, StateMachine stateMachine, EnemyAnimation ani) : base(enemy, stateMachine, ani)
    {
    }

    public override void Enter()
    {
        base.Enter();
        siren = enemy.siren;
        enemy.InitializeVar();
        siren.Stop();
        enemy.visibleTargets.Clear();
        ani.PlayDizzyAnim();
        stateMachine.ChangeState(enemy.idle);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();       
    }

}
