using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<Card> deckCards = new ();
    public Hand hand;
    [SerializeField] private float scale;
    [SerializeField] private bool isEnemy;


    public List<Card> deck = new();


    public void Start() {
    }

    public void DrawCard() {
        GameObject givenCard;
        if (deck.Count > 0) {
            givenCard = DisplayCard.CreateCard(deck[0]);
            hand.AddCard(givenCard);
            givenCard.transform.localScale = new Vector3(scale, scale, scale);
            givenCard.GetComponent<Drag>().initScale = new Vector3(scale, scale, scale);
            givenCard.GetComponent<Drag>().hand = hand;
            if (isEnemy) {
                givenCard.GetComponent<CardStyle>().toggleFront(false);
            }
            deck.RemoveAt(0);
        }

    }

    public void PlaceBackCard(GameObject card) {
        deck.Add(card.GetComponent<CardStyle>().cardStats);
        Destroy(card);
    }

}
