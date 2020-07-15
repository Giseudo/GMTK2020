using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingState : State {
    public Transform target;

    public ChasingState (Cat cat, StateMachine stateMachine, Transform target = null) : base(cat, stateMachine) {
        this.target = target;
    }

    public override void Enter (State previousState) {
        base.Enter(previousState);

        cat.walking = true;

        if (target.tag == "Snack")
            cat.FallInLove(true);
    }

    public override void Exit (State nextState) {
        base.Exit(nextState);

        cat.walking = false;

        if (target == null || !target.gameObject) return;

        if (target.tag == "Snack" && nextState.ToString() != "EatingSnackState")
            cat.FallInLove(false);
    }

    public override void LogicUpdate () {
        base.LogicUpdate();

        if (target == null || !target.gameObject || !target.gameObject.activeInHierarchy) {
            stateMachine.ChangeState(new IdlingState(cat, stateMachine));
            return;
        }

        Search();
        Chase();
        ReachTarget();

        if (cat.scaredAt != Vector3.zero && !cat.IsStarving)
            stateMachine.ChangeState(new FrighteningState(cat, stateMachine));
    }

    void Chase () {
        Vector3 position = target.position;

        position -= (position - cat.transform.position).normalized * .5f;

        cat.agent.destination = position;
    }

    void Search () {
        Bowl chasingBowl = target.GetComponent<Bowl>();

        if (chasingBowl != null) {
            if (chasingBowl.FoodAmount <= 0f)
                stateMachine.ChangeState(new IdlingState(cat, stateMachine));
        }

        foreach (var hit in cat.Search(1f)) {
            if (hit.tag == "Snack" && cat.IsHungry) {
                if (ItemManager.Instance.snack.dropped) {
                    stateMachine.ChangeState(new EatingSnackState(cat, stateMachine, hit.transform));
                    return;
                }
            }

            if (hit.tag == "Bowl") {
                Bowl bowl = hit.GetComponent<Bowl>();

                if (cat.CanEat(bowl))
                    stateMachine.ChangeState(new EatingMealState(cat, stateMachine, bowl));
            }
        }

        foreach (var collider in cat.Search(3f)) {
            if (collider.tag == "YarnBall" && !cat.IsStarving)
                stateMachine.ChangeState(new PlayingState(cat, stateMachine, collider.transform));

            if (collider.tag == "Snack" && cat.IsHungry)
                stateMachine.ChangeState(new ChasingState(cat, stateMachine, collider.transform));
        }
    }

    void ReachTarget () {
        float distance = (target.position - cat.transform.position).magnitude;

        if (distance <= .2f)
            stateMachine.ChangeState(new IdlingState(cat, stateMachine));
    }
}