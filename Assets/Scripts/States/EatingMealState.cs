using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingMealState : State {
    public Bowl bowl;

    public EatingMealState (Cat cat, StateMachine stateMachine) : base(cat, stateMachine) { }

    public override void Enter(State previousState) {
        base.Enter(previousState);
    }

    public override void LogicUpdate () {
        base.LogicUpdate();

        bowl.Eat(cat.data.eatSpeed);
    }
}
