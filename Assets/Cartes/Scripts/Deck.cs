using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] private List<Card> deckCards = new ();

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
            deck.RemoveAt(0);
        }

    }

}
