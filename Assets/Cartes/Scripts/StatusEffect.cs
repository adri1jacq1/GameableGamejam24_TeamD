using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect {

    public static Player applier;
    public int amount;
    public int temporary;
    public bool isStackable;
    public string effectName;

    public bool hasEndRoutine;
    public float modifier = 0;


    public virtual void Init() {
    }

    public virtual IEnumerator RemoveStacks(int count, bool removeTemporary) {
        amount -= count;
        if (removeTemporary) {
            temporary -= amount;
        }
        Debug.Log("Removed a stack");

        if (amount <= 0) {
            yield return Remove();
        }
    }

    public IEnumerator Remove() {
        Debug.Log("Removed totally");

        RemoveSubscriptions();
        applier.actions.allEffects.Remove(GetType());
        if (hasEndRoutine) {
            yield return EndRoutine();
        }
    }

    ~StatusEffect() {

    }

    public virtual void RemoveSubscriptions() {
    }


    public IEnumerator EndRoutine() {   //Les effets cool de removing
        yield return new WaitForEndOfFrame();
    }


    /*~StatusEffect() {
    }*/
    public StatusEffect(Player player, int count) {
        applier = player;
        amount = count;
        Init();
    }
}
