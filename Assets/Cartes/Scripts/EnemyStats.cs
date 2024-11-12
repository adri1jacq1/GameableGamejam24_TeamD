using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "EnemyStats", menuName = "new Enemy")]
public class EnemyStats : ScriptableObject
{
    public int life;
    public string enemyName;
    public string description;

    public List<Card> cardDrops;
    public List<float> cardChance;

    public List<Card> cardDeck;


    public Sprite image;


    public Color32 color;
}
