using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingState : State {
    public ChasingState (Cat cat, StateMachine stateMachine) : base(cat, stateMachine) { }

    public override void LogicUpdate () {
        base.LogicUpdate();
    }
}