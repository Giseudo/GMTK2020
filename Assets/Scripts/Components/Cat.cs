using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[ExecuteAlways]
public class Cat : MonoBehaviour {
    public CatData data;
    [NonSerialized] public NavMeshAgent agent;
    public StateMachine behaviorSM;
    public IdlingState idling;
    public WalkingState walking;
    public ChasingState chasing;
    public EatingMealState eatingMeal;
    public EatingSnackState eatingSnack;
    public PlayingState playing;
    [NonSerialized] public Bowl previousBowl;
    Bowl ClosestBowl => BowlManager.GetClosestBowl(this);
    public bool IsThief => data.type == CatType.Thief;

    State CurrentState => behaviorSM.CurrentState;
    void OnEnable() => CatManager.cats.Add(this);
    void OnDisable() => CatManager.cats.Remove(this);

    public delegate void OnSwallow(Cat cat, float amount);
    public OnSwallow onSwallow;

    public void Initialize () {
        agent = GetComponent<NavMeshAgent>();
        behaviorSM = new StateMachine();

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

        data.state = CurrentState.ToString();

        behaviorSM.CurrentState.LogicUpdate();

        Search();
    }

    void FixedUpdate() {
        if (!Application.IsPlaying(gameObject)) return;

        behaviorSM.CurrentState.PhysicsUpdate();
    }

    public void LookForFood () {
        // Is the cat already eating?
        if (behaviorSM.CurrentState == eatingMeal) return;

        Bowl bowl = ClosestBowl;

        // No meal left? Meh...
        if (bowl == null) {
            behaviorSM.ChangeState(walking);
            return;
        }

        // Take that bowl!
        chasing.target = bowl.transform;
        behaviorSM.ChangeState(chasing);
    }

    public bool CanEat (Bowl bowl) {
        bool valid = true;

        // Is there any cat eating it? Am I a thief?
        if (bowl.feedingCat != null && !IsThief) valid = false;

        // I was there just before
        if (bowl == previousBowl) valid = false;

        // There's no food
        if (bowl.FoodAmount <= 0f) valid = false;

        return valid;
    }

    public void Swallow (float amount) {
        if (onSwallow != null)
            onSwallow(this, amount);

        data.hunger.RuntimeValue -= amount;
    }

    void Search () {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);

        foreach (var hit in colliders) {
            switch (hit.gameObject.name) {
                case "Bowl":
                    if (CurrentState == eatingMeal) return;

                    Bowl bowl = hit.GetComponent<Bowl>();

                    if (CanEat(bowl)) {
                        eatingMeal.bowl = bowl;
                        behaviorSM.ChangeState(eatingMeal);
                        break;
                    }

                    if (CurrentState == chasing && chasing.target == bowl)
                        behaviorSM.ChangeState(walking);

                    break;
                case "Snack":
                    behaviorSM.ChangeState(eatingSnack);
                    break;
                case "YarnBall":
                    behaviorSM.ChangeState(playing);
                    break;
            }
        }

        colliders = Physics.OverlapSphere(transform.position, 2f);

        foreach (var hit in colliders) {
            switch (hit.gameObject.name) {
                case "YarnBall":
                case "Snack":
                    chasing.target = hit.transform;
                    behaviorSM.ChangeState(chasing);
                    break;
            }
        }
    }
}
