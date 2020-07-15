using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrighteningState : State {
    float startTime;
    public FrighteningState (Cat cat, StateMachine stateMachine) : base(cat, stateMachine) { }

    public override void Enter (State previousState) {
        base.Enter(previousState);

        if (cat.scaredAt == Vector3.zero)
            stateMachine.ChangeState(new IdlingState(cat, stateMachine));

        startTime = Time.unscaledTime;
        SoundManager.Instance.Play("Angry", .2f);

        Flee((cat.transform.position -cat. scaredAt) * 3f);
    }

    public override void Exit (State nextState) {
        base.Exit(nextState);

        cat.scaredAt = Vector3.zero;
    }

    public override void LogicUpdate () {
        base.LogicUpdate();

        if (startTime + 1f < Time.unscaledTime)
            stateMachine.ChangeState(new IdlingState(cat, stateMachine));
    }

    void Flee (Vector3 position) {
        cat.agent.speed = cat.data.walkSpeed + 5f;
        cat.agent.destination = position;
    }
}