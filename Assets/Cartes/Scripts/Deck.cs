using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] private List<Card> deckCards = new ();
    [SerializeField] private Hand hand;
    [SerializeField] private float scale;

    private List<Card> deck = new();

   




    public void Start() {
        foreach (Card card in deckCards) {
            deck.Add(card);
        }
    }

    public void DrawCard() {
        GameObject givenCard;
        if (deck.Count > 0) {
            givenCard = DisplayCard.CreateCard(deck[0]);
            hand.AddCard(givenCard);
            givenCard.transform.localScale = new Vector3(scale, scale, scale);
            givenCard.GetComponent<Drag>().hand = hand;
            givenCard.GetComponent<Drag>().initScale = new Vector3(scale, scale, scale);
            deck.RemoveAt(0);
        }

    }

}
