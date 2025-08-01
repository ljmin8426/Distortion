using System;
using System.Collections.Generic;

public class StateMachine<TStateName, TOwner>
    where TStateName : Enum
    where TOwner : class
{
    public BaseState<TOwner> CurrentState { get; private set; }
    private Dictionary<TStateName, BaseState<TOwner>> states = new();

    public StateMachine(TStateName stateName, BaseState<TOwner> state)
    {
        AddState(stateName, state);
        CurrentState = GetState(stateName);
    }

    public void UpdateState()
    {
        CurrentState?.OnUpdateState();
    }

    public void FixedUpdateState()
    {
        CurrentState?.OnFixedUpdateState();
    }

    public void AddState(TStateName stateName, BaseState<TOwner> state)
    {
        if (!states.ContainsKey(stateName))
        {
            states.Add(stateName, state);
        }
    }

    public BaseState<TOwner> GetState(TStateName stateName)
    {
        if (states.TryGetValue(stateName, out var state))
            return state;
        return null;
    }

    public void ChangeState(TStateName nextStateName)
    {
        CurrentState?.OnExitState();

        if (states.TryGetValue(nextStateName, out var newState))
        {
            CurrentState = newState;
        }

        CurrentState?.OnEnterState();
    }
    public void DeleteState(TStateName deleteStateName)
    {
        if(states.ContainsKey(deleteStateName))
        {
            states.Remove(deleteStateName);
        }
    }
}
