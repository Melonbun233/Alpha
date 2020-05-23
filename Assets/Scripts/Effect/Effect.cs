using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Each effect is essentially buff/debuff
// Add the effect into the effectData for tracking purpose
public abstract class Effect
{
    // Whether multiple instances of this effect can be applied
    // the a unit at the same time
    public abstract bool stackable;
    // Add the the effect to the unit
    public void OnApplyEffect(Unit unit) {
        
    }
    // Remove the effect from the unit
    public void OnRemoveEffect(Unit unit) {

    }
}
