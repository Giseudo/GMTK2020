using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingState : State {
    public Transform target;

    public ChasingState (Cat cat, StateMachine stateMachine) : base(cat, stateMachine) { }

    public override void Enter (State previousState) {
        base.Enter(previousState);

        cat.animator.SetBool("Walking", true);

        if (target.tag == "Snack")
            cat.FallInLove(true);
    }

    public override void Exit (State nextState) {
        base.Exit(nextState);

        cat.animator.SetBool("Walking", false);

        if (target == null || !target.gameObject) return;

        if (target.tag == "Snack" && nextState != cat.eatingSnack)
            cat.FallInLove(false);
    }

    public override void LogicUpdate () {
        base.LogicUpdate();

        if (target == null || !target.gameObject) {
            stateMachine.ChangeState(cat.idling);
            return;
        }

        Chase();
        ReachTarget();
    }

    void Chase () {
        Vector3 position = target.position;

        position -= (position - cat.transform.position).normalized * .5f;

        cat.agent.destination = position;
    }

    void ReachTarget () {
        float distance = (target.position - cat.transform.position).magnitude;

        if (target.tag == "Snack" && distance <= 1f) {
            if (ItemManager.Instance.snack.dropped) {
                cat.eatingSnack.snack = target.transform;
                stateMachine.ChangeState(cat.eatingSnack);
            }

            return;
        }

        if (distance <= 1f)
            stateMachine.ChangeState(cat.idling);
    }
}