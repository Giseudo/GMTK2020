using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingSnackState : State {
    float startTime;
    public EatingSnackState (Cat cat, StateMachine stateMachine) : base(cat, stateMachine) { }

    public override void Enter(State previousState) {
        base.Enter(previousState);

        startTime = Time.unscaledTime;
        ItemManager.Instance.snack.Drop();
        Debug.Log("Eating Snack");
    }

    public override void LogicUpdate () {
        base.LogicUpdate();

        Eat();
    }

    public void Eat() {
        if (startTime + 5f < Time.unscaledTime) {
            ItemManager.Instance.snack.Hide();
            stateMachine.ChangeState(cat.walking);
        }
    }
}
