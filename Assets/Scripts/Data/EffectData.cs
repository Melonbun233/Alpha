using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectData
{
    // Storing all effects on the unit
    public List<Effect> effects = new List<Effect>();

    // Effect Stacks
    public int burningStack;
    public int burningAttackStack;

    public EffectData() {}

    public EffectData(List<Effect> effects) {
        this.effects = effects;
    }

    public static EffectData deepCopy(EffectData effectData) {
        List<Effect> list = new List<Effect>();
        foreach(Effect effect in effectData.effects) {
            switch (effect.type) {
                case EffectType.BurningAttackEffect:
                    list.Add(BurningAttackEffect.deepCopy((BurningAttackEffect)effect) as Effect);
                    break;
                
                case EffectType.BurningEffect:
                    list.Add(BurningEffect.deepCopy((BurningEffect)effect) as Effect);
                    break;
            }
        }
        
        return new EffectData(list);
    }

    // Apply all effects in this effect data to a unit
    // This should only be called right after a deepCopy
    // or spawn a unit 
    // This unit should use this effect data!
    public void applyAllEffects(Unit unit) {
        foreach (Effect effect in effects) {
            effect.applyEffect(unit);
        }
    }

    // Remove all effect from a unit
    // This unit should use this effect data!
    public void removeAllEffects(Unit unit) {
        foreach (Effect effect in effects) {
            effect.removeEffect(unit);
        }
    }

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
