using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [ExecuteAlways]
public class Bowl : MonoBehaviour {
    public BowlData data;
    [NonSerialized] public Cat feedingCat;
    Material material;
    float previousAmount;
    public float InitialMeal => data.foodAmount.InitialValue;
    public float RemainingMeal => data.foodAmount.RuntimeValue;

    public void OnEnable () => BowlManager.AddBowl(this);
    public void OnDisable () => BowlManager.RemoveBowl(this);

    public float FoodAmount => data.foodAmount.RuntimeValue;

    public void Initialize () {
        // Clone data
        data = Instantiate(data);
        data.foodAmount = Instantiate(data.foodAmount);
        data.foodAmount.RuntimeValue = 0f;

        // Clone material
        material = Instantiate(GetComponentInChildren<MeshRenderer>().material);
        GetComponentInChildren<MeshRenderer>().material = material;
        material.SetFloat("_Amount", 0f);
    }

    public float Feed(Cat cat) {
        float speed = cat.data.eatSpeed;
        float currentAmount = data.foodAmount.RuntimeValue - Time.deltaTime * speed;
        previousAmount = data.foodAmount.RuntimeValue;

        // Clamp negative values
        if (currentAmount <= 0f) currentAmount = 0f;

        // Update material color
        material.SetFloat("_Amount", Mathf.InverseLerp(0f, data.foodAmount.InitialValue, currentAmount));

        // Update data
        data.foodAmount.RuntimeValue = currentAmount;

        feedingCat = cat;

        return previousAmount - currentAmount;
    }

    public void PlaceFood() {
        data.foodAmount.RuntimeValue = data.foodAmount.InitialValue;
        material.SetFloat("_Amount", 1f);
    }

    public void Feeding(Cat cat) {
        feedingCat = cat;
    }

    public void StopFeeding(Cat cat) {
        if (feedingCat == cat)
            feedingCat = null;
    }
}
