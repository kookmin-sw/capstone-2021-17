using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class DizzyState : State
{
    public DizzyState(Enemy enemy, EnemyAnimation anim) : base(enemy, anim)
    {
    }    

    public override void Enter()
    {
        base.Enter();
        enemy.SirenStop();
        enemy.InitializeAll();  //변수 초기화        
        enemy.ChangeToIdle();
        anim.PlayDizzyAnim();    //Dizzy 애니메이션 실행
    }

    public override void Exit()
    {
        base.Exit();
    }
   
}
