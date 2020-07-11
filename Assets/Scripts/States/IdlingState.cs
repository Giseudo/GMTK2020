using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlingState : State {
    float elapsedTime = 0f;
    public IdlingState (Cat cat, StateMachine stateMachine) : base(cat, stateMachine) { }

    public override void Enter (State previousState) {
        base.Enter(previousState);

        elapsedTime = 0f;
    }

    public override void LogicUpdate () {
        base.LogicUpdate();

        Idle();
    }

    public void Idle () {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= 2f)
            stateMachine.ChangeState(cat.walking);
    }
}