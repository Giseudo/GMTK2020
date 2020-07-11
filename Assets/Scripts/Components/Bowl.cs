using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Bowl : MonoBehaviour {
    public BowlData data;
    Material material;

    public void OnEnable () => BowlManager.bowls.Add(this);
    public void OnDisable () => BowlManager.bowls.Remove(this);

    public float FoodAmount => data.foodAmount.RuntimeValue;

    public void Start () {
        material = GetComponentInChildren<MeshRenderer>().sharedMaterial;
    }

    public void Eat(float speed) {
        data.foodAmount.RuntimeValue -= Time.deltaTime * speed;

        if (data.foodAmount.RuntimeValue < 0f)
            data.foodAmount.RuntimeValue = 0f;

        float amount = Mathf.InverseLerp(0f, data.foodAmount.InitialValue, data.foodAmount.RuntimeValue);

        material.SetFloat("_Amount", amount);
    }
}
