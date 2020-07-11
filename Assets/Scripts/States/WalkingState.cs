using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkingState : State {
    Vector3 position;
    Vector3 debugPosition;

    public WalkingState (Cat cat, StateMachine stateMachine) : base(cat, stateMachine) { }

    public override void Enter (State previousState) {
        base.Enter(previousState);

        position = RandomPosition(2f);
    }

    public override void LogicUpdate () {
        base.LogicUpdate();

        Walk();
        ReachTarget();
    }

    void Walk() {
        cat.agent.destination = position;
        Debug.DrawLine(cat.transform.position, position, Color.yellow);
    }

    void ReachTarget() {
        float distance = (cat.transform.position - position).magnitude;

        if (distance <= .5f)
            stateMachine.ChangeState(cat.idling);
    }

    Vector3 RandomPosition(float distance = 1f) {
        Vector2 random = Random.insideUnitCircle;
        Vector3 direction = new Vector3(random.x, 0f, random.y);

        if (Physics.Raycast(cat.transform.position, direction, out RaycastHit raycastHit, distance))
            direction = (cat.transform.position - raycastHit.point).normalized;

        Vector3 position = direction * distance;
        position += cat.transform.position;

        NavMeshHit navmeshHit;
        NavMesh.SamplePosition(position, out navmeshHit, distance, 1);

        return navmeshHit.position;
    }
}