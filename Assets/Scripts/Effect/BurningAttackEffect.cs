using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Unit with this effect will add @burningEffectPerAttack burning effects to the target 
// on each attack.
public class BurningAttackEffect : Effect
{
    public override EffectType type { get;} = EffectType.BurningAttackEffect;
    public override bool stackable {get; set;} = false;

    // The period this effect applied on the unit
    // If the period is float.PositiveInfinity, this effect will last forever
    public float burningAttackEffectPeriod = float.PositiveInfinity;
    // Add amount of burning stacks to the target on every attack
    public int burningEffectPerAttack = 1;
    // Burning effect used on each attack
    public BurningEffect burningEffect = new BurningEffect();

    public BurningAttackEffect(bool stackable, float burningAttackEffectPeriod, 
        int burningEffectPerAttack, BurningEffect burningEffect) {
            this.stackable = stackable;
            this.burningAttackEffectPeriod = burningAttackEffectPeriod;
            this.burningEffectPerAttack = burningEffectPerAttack;
            this.burningEffect = burningEffect;
    }

    public BurningAttackEffect() {}

    public override void apply(Unit unit) {
        bool applied = unit.effectData.hasEffect(type);
        unit.effectData.effects.Add(this);

        Delegate handler = new Delegate (OnBurningEffectHandler);
        unit.OnAttackEvent += delegate (GameObject attacker, GameObject target) {
            Unit targetUnit = target.GetComponent<Unit>();
        };
    }

    // Action that attach burning effect
    private void OnBurningEffectHandler(Unit attacker, Unit target) {

    }

    public override void remove(Unit unit) {
        unit.effectData.effects.Remove(this);
    }
}
