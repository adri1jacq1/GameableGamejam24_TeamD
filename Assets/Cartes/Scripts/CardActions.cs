using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public delegate IEnumerator EffectApplyEventHandler(Player applier, Player target);
public class CardActions {


    public Dictionary<StatusEffectType, (ElligibleTarget targets, int amount)> allEffects = new();


    public CardActions(Dictionary<StatusEffectType, (ElligibleTarget targets, int amount)> effects) {
        allEffects = effects;
        Init();
    }

    public void Init() {
        CoroutineManager.Instance.RunCoroutine(SubscribeToApplyEffects());
    }

    public virtual IEnumerator SubscribeToApplyEffects() {
        yield return new WaitForEndOfFrame();
        OnPlayed += ApplyEffects;
    }

    public bool CanApply(Player p1, Player p2, ElligibleTarget targets) {
        return ((p1.playerName == p2.playerName && targets == ElligibleTarget.Self) || (p1.playerName != p2.playerName && targets == ElligibleTarget.Enemy) || targets == ElligibleTarget.Both);
    }

    public IEnumerator ApplyEffects(Player player, Player target) {
        foreach (StatusEffectType effectType in allEffects.Keys) {
            if (CanApply(player, target, allEffects[effectType].targets)) {
                StatusEffect effect = (StatusEffect)Activator.CreateInstance(StatusEffectFactory.GetStatusEffectType(effectType), target, allEffects[effectType].amount);
                target.actions.AddStatusEffect(effect);
                Debug.Log("Add " + effectType + " Effect");
            }
        }
        yield return new WaitForEndOfFrame();
    }


    public Type GetSelectedType(string typeName) {
        return Type.GetType(typeName); // Ensure typeName includes namespace if necessary
    }



    public event EffectApplyEventHandler OnPlayed;


    public bool RunPlayEvent(Player actor, Player targ) {
        if (OnPlayed != null) {
            foreach (EffectApplyEventHandler handler in OnPlayed.GetInvocationList()) {
                CoroutineManager.Instance.RunCoroutine(handler(actor, targ));
            }
        }
        return true;
    }

    public CardActions(CardInfo c) {
        card = c;
    }

    private CardInfo card;



}

