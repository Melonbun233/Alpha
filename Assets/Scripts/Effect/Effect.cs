using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Each effect is essentially buff/debuff
// Add the effect into the effectData for tracking purpose
public abstract class Effect
{
    // Add the the effect to the unit
    public abstract void OnApplyEffect(Unit unit);
    // Remove the effect from the unit
    public abstract void OnRemoveEffect(Unit unit);
}
