using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnSystem : MonoBehaviour {

    public static Player player;
    public static Player enemy;
    public static bool isPlayerTurn = false;

    public Deck playerDeckTemplate;
    public Deck enemyDeckTemplate;

    public GameObject playerLifeBar;
    public GameObject enemyLifeBar;

    public static List<Card> enemyDeckCards;

    public GameObject backGound;

    public float modifier;

    public AudioClip punchSound;
    public AudioClip healSound;
    public AudioClip shieldSound;

    public AudioSource source;

    public GameObject playerShield;
    public GameObject enemyShield;


    public TextMeshPro playerLifeText;
    public TextMeshPro enemyLifeText;

    public GameObject enemyObj;

    public GameObject cardContainer;

    public bool finished = false;
    public bool lose = false;
    public bool win = false;

    [SerializeField] private bool isEnemy;

    public static int fightIndex;
    public static Sprite enemySprite;

    public static EnemyStats enemyStats;


    private void UpdateLifeBar(Player p, GameObject lifeBar, TextMeshPro text) {
        lifeBar.transform.localScale = new Vector2((p.life) * 1f/p.maxLife * modifier, lifeBar.transform.localScale.y);
        text.text = p.life + "/" + p.maxLife;
    }

    public void Start() {

        backGound.GetComponent<SpriteRenderer>().sprite = enemyStats.background;
        backGound.transform.localScale *= enemyStats.scaling;

        enemy = new Player(enemyStats.life, new Deck(enemyDeckCards, enemyDeckTemplate));

        player.fightDeck = new Deck(player.deck.deckCards, playerDeckTemplate);


        UpdateLifeBar(enemy, enemyLifeBar, enemyLifeText);
        UpdateLifeBar(player, playerLifeBar, playerLifeText);

        enemyObj.GetComponent<SpriteRenderer>().sprite = enemySprite;

        int i = 0;


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

            if (enemy.isShielded) {
                enemyShield.SetActive(false);
                enemy.isShielded = false;
            }

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
                if (player.isShielded && stats.healing > stats.attack) {
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
                    if (!player.isShielded && player.life <= stats.attack) {
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
                    if (!player.isShielded && enemy.maxLife - enemy.maxLife < stats.attack) {
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
                StartCoroutine(HealEnemy(stats.healing));
                StartCoroutine(ShieldEnemy(stats.givesDefense));

            } else {
                enemy.fightDeck.hand.RemoveCard(cardContainer);
                Vector3 initPos = card.transform.position;
                while (Vector3.Distance(card.transform.position, playerLifeBar.transform.position) > 1f) {
                    yield return new WaitForSeconds(0.05f);
                    card.transform.position += (playerLifeBar.transform.position - initPos).normalized;
                }
                StartCoroutine(DamageSelf(stats.attack));

            }

            enemy.fightDeck.PlaceBackCard(cardContainer);
            yield return new WaitForSeconds(0.1f);

            if (player.isShielded) {
                playerShield.SetActive(false);
                player.isShielded = false;
            }

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
                StartCoroutine(DamageEnemy(stats.attack));

            } else {
                Vector3 initPos = cardObj.transform.position;
                while (Vector3.Distance(cardObj.transform.position, playerLifeBar.transform.position) > 1f) {
                    yield return new WaitForSeconds(0.05f);
                    cardObj.transform.position += (playerLifeBar.transform.position - initPos).normalized;
                }
                StartCoroutine(HealSelf(stats.healing));
                StartCoroutine(ShieldSelf(stats.givesDefense));

            }
            StartCoroutine(PlayBot());
            player.fightDeck.PlaceBackCard(cardContainer);
        }
    }

    public IEnumerator DamageEnemy(int attack) {
        StartCoroutine(ChangeLifeEnemy(-1 * attack));
        yield return new WaitForSeconds(0.01f);
        source.PlayOneShot(punchSound);
    }

    public IEnumerator DamageSelf(int attack) {
        StartCoroutine(ChangeLifeSelf(-1 * attack));
        yield return new WaitForSeconds(0.01f);
        source.PlayOneShot(punchSound);
    }

    public IEnumerator HealSelf(int healing) {
        StartCoroutine(ChangeLifeSelf(healing));
        yield return new WaitForSeconds(0.01f);
        source.PlayOneShot(healSound);
    }

    public IEnumerator HealEnemy(int healing) {
        StartCoroutine(ChangeLifeEnemy(healing));
        yield return new WaitForSeconds(0.01f);
        source.PlayOneShot(healSound);
    }
    public IEnumerator ShieldSelf(bool canShield) {
        if (canShield) {
            yield return new WaitForSeconds(0.01f);
            playerShield.SetActive(true);
            player.isShielded = true;
            source.PlayOneShot(shieldSound);
        }
    }
    public IEnumerator ShieldEnemy(bool canShield) {
        if (canShield) {
            yield return new WaitForSeconds(0.01f);
            enemyShield.SetActive(true);
            enemy.isShielded = true;
            source.PlayOneShot(shieldSound);
        }
    }


    public IEnumerator ChangeLifeSelf(int life) {
        if (!player.isShielded) {
            player.life += life;
            if (player.life > player.maxLife) {
                player.life = player.maxLife;
            }
            if (player.life <= 0) {
                playerLifeBar.SetActive(false);
                playerLifeText.text = 0 + "";
                finished = true;
                yield return new WaitForSeconds(1f);
                lose = true;
            } else {
                UpdateLifeBar(player, playerLifeBar, playerLifeText);
            }
        }
    }

    public IEnumerator ChangeLifeEnemy(int life) {
        if (!enemy.isShielded) {
            enemy.life += life;
            if (enemy.life > enemy.maxLife) {
                enemy.life = enemy.maxLife;
            }
            if (enemy.life <= 0) {
                enemyLifeBar.SetActive(false);
                enemyLifeText.text = 0 + "";
                finished = true;
                yield return new WaitForSeconds(1f);
                win = true;
                EnemySpawning.canSpawn[fightIndex] = false;

                for (int i = 0; i < enemyStats.cardDrops.Count; i++) {
                    int rand = Random.Range(0, 100);
                    if (rand < enemyStats.cardChance[i]) {
                        player.deck.deckCards.Add(enemyStats.cardDrops[i]);
                    }
                }
            } else {
                UpdateLifeBar(enemy, enemyLifeBar, enemyLifeText);
            }
        }
    }

}
