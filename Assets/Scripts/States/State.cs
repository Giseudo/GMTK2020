public abstract class State {
    protected Cat cat;
    protected StateMachine stateMachine;

    protected State(Cat cat, StateMachine stateMachine) {
        this.cat = cat;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter(State previousState) { }
    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void Exit(State nextState) { }
}