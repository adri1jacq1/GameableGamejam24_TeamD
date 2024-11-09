using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DialogManager : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    public Button choiceButton1;
    public Button choiceButton2;
    public float typingSpeed;

    private DialogText currentDialog; 

    void Start()
    {
        choiceButton1.onClick.AddListener(() => OnChoiceMade(0));
        choiceButton2.onClick.AddListener(() => OnChoiceMade(1));
    }

    public void StartDialog(DialogText startingDialog)
    {
        currentDialog = startingDialog;
        StartCoroutine(Type());
    }

    IEnumerator Type()
    {
        dialogText.text = "";

        char[] letters = currentDialog.text.ToCharArray();
        foreach (char letter in letters)
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        ShowChoices();
    }

    private void ShowChoices()
    {
        if (currentDialog.choices.Count > 0)
        {
            choiceButton1.gameObject.SetActive(true);
            choiceButton2.gameObject.SetActive(true);
            choiceButton1.GetComponentInChildren<Text>().text = currentDialog.choices[0].choiceText;
            choiceButton2.GetComponentInChildren<Text>().text = currentDialog.choices[1].choiceText;
        }
        else
        {
            choiceButton1.gameObject.SetActive(false);
            choiceButton2.gameObject.SetActive(false);
        }
    }

    private void OnChoiceMade(int choiceIndex)
    {
        DialogText nextDialog = currentDialog.choices[choiceIndex].nextDialog;
        if (nextDialog != null)
        {
            currentDialog = nextDialog;
            StartCoroutine(Type());
        }
    }
}
