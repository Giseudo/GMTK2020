using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CatType {
    Fool,
    Thief
};

[CreateAssetMenu(fileName = "CatData", menuName = "Data/CatData", order = 1)]
public class CatData : ScriptableObject {
    public string nick;
    public string state;
    public CatType type;
    public float eatSpeed;
    public float hungerSpeed;
    public FloatVariable hunger;
}
