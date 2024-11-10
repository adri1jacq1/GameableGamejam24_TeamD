using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Card", menuName = "new Card")]

public class Card : ScriptableObject
{
    public int attack;
    public int healing;
    public string cardName;
    public string description;
    public bool givesDefense;


    public Sprite image;

    public Sprite backGround;


    public Color32 color;
}
