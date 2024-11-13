using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions {

    public Dictionary<string, PlayerStatusEffect> effectsList = new();
    public Dictionary<string, PlayerStatusEffect> effectsToRemoveList = new();
    public PlayerActions(Player p) {
        player = p;
    }

    private Player player;


    public void UpdateList() {
        foreach (PlayerStatusEffect effect in effectsToRemoveList.Values) {
            effectsList.Remove(effect.name);
        }
        effectsToRemoveList = new();
    }
    public void AddPlayerStatusEffectToRemove(PlayerStatusEffect effect) {
        if (effect.isStackable && effectsToRemoveList.ContainsKey(effect.name)) {
            effectsToRemoveList[effect.name].amount += effect.amount;
        } else {
            effectsToRemoveList[effect.name] = effect;
        }
    }

    public void AddPlayerStatusEffect(PlayerStatusEffect effect) {
        if (effect.isStackable && effectsList.ContainsKey(effect.name)) {
            effectsList[effect.name].amount += effect.amount;
        } else {
            effectsList[effect.name] = effect;
        }
    }

    public IEnumerator ChangeLife(int lifeChange) {
        if (lifeChange > 0) {
            if (lifeChange + player.life >= player.maxLife) {
                lifeChange = player.maxLife - player.life;
            }
            /*foreach (PlayerStatusEffect action in effectsList.Values) {
                lifeChange = action.GetTotalHealing(lifeChange);
            }*/

        } else if (lifeChange < 0) {
            foreach (PlayerStatusEffect action in effectsList.Values) {
                if (action is PlayerStatusEffectDamageRedux blockEffect) {
                    lifeChange = blockEffect.GetTotalDamage(lifeChange);
                }
            }
        }
        player.life += lifeChange;
        if (player.life <= 0) {
            player.lifeBar.SetActive(false);
            player.lifeText.text = 0 + "";
            TurnSystem.finished = true;
            yield return new WaitForSeconds(1f);
            if (player.enemyStats != null) {
                EnemySpawning.canSpawn[player.fightIndex] = false;
                CoroutineManager.Instance.RunCoroutine(TurnSystem.player.CardRewards(player.enemyStats));

            } else {
                TurnSystem.lose = true;
            }
        } else {
            player.UpdateLifeBar();
        }
        UpdateList();
    }


}
