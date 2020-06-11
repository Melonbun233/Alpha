using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    [Header("Health Settings")]
    public HealthData healthData;

    [Header("Resistance Settings")]
    public ResistanceData resistanceData;

    [Header("Destory Effect Settings")]
    public GameObject destroyEffect;
    public float destroyEffectPeriod;

    [Header("Receive Attack At")]
    // used to be pointed by attack indicator
    public Vector3 center;

    //Reference to the last unit that attacked this.
    public Unit LastAttacker = null;
    public bool isIntangible = false;
    public bool isStun = false;
    public bool isFragile = false;

    // Called on receive damage
    public event Action<Unit, int> OnReceiveDamageEvent;
    // Called on receive healing
    public event Action<Unit, int> OnReceiveHealingEvent;

    protected virtual void Start() {
        if (healthData.maxHealth < healthData.health) {
            healthData.health = healthData.maxHealth;
        }
    }

    // Check if dead. Kill self if dead.
    protected virtual void Update() {
        if (isDead()) {
            kill();
        }
    }

    // Deal damage to this object. This object will be killed if the health
    // is 0 in the next frame
    // Over damage will not result in negative health!
    public virtual void receiveDamage(DamageData damageData, GameObject receiveFrom) {
        if (this.isIntangible == true) return;

        int damage = damageData.getTotalDamage(this.resistanceData);
        if (isFragile) damage = (int)(damage * 1.3f);

        if (OnReceiveDamageEvent != null) {
            OnReceiveDamageEvent(receiveFrom.GetComponent<Unit>(), damage);
        }

        // Avoid negative health
        if (healthData.health <= damage) {
            healthData.health = 0;
        } else {
            healthData.health -= damage;
        }
    }

    // Receive a healing
    // Healing should not be negative
    public virtual void receiveHealing(int healing, GameObject receiveFrom) {
        if (isDead()) {
            return;
        }

        if (healing < 0) {
            return;
        }

        int actualHealing = Utils.mult(healing, healthData.healingPercentMultiplier, 
            healthData.healingFlatMultiplier);
        if (actualHealing < 0) {
            return;
        }

        if (OnReceiveHealingEvent != null) {
            OnReceiveHealingEvent(receiveFrom.GetComponent<Unit>(), actualHealing);
        }
        
        // Avoid overheal
        if (healthData.health + actualHealing > healthData.maxHealth) {
            healthData.health = healthData.maxHealth;
        } else {
            healthData.health += actualHealing;
        }
    }

    // Increment the maximum health of this object
    // A negative increment will result in decrement of max health
    public virtual void increMaxHealth(int increment, GameObject receiveFrom) {
        if (isDead()) {
            return;
        }

        if (increment < 0) {
            decreMaxHealth(-increment, receiveFrom);
            return;
        }

        healthData.maxHealth += increment;
        healthData.health += increment;
    }

    // Decrement max health. 
    // The result will not be negative.
    // If the health is higher than the new max health, current health 
    // will be set to the new max health.
    // A negative decrement will result in increment of max health
    public virtual void decreMaxHealth(int decrement, GameObject receiveFrom) {
        if (isDead()) {
            return;
        } 
        if (decrement < 0) {
            increMaxHealth(-decrement, receiveFrom);
            return;
        }

        if (healthData.maxHealth - decrement < 0) {
            healthData.maxHealth = 0;
        } else {
            healthData.maxHealth -= decrement;
        }
        
        if (healthData.health > healthData.maxHealth) {
            healthData.health = healthData.maxHealth;
        }
        
    }

    public virtual bool isDead() {
        return healthData.health <= 0;
    }


    // destroy self with possible effect
    public virtual void kill() {
        healthData.health = 0;
        if (destroyEffect) {
            GameObject effect = Instantiate(destroyEffect, transform.position, transform.rotation);
            Destroy(effect, destroyEffectPeriod);
        }
        Destroy(gameObject);
    }


}
