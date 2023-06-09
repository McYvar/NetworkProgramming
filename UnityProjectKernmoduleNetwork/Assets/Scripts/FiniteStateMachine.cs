using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine
{
    private Dictionary<System.Type, BaseState> stateDictionary = new Dictionary<System.Type, BaseState>();

    private BaseState currentState;
    private BaseState lastState;

    public FiniteStateMachine(BaseState startState, BaseState[] states)
    {
        foreach(BaseState state in states)
        {
            state.Initialize(this);
            state.Init();
            stateDictionary.Add(state.GetType(), state);
        }

        currentState = stateDictionary[startState.GetType()];
    }

    public void OnUpdate()
    {
        currentState?.OnUpdate();
    }

    public void OnFixedUpdate()
    {
        currentState?.OnFixedUpdate();
    }

    public void OnLateUpdate()
    {
        currentState?.OnLateUpdate();
    }

    public void SwitchState(System.Type newStateStype)
    {
        currentState?.OnExit();
        lastState = currentState;
        currentState = stateDictionary[newStateStype];
        currentState?.OnEnter();
    }

    public void DebugCurrentState()
    {
        Debug.Log(currentState);
    }

    public BaseState GetLastState()
    {
        return lastState;
    }
}
