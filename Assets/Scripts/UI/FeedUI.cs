using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedUI : MonoBehaviour
{
    RectTransform rectTransform;
    public RectTransform rectHUD;

    void Start() {
        rectTransform = GetComponent<RectTransform>();
    }

    public void FeedTheCats() {
        GameManager.Instance.StartRound();

        rectTransform.anchorMin = new Vector2(0, -1);
        rectTransform.anchorMax = new Vector2(1, 0);

        rectHUD.anchorMin = new Vector2(0, 0);
        rectHUD.anchorMax = new Vector2(1, 1);
    }
}
