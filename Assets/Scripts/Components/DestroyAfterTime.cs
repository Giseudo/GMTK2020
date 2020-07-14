using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {
    public bool isEnabled;
    public float secondsToDestroy;

    void Start() {
        if (isEnabled)
            StartCoroutine("DestroyTimer");
    }

    public void Enable() {
        isEnabled = true;

        StartCoroutine("DestroyTimer");
    }

    IEnumerator DestroyTimer () {
        yield return new WaitForSeconds(secondsToDestroy);

        Destroy(gameObject);
    }
}
