using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectData
{
    // Storing all effects on the unit
    public List<Effect> effects;

    // Effect Stacks
    public int burningStack;

    public bool hasEffect(EffectType type) {
        foreach(Effect effect in effects) {
            if (effect.type == type) {
                return true;
            }
        }

        return false;
    }
}
