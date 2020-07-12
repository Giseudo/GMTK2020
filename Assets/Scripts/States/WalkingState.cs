using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkingState : State {
    Vector3 position;
    Vector3 debugPosition;

    public WalkingState (Cat cat, StateMachine stateMachine) : base(cat, stateMachine) { }

    public override void Enter (State previousState) {
        base.Enter(previousState);

        position = CatManager.RandomPosition(cat.transform.position, 5f);
        Walk();
    }

    public override void Exit (State nextState) {
        base.Exit(nextState);

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

        if (distance <= .5f)
            stateMachine.ChangeState(cat.idling);
    }
}