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

    public bool hasEffect(Effect effect) {
        foreach(Effect element in effects) {
            
        }
    }
}
