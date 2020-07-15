using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprinkler : MonoBehaviour
{
    public float radius = 1.5f;

    void Start() {
        SoundManager.Instance.Play("WaterSpray");
    }

    void LateUpdate() {
        SearchCats();
    }

    void SearchCats() {
        foreach (Cat cat in CatManager.cats) {
            if ((cat.transform.position - transform.position).magnitude <= radius) {
                cat.Scare(transform.position);
            }
        }
    }
}
