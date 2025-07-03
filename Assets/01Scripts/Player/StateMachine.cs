using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public class StateMachine
{
    public BaseState CurrentState { get; private set; }
    private Dictionary<PLAYER_STATE, BaseState> states = new();

    public StateMachine(PLAYER_STATE defaultState, BaseState initialState)
    {
        AddState(defaultState, initialState);
        CurrentState = initialState;
        CurrentState.OnEnterState();
    }

    public void AddState(PLAYER_STATE stateName, BaseState state)
    {
        if (!states.ContainsKey(stateName))
            states[stateName] = state;
    }

    public void ChangeState(PLAYER_STATE nextState)
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

    public BaseState GetState(PLAYER_STATE state)
    {
        states.TryGetValue(state, out var result);
        return result;
    }

}
