using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine 
{
    private State memState;
    public State currentState 
    {
        get; 
        private set; 
    }
    public State latestState 
    { 
        get 
        {
            return memState;
        }
        set 
        {
            memState = currentState;
        }        
    }

    //������Ʈ �ʱ�ȭ
    public void Initialize(State startingState)
    {
        currentState = startingState;
        startingState.Enter();
    }

    //������Ʈ ����
    public void ChangeState(State newState)
    {
        currentState.Exit();
        memState = currentState;
        currentState = newState;
        newState.Enter();
    }
}
