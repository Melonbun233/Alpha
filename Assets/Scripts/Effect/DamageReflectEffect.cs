using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics.Tracing;

public class DamageReflectEffect : Effect
{
    public const float defaultPeriod = float.PositiveInfinity;
    public const bool defaultStackable = false;
    public override EffectType type { get; } = EffectType.DamageReflectEffect;
    public override bool stackable { get; set; } = defaultStackable;

    private float _period;
    public override float period
    {
        get
        {
            return _period;
        }
        set
        {
            _period = value;
            periodCD = value;
        }
    }
    public float periodCD;

    public DamageData damageData = new DamageData(2, 0, 0, 0, 0);

    Action<Unit, float> onUpdateAction;

    public DamageReflectEffect(float Period, DamageData damageData)
    {
        this.damageData = damageData;
        this.onUpdateAction = new Action<Unit, float>(OnUpdate);
        this.period = Period;
    }

    public DamageReflectEffect()
    {
        this.onUpdateAction = new Action<Unit, float>(OnUpdate);
        this.period = defaultPeriod;
    }

    public static DamageReflectEffect deepCopy(DamageReflectEffect effect)
    {
        return new DamageReflectEffect(effect.period, DamageData.deepCopy(effect.damageData));
    }

    private Unit EffectedUnit;
    public override void applyEffect(Unit unit)
    {
        if (unit.effectData.hasEffect(EffectType.DamageReflectEffect))
        {
            unit.effectData.getEffect(type).damage = this.damageData;
        }
        else
        {
            unit.effectData.effects.Add(this);
            EffectedUnit = unit;
            unit.effectData.damageReflectStack++;
            unit.OnUpdateEvent += onUpdateAction;
            unit.OnReceiveDamageEvent += reflectDmg;
        }
        
    }

    public override void removeEffect(Unit unit)
    {
        unit.effectData.effects.Remove(this);
        unit.OnUpdateEvent -= onUpdateAction;
        unit.effectData.damageReflectStack--;
        unit.OnReceiveDamageEvent -= reflectDmg;
    }

    public void reflectDmg(Unit from, Unit _this, int dmg)
    {
        from.receiveDamage(damageData, EffectedUnit.gameObject);
    }

    private void OnUpdate(Unit unit, float deltaTime)
    {
        // first check if this effect has expired
        periodCD -= deltaTime;

        if (periodCD <= 0)
        {
            removeEffect(unit);
            return;
        }
    }
}
