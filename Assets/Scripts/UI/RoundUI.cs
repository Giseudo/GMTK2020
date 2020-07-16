using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundUI : MonoBehaviour
{
    Text text;

    void Start() {
        GameManager.Instance.onRoundStart += UpdateRoundNumber;
        text = GetComponent<Text>();
    }

    void UpdateRoundNumber(int round) {
        text.text = string.Format("Round {0}", round);      
    }
}
