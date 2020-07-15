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

        cat.walking = false;
    }

    public override void LogicUpdate () {
        base.LogicUpdate();

        ReachTarget();
        Search();

        if (cat.scaredAt != Vector3.zero && !cat.IsStarving)
            stateMachine.ChangeState(new FrighteningState(cat, stateMachine));
    }

    void Walk() {
        cat.agent.speed = cat.data.walkSpeed;
        cat.agent.destination = position;
        cat.walking = true;
    }

    void ReachTarget() {
        float distance = (cat.transform.position - position).magnitude;

        if ((startTime + 2f < Time.unscaledTime) || distance <= .5f)
            stateMachine.ChangeState(new IdlingState(cat, stateMachine));
    }

    void Search() {
        foreach (var collider in cat.Search(3f)) {
            if (collider.tag == "YarnBall" && !cat.IsStarving)
                stateMachine.ChangeState(new PlayingState(cat, stateMachine, collider.transform));

            if (collider.tag == "Snack" && cat.IsHungry)
                stateMachine.ChangeState(new ChasingState(cat, stateMachine, collider.transform));
        }
    }
}