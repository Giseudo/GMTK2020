using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatsUI : MonoBehaviour {
    public struct CatCardUI {
        public CatCardUI(Text name, Text hunger) {
            this.name = name;
            this.hunger = hunger;
        }

        public Text name;
        public Text hunger;
    }

    public GameObject card;
    public Dictionary<string, CatCardUI> cards = new Dictionary<string, CatCardUI>();

    void Start() {
        foreach (Cat cat in CatManager.cats) {
            GameObject catCard = Instantiate(card, transform);

            Text name = catCard.transform.GetChild(0).GetComponent<Text>();
            name.text = cat.data.nick;

            Text hunger = catCard.transform.GetChild(1).GetComponent<Text>();
            hunger.text = cat.Hunger.ToString();

            catCard.transform.parent = transform;

            cards.Add(cat.data.nick, new CatCardUI(name, hunger));

            cat.onHungerChange += UpdateHunger;
        }
    }

    void UpdateHunger(Cat cat) {
        cards[cat.data.nick].hunger.text = cat.Hunger.ToString();
    }
}
