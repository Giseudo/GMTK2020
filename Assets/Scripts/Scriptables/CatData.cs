using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CatType {
    Fool,
    Thief
};

[CreateAssetMenu(fileName = "CatData", menuName = "Data/CatData", order = 1)]
public class CatData : ScriptableObject {
    public FloatVariable hunger;
    public string nick;
    public float eatSpeed;
    public CatType type;
}
