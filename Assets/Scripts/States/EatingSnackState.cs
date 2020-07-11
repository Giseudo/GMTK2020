using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingSnackState : State {
    public EatingSnackState (Cat cat, StateMachine stateMachine) : base(cat, stateMachine) { }

    public override void Enter(State previousState) {
        base.Enter(previousState);
    }

    public override void LogicUpdate () {
        base.LogicUpdate();
    }
}
