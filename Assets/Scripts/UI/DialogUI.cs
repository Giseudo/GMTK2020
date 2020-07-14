using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogUI : MonoBehaviour {
    Vector3 initialPosition;
    RectTransform rectTransform;
    [SerializeField] Transform victoryDialog;
    [SerializeField] Transform defeatDialog;

    void Start() {
        GameManager.Instance.onRoundEnd += showDialog;
        rectTransform = GetComponent<RectTransform>();
    }

    void showDialog(List<Cat> deadCats) {
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 1);

        if (deadCats.Count == 0)
            victoryDialog.gameObject.SetActive(true);
        else
            defeatDialog.gameObject.SetActive(true);
    }

    public void Continue() {
        GameManager.Instance.NextRound();
        Close();
    }

    public void Restart() {
        GameManager.Instance.Restart();
        Close();
    }

    void Close () {
        victoryDialog.gameObject.SetActive(false);
        defeatDialog.gameObject.SetActive(false);

        rectTransform.anchorMin = new Vector2(0, -1);
        rectTransform.anchorMax = new Vector2(1, 0);
    }
}
