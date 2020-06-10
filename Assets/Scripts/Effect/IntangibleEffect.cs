using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IntangibleEffect : Effect
{
    public const float defaultPeriod = 3f;
    public const bool defaultStackable = false;
    public override EffectType type { get; } = EffectType.IntangibleEffect;
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

    public IntangibleEffect(float Period)
    {
        this.onUpdateAction = new Action<Unit, float>(OnUpdate);
        this.period = Period;
    }

    public IntangibleEffect()
    {
        this.onUpdateAction = new Action<Unit, float>(OnUpdate);
        this.period = defaultPeriod;
    }

    public static IntangibleEffect deepCopy(IntangibleEffect effect)
    {
        return new IntangibleEffect(effect.period);
    }

    public override void applyEffect(Unit unit)
    {
        if (unit.effectData.hasEffect(EffectType.IntangibleEffect))
        {
            unit.effectData.getEffect(type).period = this.period;
        }
        else
        {
            unit.isIntangible = true;
            unit.effectData.effects.Add(this);
            unit.effectData.intangibleEffectStack++;
            unit.OnUpdateEvent += onUpdateAction;
        }
    }

    public override void removeEffect(Unit unit)
    {
        unit.isIntangible = false;
        unit.effectData.effects.Remove(this);
        unit.OnUpdateEvent -= onUpdateAction;
        unit.effectData.intangibleEffectStack--;
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
