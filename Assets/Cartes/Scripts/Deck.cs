using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<Card> deckCards = new ();
    public Hand hand = new Hand();
    public float scale;
    public bool isEnemy;

    public Deck() {
    }
    public Deck(List<Card> cards, Deck template) {
        foreach (Card card in cards) {
            deckCards.Add(card);
        }
        hand = template.hand;
        scale = template.scale;
        isEnemy = template.isEnemy;
    }

    public Card container;

    public void Start() {
    }

    public void DrawCard() {
        GameObject givenCard;
        if (deckCards.Count > 0) {
            givenCard = DisplayCard.CreateCard(deckCards[0]);
            Debug.Log(deckCards.Count);
            Debug.Log(givenCard);
            hand.AddCard(givenCard);
            givenCard.transform.localScale = new Vector3(scale, scale, scale);
            givenCard.GetComponent<Drag>().initScale = new Vector3(scale, scale, scale);
            givenCard.GetComponent<Drag>().hand = hand;
            if (isEnemy) {
                givenCard.GetComponent<CardStyle>().toggleFront(false);
            }
            deckCards.RemoveAt(0);
        }

    }

    public void PlaceBackCard(GameObject card) {
        deckCards.Add(card.GetComponent<CardStyle>().cardStats);
        Destroy(card);
    }

    public void Shuffle() {
        for (int index = 0; index < deckCards.Count; index++) {
            int rand = Random.Range(index, deckCards.Count);
            container = deckCards[index];
            deckCards[index] = deckCards[rand];
            deckCards[rand] = container;
        }



    }

}
