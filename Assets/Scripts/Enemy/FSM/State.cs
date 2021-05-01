using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
abstract public class State
{    
    protected Enemy enemy;
    protected StateMachine stateMachine;
    protected EnemyAnimation ani;
    protected NavMeshAgent navMeshAgent;    

    //»ý¼ºÀÚ
    protected State(Enemy enemy, StateMachine stateMachine, EnemyAnimation ani, NavMeshAgent navMeshAgent)
    {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
        this.ani = ani;
        this.navMeshAgent = navMeshAgent;
    }

    public virtual void Enter()
    {

    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Exit()
    {

    }
}
