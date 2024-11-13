using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

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

public class Dialog : MonoBehaviour
{
    public DialogManager dialogManager;
    public bool dialogOver = false;
    public GameObject canvas;


    void Start()
    {

        StartCoroutine(Dialog1());

    }

    IEnumerator Dialog1() {
        //yield return new WaitForSeconds(1.75f);
        canvas.SetActive(false);

        DialogText dialog1 = new DialogText("I am exasperated.");
        DialogText dialog2 = new DialogText("I am Anna Nass, the principal of this school.");
        DialogText dialog3 = new DialogText("Ah...");

        dialog1.choices.Add(new Choice("Who are you?", dialog2));
        dialog1.choices.Add(new Choice("What happened to you?", dialog3));

        dialogManager.StartDialog(dialog1);

        yield return new WaitUntil(() => dialogManager.hasChosen == true);
        yield return new WaitUntil(() => dialogManager.isTyping == false);

        //yield return new WaitForSeconds(1f);
        //StartCoroutine(Dialog2());

        dialogOver = true;
    }

    IEnumerator Dialog2()
    {
        DialogText dialog4 = new DialogText("Food wasted by students have taken control of the school, and won't let anyone in...");
        dialogManager.StartDialog(dialog4);

        yield return new WaitUntil(() => dialogManager.isTyping == false);

        //yield return new WaitForSeconds(1f);
        StartCoroutine(Dialog3());
    }

    IEnumerator Dialog3()
    {
        DialogText dialog5 = new DialogText("Not being able to attend classes... Students' worst nightmare...");
        DialogText dialog6 = new DialogText("Haha, I love the sense of humour of avocados, let's get to work now!");

        dialog5.choices.Add(new Choice("Let me help you with my Avocado Super Powers.", dialog6));
        dialog5.choices.Add(new Choice("It's not my problem.", dialog6));

        dialogManager.StartDialog(dialog5);

        yield return new WaitUntil(() => dialogManager.hasChosen == true);
        yield return new WaitUntil(() => dialogManager.isTyping == false);

        dialogOver = true;

    }



}