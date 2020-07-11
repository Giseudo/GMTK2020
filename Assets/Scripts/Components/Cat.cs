using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[ExecuteAlways]
public class Cat : MonoBehaviour {
    public CatData data;
    [NonSerialized] public NavMeshAgent agent;
    StateMachine behaviorSM = new StateMachine();
    public IdlingState idling;
    public WalkingState walking;
    public ChasingState chasing;
    public EatingMealState eatingMeal;
    public EatingSnackState eatingSnack;
    public PlayingState playing;
    Bowl ClosestBowl => BowlManager.GetClosestBowl(transform.position);

    void OnEnable() => CatManager.cats.Add(this);
    void OnDisable() => CatManager.cats.Remove(this);

    void Start () {
        agent = GetComponent<NavMeshAgent>();

        idling = new IdlingState (this, behaviorSM);
        walking = new WalkingState (this, behaviorSM);
        chasing = new ChasingState (this, behaviorSM);
        eatingMeal = new EatingMealState (this, behaviorSM);
        eatingSnack = new EatingSnackState (this, behaviorSM);
        playing = new PlayingState (this, behaviorSM);

        behaviorSM.Initialize(walking);
    }

    void Update () {
        if (!Application.IsPlaying(gameObject)) return;

        behaviorSM.CurrentState.LogicUpdate();

        Search();
    }

    void FixedUpdate() {
        if (!Application.IsPlaying(gameObject)) return;

        behaviorSM.CurrentState.PhysicsUpdate();
    }

    public void LookForFood () {
        chasing.target = ClosestBowl.transform;

        behaviorSM.ChangeState(chasing);
    }

    void Search () {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);

        foreach (var hit in colliders) {
            switch (hit.gameObject.name) {
                case "Bowl":
                    eatingMeal.bowl = hit.GetComponent<Bowl>();
                    behaviorSM.ChangeState(eatingMeal);
                    break;
                case "Snack":
                    behaviorSM.ChangeState(eatingSnack);
                    break;
                case "YarnBall":
                    behaviorSM.ChangeState(playing);
                    break;
            }
        }
    }
}
