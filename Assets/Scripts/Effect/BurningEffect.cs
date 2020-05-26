using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BurningEffect : Effect
{
    public const float defaultPeriod = 5f;
    public const float defaultDamagePeriod = 1f;
    public const bool defaultStackable = true;

    public override EffectType type { get;} = EffectType.BurningEffect;
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

    // Period between each burning damage dealt the attached unit
    private float _damagePeriod;
    public float damagePeriod {
        get {
            return _damagePeriod;
        }
        set {
            _damagePeriod = value;
            damagePeriodCD = value;
        }
    }
    public float damagePeriodCD;


    public DamageData damageData = new DamageData(0, 2, 0, 0, 0);

    Action<Unit, float> onUpdateAction;
    
    

    public BurningEffect(float damagePeriod, DamageData damageData) {
        this.damagePeriod = damagePeriod;
        this.damageData = damageData;
        this.onUpdateAction = new Action<Unit, float> (OnUpdate);
        this.period = defaultPeriod;
    }

    public BurningEffect(){
        this.onUpdateAction = new Action<Unit, float> (OnUpdate);
        this.period = defaultPeriod;
        this.damagePeriod = defaultDamagePeriod;
    }

    public static BurningEffect deepCopy(BurningEffect effect) {
        return new BurningEffect(effect.damagePeriod, DamageData.deepCopy(effect.damageData));
    }

    public override void applyEffect(Unit unit) {
        // Burning effect is always stackable, we dont check it anyway
        unit.effectData.effects.Add(this);
        unit.effectData.burningStack ++;
        unit.OnUpdateEvent += onUpdateAction;
    }

    public override void removeEffect(Unit unit) {
        unit.effectData.effects.Remove(this);
        unit.OnUpdateEvent -= onUpdateAction;
        unit.effectData.burningStack --;
    }

    private void OnUpdate(Unit unit, float deltaTime) {
        // first check if this effect has expired
        periodCD -= deltaTime;
        
        if (periodCD <= 0) {
            removeEffect(unit);
            return;
        }

        // check if we can deal another burning damage
        damagePeriodCD -= deltaTime;
        if (damagePeriodCD <= 0) {
            unit.receiveDamage(damageData, null);
            damagePeriodCD = damagePeriod;
        }

    }
}
