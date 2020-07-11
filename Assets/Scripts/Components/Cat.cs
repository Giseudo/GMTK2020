using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cat : MonoBehaviour {
    public CatData data;
    public Transform bowl;
    [NonSerialized] public NavMeshAgent agent;
    StateMachine behaviorSM = new StateMachine();
    IdlingState idling;
    WalkingState walking;
    EatingState eating;
    ChasingState chasing;
    PlayingState playing;

    void Start () {
        agent = GetComponent<NavMeshAgent>();

        idling = new IdlingState (this, behaviorSM);
        walking = new WalkingState (this, behaviorSM);
        eating = new EatingState (this, behaviorSM, bowl);
        chasing = new ChasingState (this, behaviorSM);
        playing = new PlayingState (this, behaviorSM);

        behaviorSM.Initialize(idling);
    }

    void Update () {
        behaviorSM.CurrentState.LogicUpdate();
    }

    void FixedUpdate() {
        behaviorSM.CurrentState.PhysicsUpdate();
    }
}
