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
        float droppedTime;
        GameObject snack;

        public SnackItem(int amount, GameObject snack) : base(amount) {
            this.snack = snack;
            Hide();
        }

        public void Move(Vector3 position) {
            dropped = false;
            snack.transform.position = position;
            Show();
        }
        
        public void Show() {
            snack.SetActive(true);
        }

        public void Hide() {
            snack.SetActive(false);
        }

        public void Drop() {
            droppedTime = Time.unscaledTime;
            dropped = true;
        }

        public override void Use() {
            base.Use();
            Drop();
        }
    }

    public SprinklerItem sprinkler = new SprinklerItem();
    public SnackItem snack;
    public Item yarnBall = new Item(2);

    public GameObject snackPrefab;

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
        snack = new SnackItem(2, Instantiate(snackPrefab, transform));
    }
}