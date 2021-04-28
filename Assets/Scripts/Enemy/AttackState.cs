using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AttackState : State
{
    public AttackState(Enemy enemy, StateMachine stateMachine, EnemyAnimation ani) : base(enemy, stateMachine, ani)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        enemy.siren.Stop();
        //NavMeshAgent ¿·Ω√ ∏ÿ√„
        enemy.navMeshAgent.isStopped = true;
        //≈∏∞Ÿ √ ±‚»≠
        enemy.visibleTargets.Clear();
        ani.PlayAttAnim();
        stateMachine.ChangeState(enemy.idle);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();        
    }

}
