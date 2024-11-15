using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        CardInfo cardInfo = cardObj.GetComponent<CardInfo>();
        Dictionary<StatusEffectType, (ElligibleTarget, int)> dict = new();
        for (int i = 0; i < cardStats.effects.Count; i++) {
            dict.Add(cardStats.effects[i], (cardStats.targets[i], cardStats.effectsAmount[i]));
            Debug.Log("Added a card effect: " + (cardStats.effects[i] + " " + cardStats.targets[i] + " " + cardStats.effectsAmount[i]));
        }
        cardInfo.SetStats(dict, cardStats.name, cardStats.description, cardStats.image, cardStats.backGround);
        cardInfo.cardStats = cardStats;
        cardObj.GetComponent<Drag>().system = systemStatic;
        //CreateEffects(cardInfo);
        return cardObj;
    }
    /*
    static private void CreateEffects(CardInfo info) {
        Card stats = info.cardStats;
        foreach (Type effect in stats.effects.Keys) {
            if (effect.IsSubclassOf(typeof(StatusEffect))) {
                StatusEffect apply = (StatusEffect)Activator.CreateInstance(effect, stats, (stats.effects[effect].toSelf, stats.effects[effect].amount));
            }
            throw new ArgumentException("No creator found for the specified animal type.");
        }
    }*/




}
