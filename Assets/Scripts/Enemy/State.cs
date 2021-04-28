using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

abstract public class State
{
    protected Enemy enemy;
    protected StateMachine stateMachine;
    protected EnemyAnimation ani;
    
    protected State(Enemy enemy, StateMachine stateMachine, EnemyAnimation ani)
    {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
        this.ani = ani;
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
