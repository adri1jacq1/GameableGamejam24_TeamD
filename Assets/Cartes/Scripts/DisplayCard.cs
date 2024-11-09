using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCard : MonoBehaviour
{

    [SerializeField] private List<Card> cards = new();
    [SerializeField] private GameObject cardTemplateObj;
    static GameObject cardTemplate;


    public void Start() {
        cardTemplate = cardTemplateObj;  
    }


    static public GameObject CreateCard(Card cardStats) {
        GameObject cardObj = Instantiate(cardTemplate);
        CardStyle cardStyle = cardObj.GetComponent<CardStyle>();
        cardStyle.SetStats(cardStats.attack, cardStats.healing, cardStats.name, cardStats.description, cardStats.givesDefense, cardStats.image);
        return cardObj;
    }






}
