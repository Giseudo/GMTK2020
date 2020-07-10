using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CatData", menuName = "Data/CatData", order = 1)]
public class CatData : ScriptableObject {
    public string name;
    public bool thief;
    public float eatTime;
}
