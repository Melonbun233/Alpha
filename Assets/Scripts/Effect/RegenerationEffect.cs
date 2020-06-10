using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RegenerationEffect : Effect
{
    public const float defaultPeriod = 5f;
    public const float defaultHealingPeriod = 1f;
    public const bool defaultStackable = true;

    public override EffectType type { get; } = EffectType.RegenerationEffect;
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

    // Period between each healing dealt the attached unit
    private float _healingPeriod;
    public float healingPeriod
    {
        get
        {
            return _healingPeriod;
        }
        set
        {
            _healingPeriod = value;
            healingPeriodCD = value;
        }
    }
    public float healingPeriodCD;

    Action<Unit, float> onUpdateAction;


    public int healAmount;
    public RegenerationEffect(float HealingPeriod, int healAmount)
    {
        this.healingPeriod = HealingPeriod;
        this.healAmount = healAmount;
        this.onUpdateAction = new Action<Unit, float>(OnUpdate);
        this.period = defaultPeriod;
    }

    public RegenerationEffect()
    {
        this.onUpdateAction = new Action<Unit, float>(OnUpdate);
        this.period = defaultPeriod;
        this.healingPeriod = defaultHealingPeriod;
    }

    public static RegenerationEffect deepCopy(RegenerationEffect effect)
    {
        return new RegenerationEffect(effect.healingPeriod, effect.healAmount);
    }

    public override void applyEffect(Unit unit)
    {
        // regen effect is always stackable, we dont check it anyway
        unit.effectData.effects.Add(this);
        unit.effectData.regenerationStack++;
        unit.OnUpdateEvent += onUpdateAction;
    }

    public override void removeEffect(Unit unit)
    {
        unit.effectData.effects.Remove(this);
        unit.OnUpdateEvent -= onUpdateAction;
        unit.effectData.regenerationStack--;
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

        // check time to heal
        healingPeriodCD -= deltaTime;
        if (healingPeriodCD <= 0)
        {
            unit.receiveHealing(healAmount, null);
            healingPeriodCD = healingPeriod;
        }

    }
}
