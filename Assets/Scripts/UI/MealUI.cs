using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MealUI : MonoBehaviour {
    [SerializeField] Transform remainingMealBar;

    public void Update () {
        float remaining = BowlManager.RemainingMeal();
        float total = BowlManager.InitialMeal();
        float t = Mathf.InverseLerp(0f, total, remaining);

        remainingMealBar.localScale = new Vector3(1, t, 1);
    }
}
