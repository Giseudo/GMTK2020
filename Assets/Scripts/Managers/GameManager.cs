using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    int round = 1;
    public bool IsPlaying => playing;
    public bool IsStarted => started;
    bool playing = false;
    bool started = false;
	public static GameManager Instance = null;
    public delegate void OnRoundStart();
    public delegate void OnRoundEnd(List<Cat> deadCats);
    public OnRoundStart onRoundStart;
    public OnRoundEnd onRoundEnd;
    public int catsPerRound = 2;


	void Awake() {
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy(gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}

    void Start() {
        CatManager.EnableCat(catsPerRound);
        BowlManager.EnableBowl(catsPerRound);
    }

    void Update() {
        if (BowlManager.RemainingMeal() <= 1f && started && playing)
            EndRound();
    }

    public void StartRound() {
        if (onRoundStart != null) onRoundStart();

        playing = true;
        started = true;

        BowlManager.PlaceFood();
    }

    public void EndRound() {
        List<Cat> deadCats = CatManager.GetDeadCats();

        if (onRoundEnd != null) onRoundEnd(deadCats);

        playing = false;

        if (deadCats.Count > 0) return;

        CatManager.EnableCat(catsPerRound);
        BowlManager.EnableBowl(catsPerRound);
    }
}
