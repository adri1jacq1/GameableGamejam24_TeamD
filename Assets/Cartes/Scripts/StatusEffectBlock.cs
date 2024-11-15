using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatusEffectBlock : StatusEffect {


    public override void Init() {
        applier.actions.OnDamaged += Block;
        Debug.Log("Added a block effect");
    }

    public override void RemoveSubscriptions() {
        applier.actions.OnDamaged -= Block;
        Debug.Log("removed a block effect");
    }
    public StatusEffectBlock(Player player, int count) : base(player, count) {
        modifier = 10;
        effectName = "Block";
        Init();
    }


    public IEnumerator Block(StatusEffect effect) {
        effect.amount = 0;
        yield return RemoveStacks(1, false);
    }

}
