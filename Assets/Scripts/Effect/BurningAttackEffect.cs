using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Unit with this effect will add @burningEffectPerAttack burning effects to the target 
// on each attack.
[System.Serializable]
public class BurningAttackEffect : Effect
{
    public const float defaultPeriod = float.PositiveInfinity;
    public const bool defaultStackable = false;
    public const int defaultBurningEffectPerAttack = 1;


    public override EffectType type { get;} = EffectType.BurningAttackEffect;
    public override bool stackable {get; set;} = defaultStackable;

    private float _period;
    public override float period {
        get {
            return _period;
        }
        set {
            _period = value;
            periodCD = value;
        }
    }
    public float periodCD;


    // Add amount of burning stacks to the target on every attack
    public int burningEffectPerAttack = defaultBurningEffectPerAttack;
    // Burning effect used on each attack
    public BurningEffect burningEffect = new BurningEffect();

    Action<Unit, Unit> onAttackAction;
    Action<Unit, float> onUpdateAction;
    

    // Constructor for the burning attack effect
    // Note that we will directly use the reference of the provided burning effect
    public BurningAttackEffect(int burningEffectPerAttack, BurningEffect burningEffect) {
            this.burningEffectPerAttack = burningEffectPerAttack;
            this.burningEffect = burningEffect;
            this.onAttackAction = new Action<Unit, Unit> (OnAttack);
            this.onUpdateAction = new Action<Unit, float> (OnUpdate);
            this.period = defaultPeriod;
    }

    public BurningAttackEffect(){
        this.onAttackAction = new Action<Unit, Unit> (OnAttack);
        this.onUpdateAction = new Action<Unit, float> (OnUpdate);
        this.period = defaultPeriod;
    }

    public static BurningAttackEffect deepCopy (BurningAttackEffect effect) {
        BurningAttackEffect ret = new BurningAttackEffect(effect.burningEffectPerAttack, 
            BurningEffect.deepCopy(effect.burningEffect));
        ret.period = effect.period;
        ret.stackable = effect.stackable;
        return ret;
    }

    public override void applyEffect(Unit unit) {
        EffectData effectData = unit.effectData;
        bool hasEffect = effectData.hasEffect(type);

        // Need to check whether overwrite the current effect
        if (hasEffect && !stackable) {
            // overwrite the effect only when the new effect has longer period
            BurningAttackEffect oldEffect = effectData.getEffect(type) as BurningAttackEffect;
            if (oldEffect.periodCD < this.period) {
                oldEffect.removeEffect(unit);
            } else {
                return;
            }
        }

        // Add the new effect to the unit's effectdata
        unit.effectData.effects.Add(this);
        unit.OnUpdateEvent += onUpdateAction;
        unit.OnAttackEvent += onAttackAction;
        unit.effectData.burningAttackStack ++;
    }

    // Action attached to OnAttackEvent on a unit, and it applies burning effect
    // on unit's attack
    private void OnAttack(Unit attacker, Unit target) {
        for (int i = 0; i < burningEffectPerAttack; i ++) {
            BurningEffect burning = BurningEffect.deepCopy(burningEffect);
            burning.applyEffect(target);
        }
    }

    // Calculate the period of this effect. 
    private void OnUpdate(Unit unit, float deltaTime) {
        if (float.IsPositiveInfinity(period)) {
            return;
        }

        periodCD -= deltaTime;

        // remove all delegates related to this effect instance from the unit
        if (periodCD <= 0) {
            removeEffect(unit);
        }
    }

    // remove all delegates related to this effect instance from the unit
    public override void removeEffect(Unit unit) {
        unit.effectData.effects.Remove(this);
        unit.OnUpdateEvent -= onUpdateAction;
        unit.OnAttackEvent -= onAttackAction;
        unit.effectData.burningAttackStack --;
    }
}
