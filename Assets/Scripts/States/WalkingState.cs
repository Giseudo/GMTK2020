using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkingState : State {
    Vector3 position;
    Vector3 debugPosition;
    float startTime;

    public WalkingState (Cat cat, StateMachine stateMachine) : base(cat, stateMachine) { }

    public override void Enter (State previousState) {
        base.Enter(previousState);
        startTime = Time.unscaledTime;

        position = CatManager.RandomPosition(cat.transform.position, 5f);
        Walk();
    }

    public override void Exit (State nextState) {
        base.Exit(nextState);

        startTime = 0f;
        cat.animator.SetBool("Walking", false);
    }

    public override void LogicUpdate () {
        base.LogicUpdate();

        ReachTarget();
    }

    void Walk() {
        cat.agent.speed = cat.data.walkSpeed;
        cat.animator.SetBool("Walking", true);
        cat.agent.destination = position;

        Debug.DrawLine(cat.transform.position, position, Color.yellow);
    }

    void ReachTarget() {
        float distance = (cat.transform.position - position).magnitude;

        if ((startTime > 0f && startTime + 3f < Time.unscaledTime) || distance <= .5f)
            stateMachine.ChangeState(cat.idling);
    }
}