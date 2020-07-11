using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : State {
    public WalkingState (Cat cat, StateMachine stateMachine) : base(cat, stateMachine) { }

    public override void LogicUpdate () {
        base.LogicUpdate();
    }
}