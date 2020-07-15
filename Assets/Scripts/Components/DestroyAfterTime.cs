using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {
    public bool destroy = true;
    public bool isEnabled;
    public float secondsToDestroy;

    void OnEnable() {
        if (isEnabled) StartCoroutine("DestroyTimer");
    }

    void Start() {
        if (isEnabled) StartCoroutine("DestroyTimer");
    }

    public void Enable() {
        isEnabled = true;

        StartCoroutine("DestroyTimer");
    }

    IEnumerator DestroyTimer () {
        yield return new WaitForSeconds(secondsToDestroy);

        if (destroy)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }
}
