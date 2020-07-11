using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CatData", menuName = "Data/CatData", order = 1)]
public class CatData : ScriptableObject {
    public enum CatType {
        Fool,
        Thief
    };

    public FloatVariable hunger;
    public string nick;
    public float mealDuration;
    public CatType type;
}
