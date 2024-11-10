using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardStyle : MonoBehaviour
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

    public void SetStats(int attack, int healing, string name, string description, bool canDefend, Sprite image, Sprite background) {
        _attackText.text = attack + "";
        _healingText.text = healing + "";
        _nameText.text = name + "";

        _background.GetComponent<SpriteRenderer>().sprite = background;

        _descriptionText.text = "";
        if (canDefend) {
            _descriptionText.text += "Give shield \n\n";
        }
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
