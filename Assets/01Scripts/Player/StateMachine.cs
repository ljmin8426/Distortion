using System;
using System.Collections.Generic;

public class StateMachine<TState, TController>
    where TState : Enum
    where TController : class
{
    public BaseState<TController> CurrentState { get; private set; }
    private Dictionary<TState, BaseState<TController>> states = new();

    public StateMachine(TState defaultState, BaseState<TController> initialState)
    {
        AddState(defaultState, initialState);
        CurrentState = initialState;
        CurrentState.OnEnterState();
    }

    public void AddState(TState stateName, BaseState<TController> state)
    {
        if (!states.ContainsKey(stateName))
            states[stateName] = state;
    }

    public void ChangeState(TState nextState)
    {
        if (!states.TryGetValue(nextState, out var newState))
            return;

        CurrentState?.OnExitState();
        CurrentState = newState;
        CurrentState?.OnEnterState();
    }

    public void UpdateState()
    {
        CurrentState?.OnUpdateState();
    }

    public void FixedUpdateState()
    {
        CurrentState?.OnFixedUpdateState();
    }

    public BaseState<TController> GetState(TState state)
    {
        states.TryGetValue(state, out var result);
        return result;
    }
}
