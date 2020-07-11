using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Bowl : MonoBehaviour {
    public void OnEnable () => BowlManager.bowls.Add(this);
    public void OnDisable () => BowlManager.bowls.Remove(this);

    Material material;

    public void Start () {
        material = GetComponentInChildren<MeshRenderer>().sharedMaterial;
    }

    public void Eat(float speed) {
        float amount = material.GetFloat("_Amount");

        if (amount >= 0) {
            material.SetFloat("_Amount", amount - Time.deltaTime * speed * .1f);
        }
    }
}
