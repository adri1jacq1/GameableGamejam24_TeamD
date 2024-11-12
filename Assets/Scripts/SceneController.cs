using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public int indexLevel;
    public float WaitTime;
    public GameObject canvas;

    public GameObject canvasLose;

    public Card guacamole;
    public Card tomato;

    public Dialog dialog;

    public TurnSystem turnSystem;

    public static bool win = false;


    public static Vector3 lastPosition;
    public static string previousScene;

    public GameObject enemyWin;
    public GameObject backGroundWin;
    public GameObject enemyLose;
    public GameObject backGroundLose;



    void Update()
    {
        if (dialog != null && dialog.dialogOver)
        {
            GetCurrentScene();
            canvas.SetActive(true);
            StartCoroutine(Wait());
        }
        else if (turnSystem != null && turnSystem.win)
        {
            enemyWin.GetComponent<Image>().sprite = turnSystem.enemy.GetComponent<SpriteRenderer>().sprite;
            backGroundWin.GetComponent<Image>().sprite = turnSystem.backGound.GetComponent<SpriteRenderer>().sprite;
            win = true;
            GetCurrentScene();
            StartCoroutine(WaitBeforeDisplay());
            canvas.SetActive(true);
            StartCoroutine(Wait());
        }
        else if (turnSystem != null && turnSystem.lose)
        {
            enemyLose.GetComponent<Image>().sprite = turnSystem.enemy.GetComponent<SpriteRenderer>().sprite;
            backGroundLose.GetComponent<Image>().sprite = turnSystem.backGound.GetComponent<SpriteRenderer>().sprite;
            win = false;
            StartCoroutine(WaitBeforeDisplay());
            lastPosition = Vector3.zero;
            canvasLose.SetActive(true);
            StartCoroutine(Wait());
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        lastPosition = transform.position;
        canvas.SetActive(true);
        Player.deck = new Deck();
        for (int i = 0; i < 5; i++) {
            Player.deck.deckCards.Add(guacamole);
        }
        for (int i = 0; i < 5; i++) {
            Player.deck.deckCards.Add(tomato);
        }
        StartCoroutine(Wait());
    }

    IEnumerator WaitBeforeDisplay()
    {
        yield return new WaitForSeconds(0.1f);
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(WaitTime);
        SceneManager.LoadScene(indexLevel);
    }

    public static void GetCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "Narration")
            previousScene = "Narration";
        else if (currentScene == "BurgerFight")
            previousScene = "Burger";
        else if (currentScene == "PoutineFight")
            previousScene = "Poutine";
        else if (currentScene == "PizzaFight")
            previousScene = "Pizza";

        Debug.Log(previousScene);
    }

}