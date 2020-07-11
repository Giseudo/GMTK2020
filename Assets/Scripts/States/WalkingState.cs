using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : State {
    Vector3 position;

    public WalkingState (Cat cat, StateMachine stateMachine) : base(cat, stateMachine) { }

    public override void Enter (State previousState) {
        base.Enter(previousState);

        position = RandomPosition();
    }

    public override void LogicUpdate () {
        base.LogicUpdate();

        Walk();
        ReachTarget();
    }

    void Walk() {
        cat.agent.destination = position;
    }

    void ReachTarget() {
        float distance = (cat.transform.position - position).magnitude;

        if (distance <= .5f)
            stateMachine.ChangeState(cat.idling);
    }

    Vector3 RandomPosition(float distance = 1f) {
        Vector2 direction = Random.insideUnitCircle.normalized;
        Vector3 position = cat.transform.position + new Vector3(direction.x, 0f, direction.y) * distance;

        return position;
    }
}