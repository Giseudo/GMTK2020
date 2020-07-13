using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatsUI : MonoBehaviour {
    public struct CatCardUI {
        public CatCardUI(Text name, Transform hunger, GameObject obj) {
            this.name = name;
            this.hunger = hunger;
            this.obj = obj;
        }

        public Text name;
        public Transform hunger;
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
        float scale = Mathf.InverseLerp(cat.data.hunger.InitialValue * 2f, 0f, cat.Hunger);
        cards[cat.data.nick].hunger.localScale = new Vector3(scale, 1f, 1f);
    }

    void AddCard(Cat cat) {
        GameObject catCard = Instantiate(card, transform);

        Transform hunger = catCard.transform.GetChild(0);
        Text name = catCard.transform.GetChild(1).GetComponent<Text>();
        name.text = cat.data.nick;

        cards.Add(cat.data.nick, new CatCardUI(name, hunger, catCard));

        cat.onHungerChange += UpdateHunger;
        UpdateHunger(cat);
    }

    void RemoveCard(Cat cat) {
        CatCardUI card = cards[cat.data.nick];

        Destroy(card.obj);

        cards.Remove(cat.data.nick);
    }
}
