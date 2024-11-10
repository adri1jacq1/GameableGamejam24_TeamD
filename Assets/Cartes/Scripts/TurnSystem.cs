using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour {
    public static bool isPlayerTurn = false;

    public Deck playerDeck;
    public Deck enemyDeck;
    public GameObject player;
    public GameObject enemy;

    public float modifier;

    public bool playerShielded = false;
    public bool enemyShielded = false;

    public AudioClip punchSound;
    public AudioClip healSound;
    public AudioClip shieldSound;

    public AudioSource source;


    public GameObject playerShield;
    public GameObject enemyShield;

    public int playerLife = 10;
    public int enemyLife = 10;

    public GameObject cardContainer;

    public bool lose = false;
    public bool win = false;


    public void Start() {

        int i = 0;

        foreach (Card card in enemyDeck.deckCards) {
            enemyDeck.deck.Add(card);
        }

        foreach (Card card in playerDeck.deckCards) {
            playerDeck.deck.Add(card);
        }


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
        if (enemyShielded) {
            enemyShield.SetActive(false);
            enemyShielded = false;
        }

        yield return new WaitForSeconds(0.1f);

        enemyDeck.DrawCard();

        yield return new WaitForSeconds(1f);

        int rand = Random.Range(0, enemyDeck.hand.hand.Count);
        GameObject card = enemyDeck.hand.hand[rand];
        Card stats = card.GetComponent<CardStyle>().cardStats;

        enemyDeck.hand.RemoveCard(card);
        card.GetComponent<CardStyle>().toggleFront(true);

        card.transform.position += new Vector3(0f, 0f, -2f);

        for (int i = 0; i < 10; i++) {
            yield return new WaitForSeconds(0.05f);
            card.transform.position += new Vector3(0f, -0.1f, 0f);
        }

        yield return new WaitForSeconds(1f);
        cardContainer = card;
        if (stats.givesDefense || stats.healing > stats.attack) {
            enemyDeck.hand.RemoveCard(cardContainer);
            Vector3 initPos = card.transform.position;
            while (Vector3.Distance(card.transform.position,enemy.transform.position) > 1f) {
                yield return new WaitForSeconds(0.05f);
                card.transform.position += (enemy.transform.position - initPos).normalized;
            }
            StartCoroutine(HealEnemy(stats.healing));
            StartCoroutine(ShieldEnemy(stats.givesDefense));

        } else {
            enemyDeck.hand.RemoveCard(cardContainer);
            Vector3 initPos = card.transform.position;
            while (Vector3.Distance(card.transform.position, player.transform.position) > 1f) {
                yield return new WaitForSeconds(0.05f);
                card.transform.position += (player.transform.position - initPos).normalized;
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

    public IEnumerator UseCard(float xPos, GameObject cardObj) {
        Card stats = cardObj.GetComponent<CardStyle>().cardStats;
        cardContainer = cardObj;
        yield return new WaitForEndOfFrame();
        if (xPos > 960) {
            Vector3 initPos = cardObj.transform.position;
            while (Vector3.Distance(cardObj.transform.position, enemy.transform.position) > 1f) {
                yield return new WaitForSeconds(0.05f);
                cardObj.transform.position += (enemy.transform.position - initPos).normalized;
            }
            StartCoroutine(DamageEnemy(stats.attack));

        } else {
            Vector3 initPos = cardObj.transform.position;
            while (Vector3.Distance(cardObj.transform.position, player.transform.position) > 1f) {
                yield return new WaitForSeconds(0.05f);
                cardObj.transform.position += (player.transform.position - initPos).normalized;
            }
            StartCoroutine(HealSelf(stats.healing));
            StartCoroutine(ShieldSelf(stats.givesDefense));

        }
        StartCoroutine(PlayBot());
        playerDeck.PlaceBackCard(cardContainer);
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
            player.transform.localScale = new Vector2(playerLife * modifier, player.transform.localScale.y);
            yield return new WaitForSeconds(2f);
            if (playerLife <= 0) {
                lose = true;
            }
        }
    }

    public IEnumerator ChangeLifeEnemy(int life) {
        if (!enemyShielded) {
            enemyLife += life;
            enemy.transform.localScale = new Vector2(enemyLife * modifier, enemy.transform.localScale.y);
            yield return new WaitForSeconds(2f);
            if (enemyLife <= 0) {
                win = true;
            }
        }
    }

}
