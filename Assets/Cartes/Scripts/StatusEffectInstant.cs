using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectInstant : StatusEffect {

    public StatusEffectInstant(Player player, int count) : base(player, count) {
    }

    public override void Init() {
        CoroutineManager.Instance.RunCoroutine(RunInstantEffects());
    }

    public IEnumerator RunInstantEffects() {
        yield return RunFunctions();
        Remove();
    }

    public virtual IEnumerator RunFunctions() {
        yield return new WaitForEndOfFrame();
    }

}
