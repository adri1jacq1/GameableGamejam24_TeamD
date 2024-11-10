using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public int indexLevel;
    public float WaitTime;
    public GameObject canvas;
    
    public Dialog dialog;


    public static Vector3 lastPosition;
    public static string previousScene;

    void OnCollisionEnter2D(Collision2D other)
    {
        GetCurrentScene();
        lastPosition = transform.position;
        canvas.SetActive(true);
        StartCoroutine(Wait());
    }

    void Update()
    {
        if (dialog != null && dialog.dialogOver)
        {
            GetCurrentScene();
            lastPosition = transform.position;
            canvas.SetActive(true);
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(WaitTime);
        SceneManager.LoadScene(indexLevel);
    }

    public static void GetCurrentScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if (currentScene == 1)
            previousScene = "Narration";
        else if (currentScene == 2)
            previousScene = "Burger";
        else if (currentScene == 3)
            previousScene = "Poutine";
        else if (currentScene == 4)
            previousScene = "Pizza";

        previousScene = "Main";
    }

}