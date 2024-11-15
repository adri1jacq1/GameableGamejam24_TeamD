using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnSystem : MonoBehaviour {

    public static Player player;
    public Deck playerDeckTemplate;

    public GameObject playerShield;
    public GameObject playerLifeBar;
    public TextMeshPro playerLifeText;

    public Player enemy;
    public GameObject enemyObj;
    public static Sprite enemySprite;
    public static EnemyStats enemyStats;
    public Deck enemyDeckTemplate;
    public static List<Card> enemyDeckCards;
    public static int fightIndex;

    public GameObject enemyShield;
    public GameObject enemyLifeBar;
    public TextMeshPro enemyLifeText;

    public static bool isPlayerTurn = false;



    public GameObject backGound;



    public GameObject cardContainer;

    public static bool finished;
    public static bool lose;
    public static bool win;



    public void Start() {


        finished = false;
        win = false;
        lose = false;


        backGound.GetComponent<SpriteRenderer>().sprite = enemyStats.background;
        backGound.transform.localScale *= enemyStats.scaling;

        enemy = new Player(enemyStats.life, new Deck(enemyDeckCards, enemyDeckTemplate));

        enemy.lifeBar = enemyLifeBar;
        enemy.lifeText = enemyLifeText;
        enemy.shield = enemyShield;

        enemy.fightIndex = fightIndex;
        enemy.enemyStats = enemyStats;

        enemy.playerName = "enemy";



        player.fightDeck = new Deck(player.deck.deckCards, playerDeckTemplate);

        player.lifeBar = playerLifeBar;
        player.lifeText = playerLifeText;
        player.shield = playerShield;

        player.playerName = "player";


        player.UpdateLifeBar();
        enemy.UpdateLifeBar();

        enemyObj.GetComponent<SpriteRenderer>().sprite = enemySprite;

        enemy.fightDeck.Shuffle();
        player.fightDeck.Shuffle();

        enemy.fightDeck.DrawCard();
        enemy.fightDeck.DrawCard();
        enemy.fightDeck.DrawCard();
        enemy.fightDeck.DrawCard();

        player.fightDeck.DrawCard();
        player.fightDeck.DrawCard();


        StartCoroutine(PlayBot());
    }

    private IEnumerator PlayBot() {

        if (!finished) {
            yield return new WaitForSeconds(0.1f);

            enemy.fightDeck.DrawCard();

            yield return new WaitForSeconds(0.1f);
            int rand = Random.Range(0, enemy.fightDeck.hand.hand.Count);
            GameObject chosenCard = null;
            float bestGoodModifier = 0;
            float bestBadModifier = 0;
            foreach (GameObject card in enemy.fightDeck.hand.hand) {

                float goodModifier = 0;
                float badModifier = 0;

                CardInfo info = card.GetComponent<CardInfo>();
                foreach (StatusEffectType effectType in info.actions.allEffects.Keys) {
                    if (info.actions.allEffects[effectType].targets == ElligibleTarget.Self) {
                        goodModifier += StatusEffectFactory.GetStatusEffectModifier(effectType) * info.actions.allEffects[effectType].amount;
                    } else if (info.actions.allEffects[effectType].targets == ElligibleTarget.Enemy) {
                        badModifier -= StatusEffectFactory.GetStatusEffectModifier(effectType) * info.actions.allEffects[effectType].amount;
                    } else if (info.actions.allEffects[effectType].targets == ElligibleTarget.Both) {
                        goodModifier += StatusEffectFactory.GetStatusEffectModifier(effectType) * info.actions.allEffects[effectType].amount;
                        badModifier -= StatusEffectFactory.GetStatusEffectModifier(effectType) * info.actions.allEffects[effectType].amount;
                    }

                }
                foreach (StatusEffect playerEffect in player.actions.allEffects.Values) {
                    badModifier -= playerEffect.modifier * playerEffect.amount;
                }

                goodModifier *= (enemy.maxLife - enemy.life);

                if (goodModifier > bestGoodModifier ) {
                    bestGoodModifier = goodModifier;
                    if (bestGoodModifier > bestBadModifier) {
                        chosenCard = card;
                        Debug.Log("New Good best is: " + card.GetComponent<CardInfo>().cardStats.cardName + ": " + goodModifier);
                    }
                }
                if (badModifier > bestBadModifier) {
                    bestBadModifier = badModifier;
                    if (bestBadModifier > bestGoodModifier) {
                        chosenCard = card;
                        Debug.Log("New Bad Best is: " + card.GetComponent<CardInfo>().cardStats.cardName + ": " + badModifier);
                    }
                }
            }

            if (chosenCard == null) {
                chosenCard = enemy.fightDeck.hand.hand[0];
            }

            Debug.Log("Total good for Winner: " + chosenCard.GetComponent<CardInfo>().cardStats.cardName + ": " + bestGoodModifier);
            Debug.Log("Total bad for Winner:" + chosenCard.GetComponent<CardInfo>().cardStats.cardName + ": " + bestBadModifier);

            Player target;
            if (bestGoodModifier > bestBadModifier) {
                target = enemy;
            } else if (bestBadModifier > bestGoodModifier) {
                target = player;
            } else {
                int rando = Random.Range(0, 2);
                if (rando == 0) {
                    target = enemy;
                } else {
                    target = player;
                }
            }

            cardContainer = chosenCard;

            chosenCard.GetComponent<CardInfo>().toggleFront(true);
            enemy.fightDeck.hand.RemoveCard(cardContainer);

            chosenCard.transform.position += new Vector3(0f, 0f, -2f);

            for (int i = 0; i < 10; i++) {
                yield return new WaitForSeconds(0.05f);
                chosenCard.transform.position += new Vector3(0f, -0.1f, 0f);
            }
            yield return FlingCard(chosenCard, target);

            chosenCard.GetComponent<CardInfo>().actions.RunPlayEvent(enemy, target);

        

            enemy.fightDeck.PlaceBackCard(cardContainer);

            yield return new WaitForSeconds(0.1f);

            player.fightDeck.DrawCard();
            isPlayerTurn = true;
        }
    }
    


    public IEnumerator FlingCard(GameObject card, Player character) {
        Vector3 initPos = card.transform.position;
        while (Vector3.Distance(card.transform.position, character.lifeBar.transform.position) > 1f) {
            yield return new WaitForSeconds(0.05f);
            card.transform.position += (character.lifeBar.transform.position - initPos).normalized;
        }
    }
    public IEnumerator UseCard(float xPos, GameObject cardObj) {
        if (!finished) {

            Card stats = cardObj.GetComponent<CardInfo>().cardStats;
            cardContainer = cardObj;
            yield return new WaitForEndOfFrame();
            if (xPos > 960) {
                yield return FlingCard(cardObj, enemy);
                cardObj.GetComponent<CardInfo>().actions.RunPlayEvent(player, enemy);

            } else {
                yield return FlingCard(cardObj, player);
                cardObj.GetComponent<CardInfo>().actions.RunPlayEvent(player, player);
            }
            StartCoroutine(PlayBot());
            player.fightDeck.PlaceBackCard(cardContainer);
        }
    }
}
