using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingMealState : State {
    public Bowl bowl;

    public EatingMealState (Cat cat, StateMachine stateMachine) : base(cat, stateMachine) { }

    public override void Enter(State previousState) {
        base.Enter(previousState);

        bowl.feedingCat = cat;
    }

    public override void Exit(State nextState) {
        base.Exit(nextState);

        if (bowl.feedingCat == cat)
            bowl.feedingCat = null;
    }

    public override void LogicUpdate () {
        base.LogicUpdate();

        if (bowl.feedingCat != cat)
            stateMachine.ChangeState(cat.walking);

        bowl.Eat(cat.data.eatSpeed);

        if (bowl.FoodAmount <= 0f) // TODO Am I still hungry?
            stateMachine.ChangeState(cat.walking);
    }
}
