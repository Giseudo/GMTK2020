using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogUI : MonoBehaviour {
    void Start() {
        GameManager.Instance.onRoundEnd += showDialog;
    }

    void showDialog(List<Cat> deadCats) {
        transform.localPosition = Vector3.zero;

        if (deadCats.Count > 0)
            Debug.Log("Game over!");
        else
            Debug.Log("You won!");
    }
}
