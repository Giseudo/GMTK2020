using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public bool IsPlaying => playing;
    public bool IsStarted => started;
    bool playing = false;
    bool started = false;
	public static GameManager Instance = null;
    public delegate void OnRoundStart(int round);
    public delegate void OnRoundEnd(List<Cat> deadCats);
    public OnRoundStart onRoundStart;
    public OnRoundEnd onRoundEnd;
    public int catsPerRound = 3;
    public int bowlsPerRound = 2;
    int round = 0;


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

        round++;

        if (onRoundStart != null) onRoundStart(round);

        playing = true;
        started = true;

        SoundManager.Instance.Play("Start", .5f);
    }

    public void EndRound() {
        List<Cat> deadCats = CatManager.GetDeadCats();

        if (onRoundEnd != null) onRoundEnd(deadCats);

        if (deadCats.Count > 0)
            SoundManager.Instance.Play("Lose");
        else
            SoundManager.Instance.Play("Win", .5f);

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
        BowlManager.EnableBowl(bowlsPerRound);
        CatManager.EnableCat(catsPerRound);
    }
}
