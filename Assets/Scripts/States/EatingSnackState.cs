using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingSnackState : State {
    public Transform snack;
    float startTime;
    public EatingSnackState (Cat cat, StateMachine stateMachine) : base(cat, stateMachine) { }

    public override void Enter(State previousState) {
        base.Enter(previousState);

        startTime = Time.unscaledTime;
        ItemManager.Instance.snack.Drop();
    }

    public override void Exit(State nextState) {
        base.Exit(nextState);
        startTime = 0f;
    }

    public override void LogicUpdate () {
        base.LogicUpdate();

        Eat();
    }

    public void Eat() {
        cat.data.hunger.RuntimeValue -= Time.deltaTime * cat.data.eatSpeed;
        if (cat.onHungerChange != null) cat.onHungerChange(cat);

        if (cat.Hunger <= 0f || snack == null) {
            stateMachine.ChangeState(cat.walking);
            return;
        }
    }
}
