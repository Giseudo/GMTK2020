using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingState : State {
    public Transform ball;
    public PlayingState (Cat cat, StateMachine stateMachine) : base(cat, stateMachine) { }

    public override void Enter(State previousState) {
        base.Enter(previousState);

        cat.agent.speed += 5f;
    }

    public override void Exit(State nextState) {
        base.Exit(nextState);

        cat.agent.speed = cat.data.walkSpeed;
    }

    public override void LogicUpdate () {
        base.LogicUpdate();

        cat.agent.destination = ball.position;
    }
}