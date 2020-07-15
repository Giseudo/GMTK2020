using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {
    public Vector3 offset;
    public Transform target;

    void Update() {
        transform.position = target.position + offset;
    }

    public void SetTarget(Transform target) {
        this.target = target;
    }
}
