using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AttackState : State
{
    float timer;
    public AttackState(Enemy enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();        
        enemy.InitializeAll();
        enemy.PlayAttAnimation();      //���� �ִϸ��̼� ���              
        enemy.SetNavMeshAgent(false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        timer += Time.deltaTime;    //Ÿ�̸ӷ� ������ �ο�
        if (timer > 1.5f)
        {
            timer = 0.0f;
            enemy.PlayAttAnimation();
            enemy.ChangeToIdle();
        }
    }

    public override void Exit()
    {
        base.Exit();        
    }
}
