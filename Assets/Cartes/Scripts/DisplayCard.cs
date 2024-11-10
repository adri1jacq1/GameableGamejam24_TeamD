using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCard : MonoBehaviour
{

    [SerializeField] private TurnSystem system;
    private static TurnSystem systemStatic;

    [SerializeField] private GameObject cardTemplateObj;
    static GameObject cardTemplate;


    public void Start() {
        cardTemplate = cardTemplateObj;
        systemStatic = system;
    }


    static public GameObject CreateCard(Card cardStats) {
        GameObject cardObj = Instantiate(cardTemplate);
        CardStyle cardStyle = cardObj.GetComponent<CardStyle>();
        cardStyle.SetStats(cardStats.attack, cardStats.healing, cardStats.name, cardStats.description, cardStats.givesDefense, cardStats.image, cardStats.backGround);
        cardStyle.cardStats = cardStats;
        cardObj.GetComponent<Drag>().system = systemStatic;
        return cardObj;
    }






}
