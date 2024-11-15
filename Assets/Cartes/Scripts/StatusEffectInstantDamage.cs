using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectInstantDamage : StatusEffectInstant {


    public StatusEffectInstantDamage(Player player, int count) : base(player, count) {
    }


    public override IEnumerator RunFunctions() {
        yield return DealDamage();
    }
    public IEnumerator DealDamage() {  //À utiliser pour Damage
        applier.actions.RunDamageEvent(this);
        applier.life -= amount;
        if (applier.life <= 0) {
            applier.lifeBar.SetActive(false);
            applier.lifeText.text = 0 + "";
            TurnSystem.finished = true;
            yield return new WaitForSeconds(1f);
            if (applier.enemyStats != null) {
                EnemySpawning.canSpawn[applier.fightIndex] = false;
                CoroutineManager.Instance.RunCoroutine(TurnSystem.player.CardRewards(applier.enemyStats));

            } else {
                TurnSystem.lose = true;
            }
        } else {
            applier.UpdateLifeBar();
        }
        AudioFXManager.Instance.PlaySound("damage");
    }
}
