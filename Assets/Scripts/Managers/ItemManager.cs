using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour {
    public class Item {
        public int amount { get; private set; }
        int initialAmount;
        protected GameObject prefab;
        public GameObject instance;
        protected Transform parent;
        public delegate void OnUse();
        public OnUse onUse;

        public Item(int amount, GameObject prefab, Transform parent) {
            this.amount = amount;
            this.initialAmount = amount;
            this.prefab = prefab;
            this.parent = parent;
        }

        public virtual void Use() {
            if (amount == 0) return;

            if (onUse != null) onUse();

            SetAmount(amount - 1);
        }

        public void SetAmount(int amount) {
            this.amount = amount;
        }

        public void Destroy() {
            if (instance != null)
                GameObject.Destroy(instance);
        }

        public virtual void Reset() {
            amount = initialAmount;
        }
    }

    public class SprinklerItem : Item {
        public SprinklerItem(int amount, GameObject prefab, Transform parent) : base (amount, prefab, parent) { }

        public void Use(Vector3 position) {
            base.Use();

            instance = Instantiate(prefab, parent);
            instance.transform.position = position;

            // TODO Play soound
        }
    }

    public class SnackItem : Item {
        public bool dropped = false;

        public SnackItem(int amount, GameObject prefab, Transform parent) : base(amount, prefab, parent) { }

        public void Move(Vector3 position) {
            if (dropped || instance.transform == null) return;

            if (position.magnitude == 0) {
                instance.transform.position = Vector3.down * 1000f;
                return;
            }

            instance.transform.position = position;
        }
        
        public void Show(Vector3 position) {
            instance = Instantiate(prefab, parent);
            instance.transform.position = position;
            dropped = false;
            instance.SetActive(true);
        }

        public void Drop() {
            dropped = true;
            instance.GetComponent<DestroyAfterTime>().Enable();
        }

        public override void Use() {
            base.Use();

            Drop();
        }
    }

    public class YarnItem : Item {
        public bool dropped = false;
        public YarnItem(int amount, GameObject prefab, Transform parent) : base (amount, prefab, parent) { }
        
        public void Show() {
            instance = Instantiate(prefab, parent);
            dropped = false;
        }

        public void Drop() {
            dropped = true;
        }

        public void Use(Vector3 origin, Vector3 target) {
            base.Use();
            Show();

            Rigidbody body = instance.GetComponent<Rigidbody>();

            instance.transform.position = origin;

            body.velocity = Vector3.zero;
            body.AddForce((target - origin).normalized * 50f, ForceMode.VelocityChange);

            Drop();
        }
    }

    public SprinklerItem sprinkler;
    public SnackItem snack;
    public YarnItem yarnBall;
    public GameObject snackPrefab;
    public GameObject yarnBallPrefab;
    public GameObject sprinklerPrefab;
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
        snack = new SnackItem(3, snackPrefab, transform);
        sprinkler = new SprinklerItem(5, sprinklerPrefab, transform);
        yarnBall = new YarnItem(2, yarnBallPrefab, transform);
    }
}