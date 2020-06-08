using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CrippleAttackEffect : Effect
{
    public const float defaultPeriod = float.PositiveInfinity;
    public const bool defaultStackable = false;
    public const int defaultCripplePerAttack = 1;

    public override EffectType type { get; } = EffectType.CrippleAttackEffect;
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

    // Add amount of cripple stacks to the target on every attack
    public int cripplePerAttack = defaultCripplePerAttack;
    public CrippleEffect crippleEffect = new CrippleEffect();

    Action<Unit, Unit> onAttackAction;
    Action<Unit, float> onUpdateAction;

    // Constructor for the cripple attack effect
    // Note that we will directly use the reference of the provided cripple effect
    public CrippleAttackEffect(int crippleEffectPerAttack, CrippleEffect crippleEffect)
    {
        this.cripplePerAttack = crippleEffectPerAttack;
        this.crippleEffect = crippleEffect;
        this.onAttackAction = new Action<Unit, Unit>(OnAttack);
        this.onUpdateAction = new Action<Unit, float>(OnUpdate);
        this.period = defaultPeriod;
    }

    public CrippleAttackEffect()
    {
        this.onAttackAction = new Action<Unit, Unit>(OnAttack);
        this.onUpdateAction = new Action<Unit, float>(OnUpdate);
        this.period = defaultPeriod;
    }

    public static CrippleAttackEffect deepCopy(CrippleAttackEffect effect)
    {
        CrippleAttackEffect ret = new CrippleAttackEffect(effect.cripplePerAttack,
            CrippleEffect.deepCopy(effect.crippleEffect));
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
            CrippleAttackEffect oldEffect = effectData.getEffect(type) as CrippleAttackEffect;
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
        unit.effectData.crippleAttackStack++;
    }

    // Action attached to OnAttackEvent on a unit, and it applies cripple effect
    // on unit's attack
    private void OnAttack(Unit attacker, Unit target)
    {
        for (int i = 0; i < cripplePerAttack; i++)
        {
            CrippleEffect cripple = CrippleEffect.deepCopy(crippleEffect);
            cripple.applyEffect(target);
        }
    }

    public override void removeEffect(Unit unit)
    {
        unit.effectData.effects.Remove(this);
        unit.OnUpdateEvent -= onUpdateAction;
        unit.OnAttackEvent -= onAttackAction;
        unit.effectData.crippleAttackStack--;
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

}
