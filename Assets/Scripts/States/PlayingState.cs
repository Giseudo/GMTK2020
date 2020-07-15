using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingState : State {
    public Transform ball;
    public float reachTime = 0f;
    public float startTime = 0f;
    public PlayingState (Cat cat, StateMachine stateMachine, Transform target = null) : base(cat, stateMachine) {
        ball = target;
    }

    public override void Enter(State previousState) {
        base.Enter(previousState);

        cat.Attention();
        Run();
    }

    public override void Exit(State nextState) {
        base.Exit(nextState);

        reachTime = 0f;
        startTime = 0f;
        cat.agent.speed = cat.data.walkSpeed;
    }

    void Run() {
        startTime = Time.unscaledTime;
        cat.agent.destination = ball.position;
        cat.agent.speed += 2f;
        cat.walking = true;
    }

    public override void LogicUpdate () {
        base.LogicUpdate();

        if (cat.IsStarving || ball == null) {
            stateMachine.ChangeState(new IdlingState(cat, stateMachine));
            return;
        }

        if (reachTime == 0f && cat.agent.remainingDistance <= .5f) {
            cat.agent.destination = CatManager.RandomPosition(cat.transform.position, 2f);
            reachTime = Time.unscaledTime;
        }

        if (reachTime + 2f < Time.unscaledTime) {
            reachTime = 0f;
            cat.agent.destination = ball.position;
        }

        if (startTime + 10f < Time.unscaledTime)
            stateMachine.ChangeState(new IdlingState(cat, stateMachine));

        if (cat.scaredAt != Vector3.zero && !cat.IsStarving)
            stateMachine.ChangeState(new FrighteningState(cat, stateMachine));

        Search();
    }

    void Search() {
        foreach (var collider in cat.Search(3f)) {
            if (collider.tag == "Snack" && cat.IsHungry)
                stateMachine.ChangeState(new ChasingState(cat, stateMachine, collider.transform));
        }
    }
}