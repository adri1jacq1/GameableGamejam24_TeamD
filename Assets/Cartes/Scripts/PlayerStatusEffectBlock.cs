using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusEffectBlock : PlayerStatusEffectDamageRedux
{
    public PlayerStatusEffectBlock(Player p, int count) : base(p) {
        player = p;
        amount = count;
        isStackable = true;
        name = "Block";
        player.shield.SetActive(true);
    }

    public override int GetTotalDamage(int initDamage) {
        amount--;
        if (amount <= 0) {
            player.actions.AddPlayerStatusEffectToRemove(this);
            player.shield.SetActive(false);
            
        }
        return 0;
    }


}
