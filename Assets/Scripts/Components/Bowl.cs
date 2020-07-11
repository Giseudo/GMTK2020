using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Bowl : MonoBehaviour {
    public void OnEnable () => BowlManager.bowls.Add(this);
    public void OnDisable () => BowlManager.bowls.Remove(this);
}
