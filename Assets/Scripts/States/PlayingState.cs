using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingState : State {
    public Transform ball;
    public float reachTime = 0f;
    public float startTime = 0f;
    public PlayingState (Cat cat, StateMachine stateMachine) : base(cat, stateMachine) { }

    public override void Enter(State previousState) {
        base.Enter(previousState);

        startTime = Time.unscaledTime;
        cat.agent.destination = ball.position;
        cat.agent.speed += 2f;

        cat.animator.SetBool("Walking", true);
    }

    public override void Exit(State nextState) {
        base.Exit(nextState);

        reachTime = 0f;
        startTime = 0f;
        cat.agent.speed = cat.data.walkSpeed;
    }

    public override void LogicUpdate () {
        base.LogicUpdate();

        if (ball == null) {
            stateMachine.ChangeState(cat.idling);
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

        if (startTime + 10f < Time.unscaledTime) {
            stateMachine.ChangeState(cat.idling);
        }
    }
}