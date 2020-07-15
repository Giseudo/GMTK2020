using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingSnackState : State {
    public Transform snack;
    float startTime;
    public EatingSnackState (Cat cat, StateMachine stateMachine, Transform snack = null) : base(cat, stateMachine) {
        this.snack = snack;
    }

    public override void Enter(State previousState) {
        base.Enter(previousState);

        cat.agent.isStopped = true;
        startTime = Time.unscaledTime;
        ItemManager.Instance.snack.Drop();
    }

    public override void Exit(State nextState) {
        base.Exit(nextState);
        cat.agent.isStopped = false;
        startTime = 0f;

        cat.FallInLove(false);
    }

    public override void LogicUpdate () {
        base.LogicUpdate();

        Eat();

        if (cat.scaredAt != Vector3.zero && !cat.IsStarving)
            stateMachine.ChangeState(new FrighteningState(cat, stateMachine));
    }

    public void Eat() {
        cat.data.hunger.RuntimeValue -= Time.deltaTime * cat.data.eatSpeed;
        if (cat.onHungerChange != null) cat.onHungerChange(cat);

        if (cat.Hunger <= 0f || snack == null) {
            stateMachine.ChangeState(new WalkingState(cat, stateMachine));
            return;
        }
    }
}
