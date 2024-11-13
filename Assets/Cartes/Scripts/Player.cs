using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Deck deck;
    public Deck fightDeck;
    public int life;
    public int maxLife;
    public bool isShielded;


    public Player(int health, Deck cards) {
        fightDeck = cards;
        life = health;
        maxLife = health;
    }

    public Player(int health) {
        life = health;
        maxLife = health;
        deck = new Deck();
    }

}
