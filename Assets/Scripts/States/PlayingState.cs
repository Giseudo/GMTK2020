using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingState : State {
    public PlayingState (Cat cat, StateMachine stateMachine) : base(cat, stateMachine) { }

    public override void LogicUpdate () {
        base.LogicUpdate();
    }
}