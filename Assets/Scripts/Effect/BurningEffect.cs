using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Unit with this effect will add @burningStacksCount burning stacks to the attack receiver
// on each attack.
// With each burning stack applied to another unit, it will receive @burningDamage
// every @burningCoolDown seconds that happens @burningTimesCount times
public class BurningEffect : Effect
{
    // Add amount of burning stack to the target on every attack
    public int burningStacksCount = 1;
    // Amount of damage times a single burning stack can deal
    public int burningTimesCount;
    // Cooldown between each burning damage
    public float burningCoolDown;
    // Damage dealt for each burning
    public DamageData burningDamage;

    public BurningEffect(int burningTimesCount, int burningCoolDown, DamageData burningDamage) {
        this.burningTimesCount = burningTimesCount;
        this.burningCoolDown = burningCoolDown;
        this.burningDamage = burningDamage;
    }
    public override void OnApplyEffect(Unit unit) {
        unit.effectData.effects.Add(this);

        unit.OnAttackEvent += delegate (GameObject attacker, GameObject target) {
            Unit targetUnit = target.GetComponent<Unit>();
        };
    }

    public override void OnRemoveEffect(Unit unit) {
        unit.effectData.effects.Remove(this);
    }
}
