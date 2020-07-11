using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Bowl : MonoBehaviour {
    public BowlData data;
    [NonSerialized] public Cat feedingCat;
    Material material;

    public void OnEnable () => BowlManager.bowls.Add(this);
    public void OnDisable () => BowlManager.bowls.Remove(this);

    public float FoodAmount => data.foodAmount.RuntimeValue;

    public void Initialize () {
        // Clone data
        data = Instantiate(data);
        data.foodAmount = Instantiate(data.foodAmount);

        // Clone material
        material = Instantiate(GetComponentInChildren<MeshRenderer>().material);
        GetComponentInChildren<MeshRenderer>().material = material;
    }

    public float Feed(float speed) {
        if (data.foodAmount.RuntimeValue < 0f) {
            data.foodAmount.RuntimeValue = 0f;
            return 0f;
        }

        data.foodAmount.RuntimeValue -= Time.deltaTime * speed;

        float amount = Mathf.InverseLerp(0f, data.foodAmount.InitialValue, data.foodAmount.RuntimeValue);

        material.SetFloat("_Amount", amount);

        return amount;
    }
}
