using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AttackState : State
{    
    public AttackState(Enemy enemy, EnemyAnimation anim) : base(enemy, anim)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        enemy.InitializeAll();  //변수 초기화        
        enemy.SirenStop();
        enemy.ControlNavMesh();
        anim.PlayAttAnim();      //공격 애니메이션 재생
        enemy.ChangeToIdle();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
