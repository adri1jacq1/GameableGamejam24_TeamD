using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DialogManager : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    public GameObject choiceButton1;
    public TextMeshProUGUI buttonText1;
    public TextMeshProUGUI buttonText2;
    public GameObject choiceButton2;
    public float typingSpeed;
    public bool isTyping = false;
    public bool hasChosen = false;

    private DialogText currentDialog;

    void Start()
    {

    }

    public void StartDialog(DialogText startingDialog)
    {
        currentDialog = startingDialog;
        hasChosen = false;
        choiceButton1.SetActive(false);
        choiceButton2.SetActive(false);
        StartCoroutine(Type());
    }

    IEnumerator Type()
    {
        dialogText.text = "";

        char[] letters = currentDialog.text.ToCharArray();
        foreach (char letter in letters)
        {
            isTyping = true;
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        ShowChoices();
        isTyping = false;
    }

    private void ShowChoices()
    {
        if (currentDialog.choices.Count > 0)
        {
            choiceButton1.SetActive(true);
            choiceButton2.SetActive(true);
            buttonText1.text = currentDialog.choices[0].choiceText;
            buttonText2.text = currentDialog.choices[1].choiceText;
        }
        else
        {
            choiceButton1.SetActive(false);
            choiceButton2.SetActive(false);
        }
    }

    public void OnChoiceMade(int choiceIndex)
    {
        hasChosen = true;
        choiceButton1.SetActive(false);
        choiceButton2.SetActive(false);
        DialogText nextDialog = null;
        if (currentDialog != null)
            nextDialog = currentDialog.choices[choiceIndex].nextDialog;
        if (nextDialog != null)
        {
            currentDialog = nextDialog;
            StartCoroutine(Type());
        }
    }
}
