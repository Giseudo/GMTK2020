using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingState : State {
    Bowl ClosestBowl => BowlManager.GetClosestBowl(cat.transform.position);

    public EatingState (Cat cat, StateMachine stateMachine) : base(cat, stateMachine) { }

    public override void Enter(State previousState) {
        base.Enter(previousState);
    }

    public override void LogicUpdate () {
        base.LogicUpdate();
        StayAtBowl();
    }


    void StayAtBowl () {
        Vector3 position = ClosestBowl.transform.position;

        position -= (position - cat.transform.position).normalized * .5f;

        cat.agent.destination = position;
    }
}
