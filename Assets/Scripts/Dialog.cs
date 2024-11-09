using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[Serializable]
public class DialogText
{
    public string text; 
    public List<Choice> choices;

    public DialogText(string text)
    {
        this.text = text;
        this.choices = new List<Choice>();
    }
}

[Serializable]
public class Choice
{
    public string choiceText;
    public DialogText nextDialog; 

    public Choice(string choiceText, DialogText nextDialog)
    {
        this.choiceText = choiceText;
        this.nextDialog = nextDialog;
    }
}

public class dialog : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;

    public DialogManager dialogManager;

    void Start()
    {
        DialogText dialog1 = new DialogText("I am exasperated");
        DialogText dialog2 = new DialogText("I am Anna Nass, the principal of this school.");
        DialogText dialog3 = new DialogText("Ah...");

        dialog1.choices.Add(new Choice("Who are you?", dialog2));
        dialog1.choices.Add(new Choice("What happened to you?", dialog3));

        dialogManager.StartDialog(dialog1);

        DialogText dialog4 = new DialogText("Food wasted by students have taken control of the school, and won't let anyone in...");
        dialogManager.StartDialog(dialog4);

        DialogText dialog5 = new DialogText("Not being able to attend classes... Student's worst nightmare...");
        DialogText dialog6 = new DialogText("Haha, I love the sense of humour of avocados, let's get to work now!");
        dialogManager.StartDialog(dialog5);

        dialog5.choices.Add(new Choice("Let me help you with my Avocado Super Powers.", ));

        

    }



}