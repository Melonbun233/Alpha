using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KnockBackAttackEffect : Effect
{
    public const float defaultPeriod = float.PositiveInfinity;
    public const bool defaultStackable = false;
    public const int defaultKnockEffectPerAttack = 1;
    public const float defaultKnockBackDistance = 2f;

    public override EffectType type { get; } = EffectType.KnockBackAttackEffect;
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

    // Add amount of knockback stacks to the target on every attack
    public int KnockEffectPerAttack = defaultKnockEffectPerAttack;
    public float knockBackDistance = defaultKnockBackDistance;

    Action<Unit, Unit> onAttackAction;
    Action<Unit, float> onUpdateAction;

    // Constructor for the knockback attack effect
    // Note that we will directly use the reference of the provided knockback effect
    public KnockBackAttackEffect(float knockBackDistance)
    {
        this.knockBackDistance = knockBackDistance;
        this.onAttackAction = new Action<Unit, Unit>(OnAttack);
        this.onUpdateAction = new Action<Unit, float>(OnUpdate);
        this.period = defaultPeriod;
    }

    public KnockBackAttackEffect()
    {
        this.onAttackAction = new Action<Unit, Unit>(OnAttack);
        this.onUpdateAction = new Action<Unit, float>(OnUpdate);
        this.period = defaultPeriod;
    }

    public static KnockBackAttackEffect deepCopy(KnockBackAttackEffect effect)
    {
        KnockBackAttackEffect ret = new KnockBackAttackEffect(effect.knockBackDistance);
        ret.period = effect.period;
        ret.stackable = effect.stackable;
        return ret;
    }

    public override void applyEffect(Unit unit)
    {
        EffectData effectData = unit.effectData;
        bool hasEffect = effectData.hasEffect(type);

        // Need to check whether overwrite the current effect
        if (hasEffect && !stackable)
        {
            // overwrite the effect only when the new effect has longer period
            KnockBackAttackEffect oldEffect = effectData.getEffect(type) as KnockBackAttackEffect;
            if (oldEffect.periodCD < this.period)
            {
                oldEffect.removeEffect(unit);
            }
            else
            {
                return;
            }
        }

        // Add the new effect to the unit's effectdata
        unit.effectData.effects.Add(this);
        unit.OnUpdateEvent += onUpdateAction;
        unit.OnAttackEvent += onAttackAction;
        unit.effectData.knockBackAttackEffectStack++;
    }

    // Action attached to OnAttackEvent on a unit, and it applies knockback effect
    // on unit's attack
    private void OnAttack(Unit attacker, Unit target)
    {
        target.transform.GetComponent<Rigidbody>().AddForce(-target.transform.forward * knockBackDistance);
    }

    // Calculate the period of this effect. 
    private void OnUpdate(Unit unit, float deltaTime)
    {
        if (float.IsPositiveInfinity(period))
        {
            return;
        }

        periodCD -= deltaTime;

        // remove all delegates related to this effect instance from the unit
        if (periodCD <= 0)
        {
            removeEffect(unit);
        }
    }

    // remove all delegates related to this effect instance from the unit
    public override void removeEffect(Unit unit)
    {
        unit.effectData.effects.Remove(this);
        unit.OnUpdateEvent -= onUpdateAction;
        unit.OnAttackEvent -= onAttackAction;
        unit.effectData.knockBackAttackEffectStack--;
    }
}
