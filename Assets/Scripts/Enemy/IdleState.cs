using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IdleState : State
{
    float timer;
    public IdleState(Enemy enemy, StateMachine stateMachine, EnemyAnimation ani) : base(enemy, stateMachine, ani)
    { 
    }
        
    public override void Enter()
    {
        base.Enter();
        enemy.InitializeVar();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        timer += Time.deltaTime;
        if (timer > 5f)
        {
            timer = 0.0f;
            enemy.navMeshAgent.isStopped = false;
            stateMachine.ChangeState(enemy.patrol);
        }
    }
}
