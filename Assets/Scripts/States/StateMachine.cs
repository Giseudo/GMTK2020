public class StateMachine {
    public State CurrentState { get; private set; }

    public void Initialize(State startingState) {
        CurrentState = startingState;
        startingState.Enter(null);
    }

    public void ChangeState(State newState) {
        CurrentState.Exit(newState);

        newState.Enter(CurrentState);
        CurrentState = newState;
    }
}