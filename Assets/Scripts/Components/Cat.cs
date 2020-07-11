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
    IdlingState idling;
    WalkingState walking;
    EatingState eating;
    ChasingState chasing;
    PlayingState playing;

    void OnEnable() => CatManager.cats.Add(this);
    void OnDisable() => CatManager.cats.Remove(this);

    void Awake () {
        agent = GetComponent<NavMeshAgent>();

        idling = new IdlingState (this, behaviorSM);
        walking = new WalkingState (this, behaviorSM);
        eating = new EatingState (this, behaviorSM);
        chasing = new ChasingState (this, behaviorSM);
        playing = new PlayingState (this, behaviorSM);

        behaviorSM.Initialize(idling);
    }

    public void Eat () {
        behaviorSM.ChangeState(eating);
    }

    void Update () {
        if (behaviorSM.CurrentState is null) return;

        behaviorSM.CurrentState.LogicUpdate();
    }

    void FixedUpdate() {
        if (behaviorSM.CurrentState is null) return;

        behaviorSM.CurrentState.PhysicsUpdate();
    }
}
