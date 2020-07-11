using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingState : State {
    public Transform target;

    public ChasingState (Cat cat, StateMachine stateMachine) : base(cat, stateMachine) { }

    public override void LogicUpdate () {
        base.LogicUpdate();

        Chase();
    }

    void Chase () {
        Vector3 position = target.position;
        position -= (position - cat.transform.position).normalized * .5f;

        cat.agent.destination = position;
    }
}