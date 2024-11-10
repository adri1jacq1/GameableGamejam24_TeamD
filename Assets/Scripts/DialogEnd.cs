using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DialogEnd : MonoBehaviour
{
    public DialogManager dialogManager;
    public GameObject salad;

    void Start()
    {

        StartCoroutine(Dialog1());

    }

    IEnumerator Dialog1()
    {
        DialogText dialog1 = new DialogText("AVOCARDO you defeated all wasted food!");
        DialogText dialog2 = new DialogText("Indeed you are. We will never waste food again and use it to add ...");
        DialogText dialog3 = new DialogText("Of course. We will never waste food again and use it to add ...");

        dialog1.choices.Add(new Choice("I know, I'm the best.", dialog2));
        dialog1.choices.Add(new Choice("I'm always at the rescue of food hunger!", dialog3));

        dialogManager.StartDialog(dialog1);

        yield return new WaitUntil(() => dialogManager.hasChosen == true);
        yield return new WaitUntil(() => dialogManager.isTyping == false);

        yield return new WaitForSeconds(1f);
        StartCoroutine(Dialog2());
    }

    IEnumerator Dialog2()
    {
        DialogText dialog4 = new DialogText("A new SALAD to our menu named AVOCARDO in your honor!");
        dialogManager.StartDialog(dialog4);

        yield return new WaitUntil(() => dialogManager.isTyping == false);

        yield return new WaitForSeconds(0.5f);
        salad.SetActive(true);
    }

}