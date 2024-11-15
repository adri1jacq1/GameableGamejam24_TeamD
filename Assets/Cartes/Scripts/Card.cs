using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum ElligibleTarget {
    Self,
    Enemy,
    Both
}
public enum StatusEffectType {
    StatusEffectInstantDamage,
    StatusEffectInstantHeal,
    StatusEffectBlock
}

public class StatusEffectFactory {
    private static readonly Dictionary<StatusEffectType, Type> statusEffectTypeMappings = new() {
        { StatusEffectType.StatusEffectInstantDamage, typeof(StatusEffectInstantDamage) },
        { StatusEffectType.StatusEffectInstantHeal, typeof(StatusEffectInstantHeal) },
        { StatusEffectType.StatusEffectBlock, typeof(StatusEffectBlock) }
    };


    private static readonly Dictionary<StatusEffectType, float> statusEffectModifierMappings = new() {
        { StatusEffectType.StatusEffectInstantDamage , -0.05f },
        { StatusEffectType.StatusEffectInstantHeal, 0.01f},
        { StatusEffectType.StatusEffectBlock, 1f }
    };

    public static Type GetStatusEffectType(StatusEffectType statusEffectType) {
        return statusEffectTypeMappings.TryGetValue(statusEffectType, out var type) ? type : null;
    }

    public static float GetStatusEffectModifier(StatusEffectType statusEffectType) {
        return statusEffectModifierMappings.TryGetValue(statusEffectType, out var mod) ? mod : 0f;
    }
}

[CreateAssetMenu(fileName = "Card", menuName = "new Card")]
public class Card : ScriptableObject
{

    [HideInInspector] public Type type;
    [HideInInspector] public string cardName;
    [HideInInspector] public string description;

    [SerializeField]
    [HideInInspector] public List<StatusEffectType> effects = new();
    [HideInInspector] public List<ElligibleTarget> targets = new();
    [HideInInspector] public List<int> effectsAmount = new();


    [HideInInspector] public Sprite image;

    [HideInInspector] public Sprite backGround;


    [HideInInspector] public Color32 color;

}

