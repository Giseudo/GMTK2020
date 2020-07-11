using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlingState : State {
    public IdlingState (Cat cat, StateMachine stateMachine) : base(cat, stateMachine) { }

    public override void LogicUpdate () {
        base.LogicUpdate();
    }
}