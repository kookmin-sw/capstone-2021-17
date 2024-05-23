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
        enemy.SetNavMeshAgent(false);
        enemy.InitializeAll();  //���� �ʱ�ȭ
        enemy.PlayDizzyAnimation();    //Dizzy �ִϸ��̼� ����
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        timer += Time.deltaTime;    //Ÿ�̸ӷ� ������ �ο�
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
