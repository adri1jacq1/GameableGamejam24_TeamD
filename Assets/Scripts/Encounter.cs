using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Encounter : MonoBehaviour {

    [SerializeField] private float waitTime;



    public int index;
    public EnemyStats stats;

    [SerializeField] private GameObject fightCanvas;
    [SerializeField] private GameObject character;
    [SerializeField] private GameObject background;
    public Sprite sprite;

    void Start() {
        this.GetComponent<SpriteRenderer>().sprite = sprite;    
    }

    void OnCollisionEnter2D(Collision2D other) {
        SceneController.win = false;
        SceneController.lastPosition = transform.position;
        fightCanvas.SetActive(true);
        character.GetComponent<Image>().sprite = sprite;
        background.GetComponent<Image>().sprite = stats.background;
        TurnSystem.enemyDeckCards = stats.cardDeck;
        TurnSystem.fightIndex = index;
        TurnSystem.enemySprite = sprite;
        TurnSystem.enemyStats = stats;
        StartCoroutine(Wait());
    }

    IEnumerator Wait() {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("Fight");
    }
}
