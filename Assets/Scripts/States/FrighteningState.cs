using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrighteningState : State {
    float startTime;
    public Vector3 scaredAtPosition;
    public FrighteningState (Cat cat, StateMachine stateMachine) : base(cat, stateMachine) { }

    public override void Enter (State previousState) {
        base.Enter(previousState);
        startTime = Time.unscaledTime;
        SoundManager.Instance.Play("Angry", .2f);

        Flee((cat.transform.position - scaredAtPosition) * 3f);
    }

    public override void LogicUpdate () {
        base.LogicUpdate();

        if (startTime + 1f < Time.unscaledTime)
            stateMachine.ChangeState(cat.walking);
    }

    void Flee (Vector3 position) {
        cat.agent.speed = cat.data.walkSpeed + 5f;
        cat.agent.destination = position;
    }
}