using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StunAttackEffect : Effect
{
    public const float defaultPeriod = float.PositiveInfinity;
    public const bool defaultStackable = false;
    public const int defaultStunEffectPerAttack = 1;


    public override EffectType type { get; } = EffectType.StunAttackEffect;
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


    // Add amount of stun stacks to the target on every attack
    public int stunEffectPerAttack = defaultStunEffectPerAttack;
    // stun effect used on each attack
    public StunEffect stunEffect = new StunEffect();

    Action<Unit, Unit> onAttackAction;
    Action<Unit, float> onUpdateAction;


    // Constructor for the stun attack effect
    // Note that we will directly use the reference of the provided stun effect
    public StunAttackEffect(int stunEffectPerAttack, StunEffect stunEffect)
    {
        this.stunEffectPerAttack = stunEffectPerAttack;
        this.stunEffect = stunEffect;
        this.onAttackAction = new Action<Unit, Unit>(OnAttack);
        this.onUpdateAction = new Action<Unit, float>(OnUpdate);
        this.period = defaultPeriod;
    }

    public StunAttackEffect()
    {
        this.onAttackAction = new Action<Unit, Unit>(OnAttack);
        this.onUpdateAction = new Action<Unit, float>(OnUpdate);
        this.period = defaultPeriod;
    }

    public static StunAttackEffect deepCopy(StunAttackEffect effect)
    {
        StunAttackEffect ret = new StunAttackEffect(effect.stunEffectPerAttack,
            StunEffect.deepCopy(effect.stunEffect));
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
            StunAttackEffect oldEffect = effectData.getEffect(type) as StunAttackEffect;
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
        unit.effectData.stunAttackEffectStack++;
    }

    // Action attached to OnAttackEvent on a unit, and it applies stun effect
    // on unit's attack
    private void OnAttack(Unit attacker, Unit target)
    {
        for (int i = 0; i < stunEffectPerAttack; i++)
        {
            StunEffect stun = StunEffect.deepCopy(stunEffect);
            stun.applyEffect(target);
        }
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
        unit.effectData.stunAttackEffectStack--;
    }
}
