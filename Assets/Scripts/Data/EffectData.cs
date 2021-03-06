﻿using System.Collections;
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

    public int crippleStack;
    public int crippleAttackStack;

    public int bleedingStack;
    public int bleedingAttackStack;

    public int damageReflectStack;

    public int knockBackAttackEffectStack;

    public int intangibleEffectStack;

    public int regenerationStack;

    public int stunEffectStack;
    public int stunAttackEffectStack;

    public int fragileEffectStack;
    public int fragileAttackEffectStack;

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


                case EffectType.CrippleAttackEffect:
                    list.Add(CrippleAttackEffect.deepCopy((CrippleAttackEffect)effect) as Effect);
                    break;
                case EffectType.CrippleEffect:
                    list.Add(CrippleEffect.deepCopy((CrippleEffect)effect) as Effect);
                    break;


                case EffectType.BleedingAttackEffect:
                    list.Add(BleedingAttackEffect.deepCopy((BleedingAttackEffect)effect) as Effect);
                    break;
                case EffectType.BleedingEffect:
                    list.Add(BleedingEffect.deepCopy((BleedingEffect)effect) as Effect);
                    break;

                case EffectType.DamageReflectEffect:
                    list.Add(DamageReflectEffect.deepCopy((DamageReflectEffect)effect) as Effect);
                    break;

                case EffectType.KnockBackAttackEffect:
                    list.Add(KnockBackAttackEffect.deepCopy((KnockBackAttackEffect)effect) as Effect);
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
