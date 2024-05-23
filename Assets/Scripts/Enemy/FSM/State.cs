using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
abstract public class State
{    
    protected Enemy enemy;
    protected EnemyAnimation anim;
    //»ý¼ºÀÚ
    protected State(Enemy enemy)
    {
        this.enemy = enemy;
        
    }
    
    protected State(Enemy enemy, EnemyAnimation anim) : this(enemy)
    {
        this.anim = anim;
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
