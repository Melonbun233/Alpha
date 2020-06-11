using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StunEffect : Effect
{
    public const float defaultPeriod = 2f;
    public const bool defaultStackable = false;
    public override EffectType type { get; } = EffectType.StunEffect;
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

    public StunEffect(float Period)
    {
        this.onUpdateAction = new Action<Unit, float>(OnUpdate);
        this.period = Period;
    }

    public StunEffect()
    {
        this.onUpdateAction = new Action<Unit, float>(OnUpdate);
        this.period = defaultPeriod;
    }

    public static StunEffect deepCopy(StunEffect effect)
    {
        return new StunEffect(effect.period);
    }

    public override void applyEffect(Unit unit)
    {
        if (unit.effectData.hasEffect(EffectType.StunEffect))
        {
            unit.effectData.getEffect(type).period = this.period;
        }
        else
        {
            unit.isStun = true;
            unit.effectData.effects.Add(this);
            unit.effectData.stunEffectStack++;
            unit.OnUpdateEvent += onUpdateAction;
        }
    }

    public override void removeEffect(Unit unit)
    {
        unit.isStun = false;
        unit.effectData.effects.Remove(this);
        unit.OnUpdateEvent -= onUpdateAction;
        unit.effectData.stunEffectStack--;
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
