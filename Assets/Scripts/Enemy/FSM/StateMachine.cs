using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine 
{
    public State currentState { get; private set; }

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

        currentState = newState;
        newState.Enter();
    }
}
