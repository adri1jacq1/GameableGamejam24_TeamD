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

    public static Player enemy;
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


        player.fightDeck = new Deck(player.deck.deckCards, playerDeckTemplate);

        player.lifeBar = playerLifeBar;
        player.lifeText = playerLifeText;
        player.shield = playerShield;

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

            bool chosen = false;
            yield return new WaitForSeconds(0.1f);
            GameObject card = null;
            Card stats = null;
            int rand = Random.Range(0, enemy.fightDeck.hand.hand.Count);
            for (int i = 0; i < enemy.fightDeck.hand.hand.Count; i++) {  /*quand joueur shielded vaut plus la peine de heal*/
                card = enemy.fightDeck.hand.hand[rand];
                stats = card.GetComponent<CardStyle>().cardStats;
                if (player.actions.effectsList.ContainsKey("shield") && stats.healing > stats.attack) {
                    Debug.Log("quand joueur shielded vaut plus la peine de heal");
                    chosen = true;
                    break;
                }
                rand++;
                rand %= enemy.fightDeck.hand.hand.Count;
            }
            if (!chosen) {
                for (int i = 0; i < enemy.fightDeck.hand.hand.Count; i++) { /*sinon si on peut le tuer... :) */
                    card = enemy.fightDeck.hand.hand[rand];
                    stats = card.GetComponent<CardStyle>().cardStats;
                    if (!player.actions.effectsList.ContainsKey("shield") && player.life <= stats.attack) {
                        Debug.Log("sinon si on peut le tuer... :)");
                        chosen = true;
                        break;
                    }
                    rand++;
                    rand %= enemy.fightDeck.hand.hand.Count;
                }
            }
            if (!chosen) {
                for (int i = 0; i < enemy.fightDeck.hand.hand.Count; i++) { /*sinon vaut plus la peine d'attaquer (heal innutile)*/
                    card = enemy.fightDeck.hand.hand[rand];
                    stats = card.GetComponent<CardStyle>().cardStats;
                    if (!player.actions.effectsList.ContainsKey("shield") && enemy.maxLife - enemy.life < stats.attack) {
                        Debug.Log("sinon vaut plus la peine d'attaquer (heal innutile)");
                        chosen = true;
                        break;
                    }
                    rand++;
                    rand %= enemy.fightDeck.hand.hand.Count;
                }
            }
            if (!chosen) {
                for (int i = 0; i < enemy.fightDeck.hand.hand.Count; i++) { /*sinon vaut plus la peine de heal (totalité)*/
                    card = enemy.fightDeck.hand.hand[rand];
                    stats = card.GetComponent<CardStyle>().cardStats;
                    if (stats.healing > stats.attack && enemy.maxLife - enemy.life >= stats.healing) {
                        Debug.Log("sinon vaut plus la peine de heal (totalité)");
                        chosen = true;
                        break;
                    }
                    rand++;
                    rand %= enemy.fightDeck.hand.hand.Count;
                }
            }
            if (!chosen) {
                for (int i = 0; i < enemy.fightDeck.hand.hand.Count; i++) { /*sinon si on peut shield*/
                    card = enemy.fightDeck.hand.hand[rand];
                    stats = card.GetComponent<CardStyle>().cardStats;
                    if (stats.givesDefense) {
                        Debug.Log("sinon si on peut shield");
                        break;
                    }
                    rand++;
                    rand %= enemy.fightDeck.hand.hand.Count;
                }
            }
            //Sinon on prend une carte random

            enemy.fightDeck.hand.RemoveCard(card);
            card.GetComponent<CardStyle>().toggleFront(true);

            card.transform.position += new Vector3(0f, 0f, -2f);

            for (int i = 0; i < 10; i++) {
                yield return new WaitForSeconds(0.05f);
                card.transform.position += new Vector3(0f, -0.1f, 0f);
            }

            yield return new WaitForSeconds(0.1f);
            cardContainer = card;
            if (stats.givesDefense || (stats.healing > stats.attack && enemy.life < enemy.maxLife)) {
                enemy.fightDeck.hand.RemoveCard(cardContainer);
                Vector3 initPos = card.transform.position;
                while (Vector3.Distance(card.transform.position, enemyLifeBar.transform.position) > 1f) {
                    yield return new WaitForSeconds(0.05f);
                    card.transform.position += (enemyLifeBar.transform.position - initPos).normalized;
                }
                StartCoroutine(Heal(enemy, stats.healing)); if (stats.givesDefense) {
                    enemy.actions.AddPlayerStatusEffect(new PlayerStatusEffectBlock(enemy, 1));
                }

            } else {
                enemy.fightDeck.hand.RemoveCard(cardContainer);
                Vector3 initPos = card.transform.position;
                while (Vector3.Distance(card.transform.position, playerLifeBar.transform.position) > 1f) {
                    yield return new WaitForSeconds(0.05f);
                    card.transform.position += (playerLifeBar.transform.position - initPos).normalized;
                }
                StartCoroutine(Damage(player, stats.attack));

            }

            enemy.fightDeck.PlaceBackCard(cardContainer);
            yield return new WaitForSeconds(0.1f);

            yield return new WaitForSeconds(0.1f);

            player.fightDeck.DrawCard();
            isPlayerTurn = true;
        }
    }

    public IEnumerator UseCard(float xPos, GameObject cardObj) {
        if (!finished) {

            Card stats = cardObj.GetComponent<CardStyle>().cardStats;
            cardContainer = cardObj;
            yield return new WaitForEndOfFrame();
            if (xPos > 960) {
                Vector3 initPos = cardObj.transform.position;
                while (Vector3.Distance(cardObj.transform.position, enemyLifeBar.transform.position) > 1f) {
                    yield return new WaitForSeconds(0.05f);
                    cardObj.transform.position += (enemyLifeBar.transform.position - initPos).normalized;
                }
                StartCoroutine(Damage(enemy,stats.attack));

            } else {
                Vector3 initPos = cardObj.transform.position;
                while (Vector3.Distance(cardObj.transform.position, playerLifeBar.transform.position) > 1f) {
                    yield return new WaitForSeconds(0.05f);
                    cardObj.transform.position += (playerLifeBar.transform.position - initPos).normalized;
                }
                StartCoroutine(Heal(player, stats.healing));
                if (stats.givesDefense) {
                    player.actions.AddPlayerStatusEffect(new PlayerStatusEffectBlock(player, 1));
                }   //remplacer par un applyStatusEffectBlock

            }
            StartCoroutine(PlayBot());
            player.fightDeck.PlaceBackCard(cardContainer);
        }
    }

    public IEnumerator Damage(Player target, int attack) {
        StartCoroutine(target.actions.ChangeLife(-1 * attack));
        yield return new WaitForSeconds(0.01f);
        AudioFXManager.Instance.PlaySound("damage");
    }

    public IEnumerator Heal(Player target, int healing) {
        StartCoroutine(target.actions.ChangeLife(healing));
        yield return new WaitForSeconds(0.01f);
        AudioFXManager.Instance.PlaySound("heal");
    }
}
