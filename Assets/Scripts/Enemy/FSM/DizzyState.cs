using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class DizzyState : State
{
    float timer;
    public DizzyState(Enemy enemy) : base(enemy)
    {
    }    

    public override void Enter()
    {
        base.Enter();        
        enemy.InitializeAll();  //변수 초기화
        enemy.PlayDizzyAnimation();    //Dizzy 애니메이션 실행
        enemy.SetNavMeshAgent();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        timer += Time.deltaTime;    //타이머로 딜레이 부여
        if (timer > 4f)
        {
            timer = 0.0f;
            enemy.StopDizzyAnimation();
            enemy.ChangeToIdle();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }   
}
