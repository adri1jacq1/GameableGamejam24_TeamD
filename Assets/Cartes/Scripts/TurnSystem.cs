using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnSystem : MonoBehaviour {
    public static bool isPlayerTurn = false;

    public Deck playerDeck;
    public Deck enemyDeck;
    public GameObject playerLifeBar;
    public GameObject enemyLifeBar;

    public static List<Card> enemyDeckCards;

    public GameObject backGound;

    public float modifier;

    public bool playerShielded = false;
    public bool enemyShielded = false;

    public AudioClip punchSound;
    public AudioClip healSound;
    public AudioClip shieldSound;

    public AudioSource source;

    public Player player;

    public GameObject playerShield;
    public GameObject enemyShield;

    public int playerLife = 10;
    public int playerMaxLife = 10;
    static public int enemyLife;
    static public int enemyMaxLife;

    public TextMeshPro playerLifeText;
    public TextMeshPro enemyLifeText;
    public GameObject enemy;

    public GameObject cardContainer;

    public bool finished = false;
    public bool lose = false;
    public bool win = false;

    [SerializeField] private bool isEnemy;

    public static int fightIndex;
    public static Sprite enemySprite;

    public static EnemyStats enemyStats;


    private void UpdateLifeBar(int life, int maxLife, GameObject lifeBar, TextMeshPro text) {
        lifeBar.transform.localScale = new Vector2((life) * 1f/maxLife * modifier, lifeBar.transform.localScale.y);
        text.text = life + "/" + maxLife;
    }

    public void Start() {

        backGound.GetComponent<SpriteRenderer>().sprite = enemyStats.background;
        backGound.transform.localScale *= enemyStats.scaling;

        playerMaxLife = playerLife;
        enemyMaxLife = enemyLife;

        UpdateLifeBar(enemyLife, enemyMaxLife, enemyLifeBar, enemyLifeText);
        UpdateLifeBar(playerLife, playerMaxLife, playerLifeBar, playerLifeText);

        enemy.GetComponent<SpriteRenderer>().sprite = enemySprite;

        int i = 0;

        playerDeck = new Deck(Player.deck.deckCards, playerDeck);
        enemyDeck = new Deck(enemyDeckCards, enemyDeck);


        enemyDeck.Shuffle();
        playerDeck.Shuffle();

        enemyDeck.DrawCard();
        enemyDeck.DrawCard();
        enemyDeck.DrawCard();
        enemyDeck.DrawCard();

        playerDeck.DrawCard();
        playerDeck.DrawCard();


        StartCoroutine(PlayBot());
    }

    private IEnumerator PlayBot() {

        if (!finished) {

            if (enemyShielded) {
                enemyShield.SetActive(false);
                enemyShielded = false;
            }

            yield return new WaitForSeconds(0.1f);

            enemyDeck.DrawCard();

            bool chosen = false;
            yield return new WaitForSeconds(0.1f);
            GameObject card = null;
            Card stats = null;
            int rand = Random.Range(0, enemyDeck.hand.hand.Count);
            for (int i = 0; i < enemyDeck.hand.hand.Count; i++) {  /*quand joueur shielded vaut plus la peine de heal*/
                card = enemyDeck.hand.hand[rand];
                stats = card.GetComponent<CardStyle>().cardStats;
                if (playerShielded && stats.healing > stats.attack) {
                    chosen = true;
                    break;
                }
                rand++;
                rand %= enemyDeck.hand.hand.Count;
            }
            if (!chosen) {
                for (int i = 0; i < enemyDeck.hand.hand.Count; i++) { /*sinon si on peut le tuer... :) */
                    card = enemyDeck.hand.hand[rand];
                    stats = card.GetComponent<CardStyle>().cardStats;
                    if (!playerShielded && playerLife <= stats.attack) {
                        chosen = true;
                        break;
                    }
                    rand++;
                    rand %= enemyDeck.hand.hand.Count;
                }
            }
            if (!chosen) {
                for (int i = 0; i < enemyDeck.hand.hand.Count; i++) { /*sinon vaut plus la peine d'attaquer (heal innutile)*/
                    card = enemyDeck.hand.hand[rand];
                    stats = card.GetComponent<CardStyle>().cardStats;
                    if (!playerShielded && enemyMaxLife - enemyLife < stats.attack) {
                        chosen = true;
                        break;
                    }
                    rand++;
                    rand %= enemyDeck.hand.hand.Count;
                }
            }
            if (!chosen) {
                for (int i = 0; i < enemyDeck.hand.hand.Count; i++) { /*sinon vaut plus la peine de heal (totalité)*/
                    card = enemyDeck.hand.hand[rand];
                    stats = card.GetComponent<CardStyle>().cardStats;
                    if (stats.healing > stats.attack && enemyMaxLife - enemyLife >= stats.healing) {
                        chosen = true;
                        break;
                    }
                    rand++;
                    rand %= enemyDeck.hand.hand.Count;
                }
            }
            if (!chosen) {
                for (int i = 0; i < enemyDeck.hand.hand.Count; i++) { /*sinon si on peut shield*/
                    card = enemyDeck.hand.hand[rand];
                    stats = card.GetComponent<CardStyle>().cardStats;
                    if (stats.givesDefense) {
                        break;
                    }
                    rand++;
                    rand %= enemyDeck.hand.hand.Count;
                }
            }
            //Sinon on prend une carte random

            enemyDeck.hand.RemoveCard(card);
            card.GetComponent<CardStyle>().toggleFront(true);

            card.transform.position += new Vector3(0f, 0f, -2f);

            for (int i = 0; i < 10; i++) {
                yield return new WaitForSeconds(0.05f);
                card.transform.position += new Vector3(0f, -0.1f, 0f);
            }

            yield return new WaitForSeconds(0.1f);
            cardContainer = card;
            if (stats.givesDefense || (stats.healing > stats.attack && enemyLife < enemyMaxLife)) {
                enemyDeck.hand.RemoveCard(cardContainer);
                Vector3 initPos = card.transform.position;
                while (Vector3.Distance(card.transform.position, enemyLifeBar.transform.position) > 1f) {
                    yield return new WaitForSeconds(0.05f);
                    card.transform.position += (enemyLifeBar.transform.position - initPos).normalized;
                }
                StartCoroutine(HealEnemy(stats.healing));
                StartCoroutine(ShieldEnemy(stats.givesDefense));

            } else {
                enemyDeck.hand.RemoveCard(cardContainer);
                Vector3 initPos = card.transform.position;
                while (Vector3.Distance(card.transform.position, playerLifeBar.transform.position) > 1f) {
                    yield return new WaitForSeconds(0.05f);
                    card.transform.position += (playerLifeBar.transform.position - initPos).normalized;
                }
                StartCoroutine(DamageSelf(stats.attack));

            }

            enemyDeck.PlaceBackCard(cardContainer);
            yield return new WaitForSeconds(0.1f);

            if (playerShielded) {
                playerShield.SetActive(false);
                playerShielded = false;
            }

            yield return new WaitForSeconds(0.1f);

            playerDeck.DrawCard();
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
            playerDeck.PlaceBackCard(cardContainer);
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
            playerShielded = true;
            source.PlayOneShot(shieldSound);
        }
    }
    public IEnumerator ShieldEnemy(bool canShield) {
        if (canShield) {
            yield return new WaitForSeconds(0.01f);
            enemyShield.SetActive(true);
            enemyShielded = true;
            source.PlayOneShot(shieldSound);
        }
    }


    public IEnumerator ChangeLifeSelf(int life) {
        if (!playerShielded) {
            playerLife += life;
            if (playerLife > playerMaxLife) {
                playerLife = playerMaxLife;
            }
            if (playerLife <= 0) {
                playerLifeBar.SetActive(false);
                playerLifeText.text = 0 + "";
                finished = true;
                yield return new WaitForSeconds(1f);
                lose = true;
            } else {
                UpdateLifeBar(playerLife, playerMaxLife, playerLifeBar, playerLifeText);
            }
        }
    }

    public IEnumerator ChangeLifeEnemy(int life) {
        if (!enemyShielded) {
            enemyLife += life;
            if (enemyLife > enemyMaxLife) {
                enemyLife = enemyMaxLife;
            }
            if (enemyLife <= 0) {
                enemyLifeBar.SetActive(false);
                enemyLifeText.text = 0 + "";
                finished = true;
                yield return new WaitForSeconds(1f);
                win = true;
                EnemySpawning.canSpawn[fightIndex] = false;

                for (int i = 0; i < enemyStats.cardDrops.Count; i++) {
                    int rand = Random.Range(0, 100);
                    if (rand < enemyStats.cardChance[i]) {
                        Player.deck.deckCards.Add(enemyStats.cardDrops[i]);
                    }
                }
            } else {
                UpdateLifeBar(enemyLife, enemyMaxLife, enemyLifeBar, enemyLifeText);
            }
        }
    }

}
