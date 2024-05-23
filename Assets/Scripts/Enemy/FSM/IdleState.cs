using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class IdleState : State
{
    float timer;

    public IdleState(Enemy enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.InitializeAll();  //���� �ʱ�ȭ      
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        timer += Time.deltaTime;    //Ÿ�̸ӷ� ������ �ο�      
        if (timer > 2f && enemy.IsLatestStateAtt() )
        {
            enemy.SetCollider();
            enemy.MemoState();
        }
        if (timer > 5f)
        {
            timer = 0.0f;
            if (enemy.IsLatestStateAtt() || enemy.IsLatestStateDizzy())
            {
                enemy.SetNavMeshAgent(true);                
            }            
            enemy.ChangeToPatrol();
        }
    }

    public override void Exit()
    {
        base.Exit();        
    }
}
