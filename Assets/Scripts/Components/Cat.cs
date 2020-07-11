using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cat : MonoBehaviour {
    public enum CatBehavior {
        Idleing,
        Eating,
        Hiding
    };

    public CatData data;
    public Transform goal;
    CatBehavior behavior = CatBehavior.Idleing;
    NavMeshAgent agent;

    void Awake () {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update () {
        Vector3 position = goal.position;

        position -= (goal.position - transform.position).normalized * .5f;

        agent.destination = position;
    }
}
