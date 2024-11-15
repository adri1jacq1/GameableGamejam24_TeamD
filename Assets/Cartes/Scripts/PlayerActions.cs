using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public delegate IEnumerator EffectAppliedEventHandler(StatusEffect effect);
public class PlayerActions {


    public Dictionary<Type, StatusEffect> allEffects = new();

    public void AddStatusEffect(StatusEffect effect) {
        if (effect.isStackable && allEffects.ContainsKey(effect.GetType())) {
            allEffects[effect.GetType()].amount += effect.amount;
        } else if (!allEffects.ContainsKey(effect.GetType())) {
            allEffects.Add(effect.GetType(), effect);
        }
    }


    public event EffectAppliedEventHandler OnApplied;
    public event EffectAppliedEventHandler OnDamaged;
    public event EffectAppliedEventHandler OnHealed;


    public bool RunApplyEvent(StatusEffect effect) {
        if (OnApplied != null) {
            foreach (EffectAppliedEventHandler handler in OnApplied.GetInvocationList()) {
                CoroutineManager.Instance.RunCoroutine(handler(effect));
            }
        }
        return true;
    }

    public bool RunDamageEvent(StatusEffect effect) {
        if (OnDamaged != null) {
            foreach (EffectAppliedEventHandler handler in OnDamaged.GetInvocationList()) {
                CoroutineManager.Instance.RunCoroutine(handler(effect));
            }
        }
        return true;
    }

    public bool RunHealEvent(StatusEffect effect) {
        if (OnHealed != null) {
            foreach (EffectAppliedEventHandler handler in OnHealed.GetInvocationList()) {
                CoroutineManager.Instance.RunCoroutine(handler(effect));
            }
        }
        return true;
    }

    public PlayerActions(Player p) {
        player = p;
    }

    private Player player;



}
