using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingMealState : State {
    public Bowl bowl;

    public EatingMealState (Cat cat, StateMachine stateMachine) : base(cat, stateMachine) { }

    public override void Enter(State previousState) {
        base.Enter(previousState);

        StayAtBowl();
        bowl.Feeding(cat);
        cat.animator.SetBool("Eating", true);
    }

    public override void Exit(State nextState) {
        base.Exit(nextState);

        cat.StopEating(bowl);
        bowl.StopFeeding(cat);
        cat.animator.SetBool("Eating", false);
    }

    public override void LogicUpdate () {
        base.LogicUpdate();

        Eat();
        FaceBowl();
    }

    void Eat() {
        // Another cat take it from me?
        if (bowl.feedingCat != cat) {
            stateMachine.ChangeState(cat.walking);
            return;
        }

        cat.Eat(bowl);

        if (cat.Hunger <= 0f) {
            stateMachine.ChangeState(cat.walking);
            return;
        }

        if (bowl.FoodAmount <= 0f)
            cat.LookForFood();
    }

    void StayAtBowl() {
        Vector3 position = bowl.transform.position;

        position -= (position - cat.transform.position).normalized * .5f;

        cat.agent.destination = position;
    }

    void FaceBowl() {
        Vector3 direction = Vector3.RotateTowards(cat.transform.forward, (bowl.transform.position - cat.transform.position).normalized, .5f, Time.deltaTime);

        cat.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }
}
