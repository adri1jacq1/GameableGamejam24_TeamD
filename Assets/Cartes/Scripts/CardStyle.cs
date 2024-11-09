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
    
    public void SetStats(int attack, int healing, string name, string description, bool canDefend, Sprite image) {
        _attackText.text = attack + "";
        _healingText.text = healing + "";
        _nameText.text = name + "";

        _descriptionText.text = "";
        if (canDefend) {
            _descriptionText.text += "Give shield \n\n";
        }
        _descriptionText.text += description;

        _image.GetComponent<SpriteRenderer>().sprite = image;
    }
}
