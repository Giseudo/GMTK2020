using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	}

    void Start() {
        SpawnCats();
    }

    void Update() {
        if (BowlManager.RemainingMeal() <= 1f && started && playing)
            EndRound();
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartRound() {
        if (playing) return;

        if (onRoundStart != null) onRoundStart();

        playing = true;
        started = true;

        CatManager.FeedTheCats();
    }

    public void EndRound() {
        List<Cat> deadCats = CatManager.GetDeadCats();

        if (onRoundEnd != null) onRoundEnd(deadCats);

        playing = false;
    }

    public void NextRound() {
        ItemManager.Instance.yarnBall.Reset();
        ItemManager.Instance.snack.Reset();
        ItemManager.Instance.sprinkler.Reset();

        SpawnCats();
        StartRound();
    }

    public void SpawnCats() {
        BowlManager.EnableBowl(catsPerRound);
        CatManager.EnableCat(catsPerRound);
    }
}
