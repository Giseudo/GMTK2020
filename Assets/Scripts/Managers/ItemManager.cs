using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {
    public class Item {
        int amount;
        int initialAmount;
        public delegate void OnUse();
        public OnUse onUse;

        public Item(int amount = 1000) {
            this.amount = amount;
            this.initialAmount = amount;
        }

        public virtual void Use() {
            if (this.amount == 0) return;

            if (onUse != null) onUse();

            this.amount--;
        }

        public virtual void Reset() {
            amount = initialAmount;
        }
    }

    public class SprinklerItem : Item {
        public SprinklerItem() { }

        public void Use(Cat cat) {
            base.Use();

            cat.Scare();
        }
    }

    public class SnackItem : Item {
        public bool dropped = false;
        GameObject snack;

        public SnackItem(int amount, GameObject snack) : base(amount) {
            this.snack = snack;
            Hide();
        }

        public void Move(Vector3 position) {
            if (dropped) return;

            snack.transform.position = position;
        }
        
        public void Show() {
            dropped = false;
            snack.SetActive(true);
        }

        public void Hide() {
            snack.SetActive(false);
        }

        public void Drop() {
            dropped = true;
        }

        public override void Use() {
            base.Use();
            Drop();
        }
    }

    public class YarnItem : Item {
        public bool dropped = false;
        GameObject yarnBall;
        public YarnItem(int amount, GameObject yarnBall) : base (amount) {
            this.yarnBall = yarnBall;
            Hide();
        }
        
        public void Show() {
            dropped = false;
            yarnBall.SetActive(true);
        }

        public void Hide() {
            yarnBall.SetActive(false);
        }

        public void Drop() {
            dropped = true;
        }

        public void Use(Vector3 origin, Vector3 target) {
            base.Use();
            Rigidbody body = yarnBall.GetComponent<Rigidbody>();

            yarnBall.transform.position = origin;
            Show();

            body.velocity = Vector3.zero;
            body.AddForce((target - origin).normalized * 10f, ForceMode.VelocityChange);

            Drop();
        }
    }

    public SprinklerItem sprinkler;
    public SnackItem snack;
    public YarnItem yarnBall;
    public GameObject snackPrefab;
    public GameObject yarnBallPrefab;
	public static ItemManager Instance = null;

	void Awake() {
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy(gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}

    void Start() {
        sprinkler = new SprinklerItem();
        snack = new SnackItem(2, Instantiate(snackPrefab, transform));
        yarnBall = new YarnItem(1, Instantiate(yarnBallPrefab, transform));
    }
}