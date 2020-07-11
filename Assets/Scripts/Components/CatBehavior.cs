using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatBehavior : MonoBehaviour {
    public CatData data;
    public Transform goal;
    NavMeshAgent agent;

    void Awake () {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update () {
        agent.destination = goal.position;
    }
}
