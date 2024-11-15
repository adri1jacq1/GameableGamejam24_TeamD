using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectInstantHeal : StatusEffectInstant {
    public StatusEffectInstantHeal(Player player, int count) : base(player, count) {
    }

    public override IEnumerator RunFunctions() {
        yield return GiveHealing();
    }
    public IEnumerator GiveHealing() {    //utiliser pour Heal
        applier.actions.RunHealEvent(this);

        yield return new WaitForEndOfFrame();
        if (amount + applier.life >= applier.maxLife) {
            amount = applier.maxLife - applier.life;
        }
        applier.life += amount;
        AudioFXManager.Instance.PlaySound("heal");
        applier.UpdateLifeBar();
    }
}
