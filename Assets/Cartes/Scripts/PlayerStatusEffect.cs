using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusEffect
{
    public Player player;
    public int amount;
    public bool isStackable;
    public string name;

    /*~PlayerStatusEffect() {
    }*/
    public PlayerStatusEffect(Player p) {
        player = p;
    }
    public virtual int GetTotalDamage(int initDamage) {
        return initDamage;
    }

    public virtual int GetTotalHealing(int initHealing) {
        return initHealing;
    }

}
