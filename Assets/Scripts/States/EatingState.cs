using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingState : State {
    Transform bowl;

    public EatingState (Cat cat, StateMachine stateMachine, Transform bowl) : base(cat, stateMachine) {
        this.bowl = bowl;
    }

    public override void Enter(State previousState) {
        base.Enter(previousState);
    }

    public override void LogicUpdate () {
        base.LogicUpdate();
        StayAtBowl();
    }

    void StayAtBowl () {
        Vector3 position = bowl.position;

        position -= (bowl.position - cat.transform.position).normalized * .5f;

        cat.agent.destination = position;
    }
}
