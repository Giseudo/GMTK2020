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
    public FrighteningState frightening;
    [NonSerialized] public Bowl previousBowl;
    float previousBowlTime;
    public Animator animator;
    Bowl ClosestBowl => BowlManager.GetClosestBowl(this);
    public bool IsThief => data.type == CatType.Thief;
    public float Hunger => Mathf.Round(data.hunger.RuntimeValue);
    State CurrentState => behaviorSM.CurrentState;

    void OnEnable() => CatManager.AddCat(this);
    void OnDisable() => CatManager.RemoveCat(this);

    public delegate void OnHungerChange(Cat cat);
    public OnHungerChange onHungerChange;
    Material material;

    public void Initialize () {
        material = GetComponentInChildren<Renderer>().sharedMaterial;
        agent = GetComponent<NavMeshAgent>();
        behaviorSM = new StateMachine();

        idling = new IdlingState (this, behaviorSM);
        walking = new WalkingState (this, behaviorSM);
        chasing = new ChasingState (this, behaviorSM);
        eatingMeal = new EatingMealState (this, behaviorSM);
        eatingSnack = new EatingSnackState (this, behaviorSM);
        playing = new PlayingState (this, behaviorSM);
        frightening = new FrighteningState (this, behaviorSM);
        agent.speed  = data.walkSpeed;

        behaviorSM.Initialize(idling);
    }

    void Update () {
        if (!Application.IsPlaying(gameObject)) return;

        UpdateState();
        Search();
        behaviorSM.CurrentState.LogicUpdate();
        ClearState();
    }

    void FixedUpdate() {
        if (!Application.IsPlaying(gameObject)) return;

        behaviorSM.CurrentState.PhysicsUpdate();
    }

    void UpdateState() {
        data.state = CurrentState.ToString();

        if (!GameManager.Instance.IsPlaying) return;

        if (CurrentState != eatingMeal && CurrentState != eatingSnack) {
            data.hunger.RuntimeValue += data.hungerSpeed * Time.deltaTime;

            if (onHungerChange != null) onHungerChange(this);
        }

        animator.SetBool("Jumping", agent.isOnOffMeshLink);

        float hunger = Mathf.InverseLerp(0f, data.hunger.InitialValue * 2f, data.hunger.RuntimeValue);
        material.SetFloat("_Hunger", hunger);
    }

    void ClearState() {
        if (previousBowlTime + 5f < Time.unscaledTime)
            previousBowl = null;
    }

    public void LookForFood () {
        Bowl bowl = ClosestBowl;

        // No meal left? Meh...
        if (bowl == null || Hunger < 25f) {
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

        // Can't eat if I'm running :(
        if (CurrentState == frightening) valid = false;

        return valid;
    }

    public void Eat (Bowl bowl) {
        float amount = bowl.Feed(this);

        if (Hunger <= 0f) {
            amount = 0f;
            data.hunger.RuntimeValue = 0f;
        }

        if (onHungerChange != null) onHungerChange(this);

        data.hunger.RuntimeValue -= amount;
    }

    public void StopEating (Bowl bowl) {
        previousBowlTime = Time.unscaledTime;
        previousBowl = bowl;
    }

    public void Scare () {
        behaviorSM.ChangeState(frightening);
    }

    void Search () {
        if (CurrentState == eatingSnack) return;
        if (CurrentState == playing) return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);

        foreach (var hit in colliders) {
            switch (hit.tag) {
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
                    if (Hunger > 50f) {
                        behaviorSM.ChangeState(eatingSnack);
                        return;
                    }
                    break;
            }
        }

        colliders = Physics.OverlapSphere(transform.position, 2f);

        foreach (var hit in colliders) {
            switch (hit.tag) {
                case "YarnBall":
                    if (Hunger > 50) break;

                    playing.ball = hit.transform;
                    behaviorSM.ChangeState(playing);
                    break;
                case "Snack":
                    if (Hunger > 100f) {
                        chasing.target = hit.transform;
                        behaviorSM.ChangeState(chasing);
                    }
                    break;
            }
        }
    }
}
