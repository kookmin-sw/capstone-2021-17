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

    //스테이트 초기화
    public void Initialize(State startingState)
    {
        currentState = startingState;
        startingState.Enter();
    }

    //스테이트 변경
    public void ChangeState(State newState)
    {
        currentState.Exit();
        memState = currentState;
        currentState = newState;
        newState.Enter();
    }
}
