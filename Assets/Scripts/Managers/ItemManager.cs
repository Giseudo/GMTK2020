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
        public SprinklerItem() {

        }

        public void Use(Cat cat) {
            base.Use();

            cat.Scare();
        }
    }

    public static SprinklerItem Sprinkler = new SprinklerItem();
    public static Item Snacks = new Item(2);
    public static Item YarnBall = new Item(2);

    public void Start() {
    }

    public void Reset() {

    }
}
