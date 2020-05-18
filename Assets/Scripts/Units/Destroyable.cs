using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    [Header("Health Settings")]
    public int health;
    public int maxHealth;
    public float healingPercentMultiplier = 0f;
    public int healingFlatMultiplier = 0;

    [Header("Resistance Settings")]
    public ResistanceData resistance;

    [Header("Destory Effect Settings")]
    public GameObject destroyEffect;
    public float destroyEffectPeriod;

    [Header("Receive Attack At")]
    // used to be pointed by attack indicator
    public Vector3 center;

    protected virtual void Start() {
        if (maxHealth < health) {
            health = maxHealth;
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
        int damage = damageData.getTotalDamage(this.resistance);

        // Avoid negative health
        if (health <= damage) {
            health = 0;
        } else {
            health -= damage;
        }
    }

    // Receive a healing
    // Healing should not be negative
    public virtual void receiveHealing(int healing, GameObject receiveFrom) {
        if (healing < 0) {
            return;
        }

        int actualHealing = Utils.mult(healing, healingPercentMultiplier, healingFlatMultiplier);
        if (actualHealing < 0) {
            return;
        }
        
        // Avoid overheal
        if (health + actualHealing > maxHealth) {
            health = maxHealth;
        } else {
            health += actualHealing;
        }
    }

    // Increment the maximum health of this object
    // A negative increment will result in decrement of max health
    public virtual void increMaxHealth(int increment, GameObject receiveFrom) {
        if (increment < 0) {
            decreMaxHealth(-increment, receiveFrom);
            return;
        }

        maxHealth += increment;
        health += increment;
    }

    // Decrement max health. 
    // The result will not be negative.
    // If the health is higher than the new max health, current health 
    // will be set to the new max health.
    // A negative decrement will result in increment of max health
    public virtual void decreMaxHealth(int decrement, GameObject receiveFrom) {
        if (decrement < 0) {
            increMaxHealth(-decrement, receiveFrom);
            return;
        }

        if (maxHealth - decrement < 0) {
            maxHealth = 0;
        } else {
            maxHealth -= decrement;
        }
        
        if (health > maxHealth) {
            health = maxHealth;
        }
        
    }

    public virtual bool isDead() {
        return health <= 0;
    }


    // destroy self with possible effect
    public virtual void kill() {
        health = 0;
        if (destroyEffect) {
            GameObject effect = Instantiate(destroyEffect, transform.position, transform.rotation);
            Destroy(effect, destroyEffectPeriod);
        }
        Destroy(gameObject);
    }


}
