using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Each effect is essentially buff/debuff
// Add the effect into the effectData for tracking purpose
[System.Serializable]
public abstract class Effect
{
    // Whether multiple instances of this effect can be applied
    // on a unit at the same time
    public abstract bool stackable {get; set;}
    public abstract EffectType type {get;}

    // life time this effect applied on the unit
    // If the period is PositiveInfinity, it should last forever
    public abstract float period {get; set;}

    // Add the the effect to the unit
    // If this effect is non-stackable, there should be only
    // one instance of this effect applied on the unit
    public abstract void applyEffect(Unit unit);

    // Remove the effect from the unit
    public abstract void removeEffect(Unit unit); 

}


// All types of effect
public enum EffectType {
    BurningEffect,
    BurningAttackEffect
}