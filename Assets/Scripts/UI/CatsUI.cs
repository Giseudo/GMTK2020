using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatsUI : MonoBehaviour {
    public struct CatCardUI {
        public CatCardUI(Text name, Text hunger, GameObject obj) {
            this.name = name;
            this.hunger = hunger;
            this.obj = obj;
        }

        public Text name;
        public Text hunger;
        public GameObject obj;
    }

    public GameObject card;
    public Dictionary<string, CatCardUI> cards;

    void Awake() {
        cards = new Dictionary<string, CatCardUI>();

        CatManager.onAddCat += AddCard;
        CatManager.onRemoveCat += RemoveCard;
    }

    void OnDestroy() {
        CatManager.onAddCat -= AddCard;
        CatManager.onRemoveCat -= RemoveCard;
    }

    void UpdateHunger(Cat cat) {
        cards[cat.data.nick].hunger.text = cat.Hunger.ToString();
    }

    void AddCard(Cat cat) {
        GameObject catCard = Instantiate(card, transform);

        Text name = catCard.transform.GetChild(0).GetComponent<Text>();
        name.text = cat.data.nick;

        Text hunger = catCard.transform.GetChild(1).GetComponent<Text>();
        hunger.text = cat.Hunger.ToString();

        cards.Add(cat.data.nick, new CatCardUI(name, hunger, catCard));

        cat.onHungerChange += UpdateHunger;
    }

    void RemoveCard(Cat cat) {
        CatCardUI card = cards[cat.data.nick];

        Destroy(card.obj);

        cards.Remove(cat.data.nick);
    }
}
