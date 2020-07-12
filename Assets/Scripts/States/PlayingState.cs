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
        cat.agent.speed += 5f;
    }

    public override void Exit(State nextState) {
        base.Exit(nextState);

        reachTime = 0f;
        startTime = 0f;
        cat.agent.speed = cat.data.walkSpeed;
        cat.agent.isStopped = false;
    }

    public override void LogicUpdate () {
        base.LogicUpdate();

        cat.agent.destination = ball.position;

        if (cat.agent.remainingDistance <= .25f) {
            cat.agent.isStopped = true;
            reachTime = Time.unscaledTime;
        }

        if (reachTime + 2f < Time.unscaledTime) {
            reachTime = 0f;
            cat.agent.isStopped = false;
        }

        if (startTime + 10f < Time.unscaledTime) {
            stateMachine.ChangeState(cat.idling);
        }
    }
}