using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlingState : State {
    float startTime;
    public IdlingState (Cat cat, StateMachine stateMachine) : base(cat, stateMachine) { }

    public override void Enter (State previousState) {
        base.Enter(previousState);

        startTime = Time.unscaledTime;
    }

    public override void LogicUpdate () {
        base.LogicUpdate();

        Search();
        Idle();

        if (cat.scaredAt != Vector3.zero && !cat.IsStarving)
            stateMachine.ChangeState(new FrighteningState(cat, stateMachine));
    }

    public void Idle () {
        if (startTime + 2f < Time.unscaledTime)
            stateMachine.ChangeState(new WalkingState(cat, stateMachine));
    }

    public void Search() {
        if (cat.IsHungry) {
            Bowl bowl = cat.ClosestBowl;

            if (bowl != null)  {
                stateMachine.ChangeState(new ChasingState(cat, stateMachine, bowl.transform));
                return;
            }
        }

        foreach (var collider in cat.Search(3f)) {
            if (collider.tag == "YarnBall" && !cat.IsStarving)
                stateMachine.ChangeState(new PlayingState(cat, stateMachine, collider.transform));

            if (collider.tag == "Snack" && cat.IsHungry)
                stateMachine.ChangeState(new ChasingState(cat, stateMachine, collider.transform));
        }
    }
}