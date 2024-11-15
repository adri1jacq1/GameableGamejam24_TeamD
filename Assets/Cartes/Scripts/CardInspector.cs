using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(Card))]

public class CardInspector : Editor {
    // Reference to the target script
    private Card cardScript;

    private StatusEffectType nextEffectType;
    private int nextEffectAmount;
    private ElligibleTarget nextTargets;

    private Dictionary<StatusEffectType, (ElligibleTarget targ, int amount)> effectsCopy = new();

    private void OnEnable() {
        cardScript = (Card)target;

        for (int i = 0; i < cardScript.effects.Count; i++) {
            effectsCopy.Add(cardScript.effects[i], (cardScript.targets[i], cardScript.effectsAmount[i]));
        }

    }

    public Type GetSelectedType(string typeName) {
        return Type.GetType(typeName); // Ensure typeName includes namespace if necessary
    }


    public override void OnInspectorGUI() {

        // Draw the default inspector
        DrawDefaultInspector();

        // Set up custom style for headers
        GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel);
        headerStyle.normal.textColor = Color.white; // Change header text color here

        Color originalColor = GUI.backgroundColor;

        GUI.backgroundColor = Color.cyan; // Change background color here
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.LabelField("Identifiers", headerStyle);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);


        cardScript.cardName = EditorGUILayout.TextField("Name", cardScript.cardName);
        cardScript.description = EditorGUILayout.TextField("Description", cardScript.description);
        //cardScript.type = EditorGUILayout.IntField("Type", cardScript.type);
        //cardScript.spellType = (SpellType)EditorGUILayout.EnumPopup("Spell Type", cardScript.spellType);

        GUI.backgroundColor = Color.blue; // Change background color here
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.LabelField("Costs", headerStyle);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        GUI.backgroundColor = new Color(1f, 0f, 0.3f); // Change background color here


        GUI.backgroundColor = Color.green; // Change background color here
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.LabelField("Stats", headerStyle);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        GUI.backgroundColor = Color.magenta; // Change background color here
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.LabelField("Visuals", headerStyle);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);


        cardScript.image = (Sprite)EditorGUILayout.ObjectField("Sprite", cardScript.image, typeof(Sprite), false);
        cardScript.backGround = (Sprite)EditorGUILayout.ObjectField("Background", cardScript.backGround, typeof(Sprite), false);



        nextEffectType = (StatusEffectType)EditorGUILayout.EnumPopup("Type Name", nextEffectType);
        nextEffectAmount = EditorGUILayout.IntField("Amount", nextEffectAmount);
        nextTargets = (ElligibleTarget)EditorGUILayout.EnumPopup("Targets", nextTargets);

        if (GUILayout.Button("Add Apply Effect")) {
            // Add a new attack to the list
            if (effectsCopy.ContainsKey(nextEffectType)) {
                Debug.Log("Already Present");
            } else {
                cardScript.effects.Add(nextEffectType);
                cardScript.targets.Add(nextTargets);
                cardScript.effectsAmount.Add(nextEffectAmount);
                effectsCopy.Add(nextEffectType, (nextTargets, nextEffectAmount));
            }
        }

        if (GUILayout.Button("Reset")) {
            effectsCopy = new();
            cardScript.effects = new();
            cardScript.targets = new();
            cardScript.effectsAmount = new();
        }



        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        // Display current list of abilities
        EditorGUILayout.LabelField("Abilities:");
        for (int i = 0; i < cardScript.effects.Count; i++) {


            EditorGUILayout.BeginVertical();

            EditorGUILayout.EnumPopup("Type", cardScript.effects[i]);

            cardScript.targets[i] = (ElligibleTarget)EditorGUILayout.EnumPopup("Ability Type", cardScript.targets[i]);
            cardScript.effectsAmount[i] = EditorGUILayout.IntField("Target", cardScript.effectsAmount[i]);





            // Remove button
            if (GUILayout.Button("Remove")) {
                effectsCopy.Remove(cardScript.effects[i]);
                cardScript.effects.Remove(cardScript.effects[i]);
                cardScript.targets.Remove(cardScript.targets[i]);
                cardScript.effectsAmount.Remove(cardScript.effectsAmount[i]);
                GUIUtility.ExitGUI(); // Exit GUI to avoid index out of range error
            }


            EditorGUILayout.EndHorizontal();
        }
            
        EditorUtility.SetDirty(cardScript); // Mark object as dirty

        // Apply changes to the serialized object
        serializedObject.ApplyModifiedProperties();

    }

}

