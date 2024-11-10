using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public int indexLevel;
    public float WaitTime;
    public GameObject canvas;

    public GameObject canvasLose;
    
    public Dialog dialog;

    public TurnSystem turnSystem;

    public static bool win = false;


    public static Vector3 lastPosition;
    public static string previousScene;

    void OnCollisionEnter2D(Collision2D other)
    {
        win = false;
        lastPosition = transform.position;
        canvas.SetActive(true);
        StartCoroutine(Wait());
    }

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
            win = true;
            GetCurrentScene();
            StartCoroutine(WaitBeforeDisplay());
            canvas.SetActive(true);
            StartCoroutine(Wait());
        }
        else if (turnSystem != null && turnSystem.lose)
        {
            win = false;
            StartCoroutine(WaitBeforeDisplay());
            lastPosition = Vector3.zero;
            canvasLose.SetActive(true);
            StartCoroutine(Wait());
        }
    }

    IEnumerator WaitBeforeDisplay()
    {
        yield return new WaitForSeconds(3);
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