using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CardInfo : MonoBehaviour
{
    [SerializeField] private TextMeshPro _attackText;
    [SerializeField] private TextMeshPro _healingText;
    [SerializeField] private TextMeshPro _nameText;
    [SerializeField] private TextMeshPro _descriptionText;
    [SerializeField] private GameObject _image;
    [SerializeField] private GameObject _cardBack;
    [SerializeField] private GameObject _cardFront;
    [SerializeField] private GameObject _background;

    public Card cardStats;

    public CardActions actions;

    public void Start() {
    }

    public void SetStats(Dictionary<StatusEffectType,(ElligibleTarget target, int amount)> effects , string name, string description, Sprite image, Sprite background) {

        actions = new(effects); 
        
        if (effects.ContainsKey(StatusEffectType.StatusEffectInstantDamage)) {
            _attackText.text = effects[StatusEffectType.StatusEffectInstantDamage].amount + "";
        } else {
            _attackText.text = "0";
        }
        if (effects.ContainsKey(StatusEffectType.StatusEffectInstantHeal)) {
            _healingText.text = effects[StatusEffectType.StatusEffectInstantHeal].amount + "";
        } else {
            _healingText.text = "0";
        }
        _nameText.text = name + "";

        _background.GetComponent<SpriteRenderer>().sprite = background;

        _descriptionText.text = "";

        _descriptionText.text += description;

        _image.GetComponent<SpriteRenderer>().sprite = image;
    }

    public void toggleFront(bool front) {
        if (front) {
            _cardBack.GetComponent<SpriteRenderer>().enabled = false;
            _cardFront.SetActive(true);
        } else {
            _cardBack.GetComponent<SpriteRenderer>().enabled = true;
            _cardFront.SetActive(false);
        }
    }


}
