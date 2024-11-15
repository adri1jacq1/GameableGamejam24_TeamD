using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour {
    public string playerName;
    public Deck deck;
    public Deck fightDeck;
    public int life;
    public int maxLife;

    public GameObject shield;

    public EnemyStats enemyStats;

    public GameObject lifeBar;
    public TextMeshPro lifeText;

    public PlayerActions actions;

    public int fightIndex;


    public void UpdateLifeBar() {
        lifeBar.transform.localScale = new Vector2((life) * 1f / maxLife * 3f, lifeBar.transform.localScale.y);
        lifeText.text = life + "/" + maxLife;
    }


    public Player(int health, Deck cards) {
        fightDeck = cards;
        life = health;
        maxLife = health;
        actions = new PlayerActions(this);
    }

    public Player(int health) {
        life = health;
        maxLife = health;
        deck = new Deck();
        actions = new PlayerActions(this);
    }


    public IEnumerator CardRewards(EnemyStats stats) {
        for (int i = 0; i < stats.cardDrops.Count; i++) {
            int rand = Random.Range(0, 100);
            if (rand < stats.cardChance[i]) {
                yield return new WaitForEndOfFrame(); //Rajouter l'anim de gagner une carte
                deck.deckCards.Add(stats.cardDrops[i]);
            }
        }
        TurnSystem.win = true;
    } 

}
