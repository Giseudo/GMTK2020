using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogUI : MonoBehaviour {
    Vector3 initialPosition;
    [SerializeField] Transform victoryDialog;
    [SerializeField] Transform defeatDialog;

    void Start() {
        initialPosition = transform.localPosition;
        GameManager.Instance.onRoundEnd += showDialog;
    }

    void showDialog(List<Cat> deadCats) {
        transform.localPosition = Vector3.zero;

        if (deadCats.Count == 0)
            victoryDialog.gameObject.SetActive(true);
        else
            defeatDialog.gameObject.SetActive(true);
    }

    public void Continue() {
        GameManager.Instance.SpawnCats();
        Close();
    }

    public void Restart() {
        Close();
    }

    void Close () {
        victoryDialog.gameObject.SetActive(false);
        defeatDialog.gameObject.SetActive(false);
        transform.localPosition = initialPosition;
    }
}
