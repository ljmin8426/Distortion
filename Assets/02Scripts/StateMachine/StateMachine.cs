using System;
using System.Collections.Generic;

public class StateMachine<TStateName, TOwner>
    where TStateName : Enum
    where TOwner : class
{
    public BaseState<TOwner> CurrentState { get; private set; }
    private Dictionary<TStateName, BaseState<TOwner>> states = new();

    public StateMachine(TStateName startStateName, BaseState<TOwner> startState)
    {
        AddState(startStateName, startState);
        CurrentState = startState;
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
        var nextState = GetState(nextStateName);

        // 상태가 없거나 현재 상태와 같으면 무시
        if (nextState == null || EqualityComparer<TStateName>.Default.Equals(GetStateName(CurrentState), nextStateName))
            return;

        // 전환 불가하면 무시
        if (!nextState.CanEnter())
            return;

        CurrentState?.OnExitState();
        CurrentState = nextState;
        CurrentState?.OnEnterState();
    }

    public void DeleteState(TStateName deleteStateName)
    {
        if (states.ContainsKey(deleteStateName))
        {
            states.Remove(deleteStateName);
        }
    }

    private TStateName GetStateName(BaseState<TOwner> state)
    {
        foreach (var kvp in states)
        {
            if (kvp.Value == state)
                return kvp.Key;
        }
        return default;
    }
}
