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
    [NonSerialized] public Bowl previousBowl;
    [NonSerialized] public bool walking, eating;
    public Transform head;
    public Animator animator;
    public delegate void OnHungerChange(Cat cat);
    public OnHungerChange onHungerChange;
    Material material;
    float lastTimeScared, lovedLastTime, previousBowlTime = 0f;
    public Vector3 scaredAt = Vector3.zero;
    GameObject loveEmotion;
    State CurrentState => behaviorSM.CurrentState;
    public Bowl ClosestBowl => BowlManager.GetClosestBowl(this);
    public bool IsThief => data.type == CatType.Thief;
    public bool IsStarving => Hunger > 100f;
    public bool IsHungry => Hunger > 50f;
    public float Hunger => Mathf.Round(data.hunger.RuntimeValue);

    void OnEnable() => CatManager.AddCat(this);
    void OnDisable() => CatManager.RemoveCat(this);

    public void Start() {
        agent = GetComponent<NavMeshAgent>();
        agent.speed  = data.walkSpeed;
        agent.updateRotation = false;

        behaviorSM = new StateMachine();
        behaviorSM.Initialize(new IdlingState (this, behaviorSM));
    }

    public void Initialize () {
        GetComponentInChildren<Renderer>().material = Instantiate(GetComponentInChildren<Renderer>().material);
        material = GetComponentInChildren<Renderer>().sharedMaterial;
        material.SetFloat("_Hunger", Mathf.InverseLerp(0f, data.hunger.InitialValue * 2, data.hunger.InitialValue));
    }

    void Update () {
        if (!Application.IsPlaying(gameObject)) return;

        UpdateState();
        behaviorSM.CurrentState.LogicUpdate();
        ClearState();
    }

    void LateUpdate() {
        if (!Application.IsPlaying(gameObject)) return;

        FaceDirection();
    }

    void FixedUpdate() {
        if (!Application.IsPlaying(gameObject)) return;

        behaviorSM.CurrentState.PhysicsUpdate();
    }

    void UpdateState() {
        data.state = CurrentState.ToString();

        if (!GameManager.Instance.IsPlaying) return;

        animator.SetBool("Jumping", agent.isOnOffMeshLink);
        animator.SetBool("Walking", walking);
        animator.SetBool("Eating", eating);

        IncreaseHunger();
    }

    void ClearState() {
        if (previousBowlTime + 5f < Time.unscaledTime)
            previousBowl = null;

        if  (lovedLastTime + 5f < Time.unscaledTime)
            FallInLove(false);
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
        if (CurrentState.ToString() == "FrighteningState") valid = false;

        return valid;
    }

    public void Eat (Bowl bowl) {
        float amount = bowl.Feed(this);
        eating = true;

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
        eating = false;
    }

    public void Scare (Vector3 position) {
        if (Hunger > 150f) return;

        if (lastTimeScared + .5f < Time.unscaledTime)
            scaredAt = position;

        lastTimeScared = Time.unscaledTime;
    }

    public void FallInLove (bool active) {
        if (!active) {
            if (loveEmotion != null)
                loveEmotion.SetActive(false);

            loveEmotion = null;

            return;
        }

        GameObject emotion = EmotionManager.Instance.GetLoveEmotion();

        emotion.GetComponent<FollowTarget>().target = head;
        emotion.SetActive(true);

        SoundManager.Instance.Play("Hungry", .5f);

        loveEmotion = emotion;
        lovedLastTime = Time.unscaledTime;
    }

    public void Attention () {
        GameObject emotion = EmotionManager.Instance.GetAttentionEmotion();

        emotion.GetComponent<FollowTarget>().target = head;
        emotion.SetActive(true);

        SoundManager.Instance.Play("Playing", .5f);
    }

    public Collider[] Search (float radius) {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        return colliders;
    }

    void FaceDirection() {
        if (agent.velocity.magnitude < .1f) return;

        Quaternion agentRotation = Quaternion.LookRotation(agent.velocity.normalized);
        Quaternion desiredRotation = Quaternion.RotateTowards(transform.rotation, agentRotation, 500f * Time.deltaTime);

        transform.rotation = desiredRotation;
    }

    void IncreaseHunger() {
        string[] eatingStates = {"EatingMealState", "EatingSnackState"};

        if (Array.IndexOf(eatingStates, CurrentState.ToString()) == -1 && Hunger < data.hunger.InitialValue * 2) {
            data.hunger.RuntimeValue += data.hungerSpeed * Time.deltaTime;

            if (onHungerChange != null) onHungerChange(this);
        }

        float hunger = Mathf.InverseLerp(0f, data.hunger.InitialValue * 2f, data.hunger.RuntimeValue);
        material.SetFloat("_Hunger", hunger);
    }
}
