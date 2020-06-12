using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FragilEffect : Effect
{
    public const float defaultPeriod = 5f;
    public const bool defaultStackable = false;
    public override EffectType type { get; } = EffectType.FragileEffect;
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

    Action<Unit, float> onUpdateAction;

    public FragilEffect(float Period)
    {
        this.onUpdateAction = new Action<Unit, float>(OnUpdate);
        this.period = Period;
    }

    public FragilEffect()
    {
        this.onUpdateAction = new Action<Unit, float>(OnUpdate);
        this.period = defaultPeriod;
    }

    public static FragilEffect deepCopy(FragilEffect effect)
    {
        return new FragilEffect(effect.period);
    }

    public override void applyEffect(Unit unit)
    {
        if (unit.effectData.hasEffect(EffectType.FragileEffect))
        {
            unit.effectData.getEffect(type).period = this.period;
        }
        else
        {
            unit.effectData.effects.Add(this);
            unit.effectData.fragileEffectStack++;
            unit.OnUpdateEvent += onUpdateAction;
        }
    }

    public override void removeEffect(Unit unit)
    {
        unit.effectData.effects.Remove(this);
        unit.OnUpdateEvent -= onUpdateAction;
        unit.effectData.fragileEffectStack--;
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
