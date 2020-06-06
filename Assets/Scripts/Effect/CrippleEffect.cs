using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CrippleEffect : Effect
{
    public const float defaultPeriod = 5f;
    public const float defaultDamageTick = 1f;
    public const bool defaultStackable = false;
    public override EffectType type { get; } = EffectType.CrippleEffect;
    public override bool stackable { get; set; } = defaultStackable;

    private float _period;
    private Vector3 LastPosition;
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


    public DamageData damageData = new DamageData(1, 0, 0, 0, 0);

    public override DamageData damage
    {
        get
        {
            return damageData;
        }
        set
        {
            damageData = value;
        }
    }


    Action<Unit, float> onUpdateAction;

    private float _damageTick;
    public float damageTick
    {
        get
        {
            return _damageTick;
        }
        set
        {
            _damageTick = value;
        }
    }


    public CrippleEffect(float damageTick, DamageData damageData)
    {
        this.damageTick = damageTick;
        this.damageData = damageData;
        this.onUpdateAction = new Action<Unit, float>(OnUpdate);
        this.period = defaultPeriod;
    }

    public CrippleEffect()
    {
        this.onUpdateAction = new Action<Unit, float>(OnUpdate);
        this.period = defaultPeriod;
        this.damageTick = defaultDamageTick;
    }

    public static CrippleEffect deepCopy(CrippleEffect effect)
    {
        return new CrippleEffect(effect.damageTick, DamageData.deepCopy(effect.damageData));
    }

    public override void applyEffect(Unit unit)
    {
        if (unit.effectData.hasEffect(EffectType.CrippleEffect))
        {
            unit.effectData.getEffect(type).period = this.period;
            unit.effectData.getEffect(type).damage = this.damageData;
        }
        else
        {
            unit.effectData.effects.Add(this);
            unit.effectData.crippleStack++;
            this.LastPosition = unit.transform.position;
            unit.OnUpdateEvent += onUpdateAction;
        }
    }

    public override void removeEffect(Unit unit)
    {
        unit.effectData.effects.Remove(this);
        unit.OnUpdateEvent -= onUpdateAction;
        unit.effectData.crippleStack--;
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

        int distance = (int)(unit.transform.position - LastPosition).magnitude;
        if(distance >= 1)
        {
            damageData.physicalDamage *= distance;
            unit.receiveDamage(damageData, null);
            LastPosition = unit.transform.position;
        }
    }
}
