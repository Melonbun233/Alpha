using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningEffect : Effect
{
    public override bool stackable {get; set;}
    public override EffectType type {get {return EffectType.BurningEffect;}}

    public override void apply(Unit unit) {

    }

    public override void remove(Unit unit) {

    }
}
