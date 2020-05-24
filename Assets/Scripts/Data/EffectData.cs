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

    // get the first effect with specified type
    public Effect getEffect(EffectType type) {
        foreach(Effect effect in effects) {
            if (effect.type == type) {
                return effect;
            }
        }

        return null;
    }

    // get all the effects in the Effect data with specified type 
    public List<Effect> getEffects(EffectType type) {
        List<Effect> allEffects = new List<Effect>();
        foreach(Effect effect in effects) {
            if (effect.type == type) {
                allEffects.Add(effect);
            }
        }

        return allEffects;
    }

}
