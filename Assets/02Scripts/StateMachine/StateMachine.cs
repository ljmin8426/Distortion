using System;
using System.Collections.Generic;

public class StateMachine<TStateName, TOwner>
    where TStateName : Enum
    where TOwner : class
{
    public BaseState<TOwner> CurrentState { get; private set; }
    public TStateName CurrentStateName { get; private set; }

    private readonly Dictionary<TStateName, BaseState<TOwner>> states = new();

    public StateMachine(TStateName startStateName, BaseState<TOwner> startState)
    {
        AddState(startStateName, startState);
        CurrentState = startState;
        CurrentStateName = startStateName;
    }

    public void UpdateState()
    {
        CurrentState?.OnUpdateState();
    }

    public void FixedUpdateState()
    {
        CurrentState?.OnFixedUpdateState();
    }

    public void AddState(TStateName stateName, BaseState<TOwner> state, bool overwrite = false)
    {
        if (states.ContainsKey(stateName))
        {
            if (overwrite)
                states[stateName] = state;
            else
                return;
        }
        else
        {
            states.Add(stateName, state);
        }
    }

    public BaseState<TOwner> GetState(TStateName stateName)
    {
        states.TryGetValue(stateName, out var state);
        return state;
    }

    public void ChangeState(TStateName nextStateName)
    {
        if (EqualityComparer<TStateName>.Default.Equals(CurrentStateName, nextStateName))
            return;

        if (!states.TryGetValue(nextStateName, out var nextState))
            return;

        if (!nextState.CanEnter())
            return;

        CurrentState?.OnExitState();
        CurrentState = nextState;
        CurrentStateName = nextStateName;
        CurrentState?.OnEnterState();
    }

    public void DeleteState(TStateName stateName)
    {
        if (!states.TryGetValue(stateName, out var state))
            return;

        if (state == CurrentState)
        {
            CurrentState = null;
            CurrentStateName = default;
        }

        states.Remove(stateName);
    }
}
