using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingMealState : State {
    public Bowl bowl;

    public EatingMealState (Cat cat, StateMachine stateMachine, Bowl bowl = null) : base(cat, stateMachine) {
        this.bowl = bowl;
    }

    public override void Enter(State previousState) {
        base.Enter(previousState);

        cat.Eat(bowl);
        StayAtBowl();
    }

    public override void Exit(State nextState) {
        base.Exit(nextState);

        cat.StopEating(bowl);
        bowl.StopFeeding(cat);
    }

    public override void LogicUpdate () {
        base.LogicUpdate();

        Eat();
        FaceBowl();

        if (cat.scaredAt != Vector3.zero && !cat.IsStarving)
            stateMachine.ChangeState(new FrighteningState(cat, stateMachine));
    }

    void Eat() {
        if (bowl.feedingCat != cat || cat.Hunger <= 0f || bowl.FoodAmount <= 0f) {
            stateMachine.ChangeState(new WalkingState(cat, stateMachine));
            return;
        }

        cat.Eat(bowl);
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
