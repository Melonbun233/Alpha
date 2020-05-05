﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public GameObject destroyEffect;
    public float destroyEffectPeriod;

    public virtual void Start() {
        if (maxHealth < health) {
            health = maxHealth;
        }
    }

    // Does nothing. But may be override by an extended class
    public virtual void Update() {}

    // Deal damage to this object. This object will not be killed if the result is zero. 
    // If the damage is negative, the attack will heal this object instead.
    // Over damage will not result in negative health!
    public virtual void receiveDamage(int damage) {
        if (damage < 0) {
            receiveHealing(-damage);
            return;
        }

        // Avoid negative health
        if (health <= damage) {
            health = 0;
        } else {
            health -= damage;
        }
    }

    public virtual void receiveHealing(int healing) {
        if (healing < 0) {
            receiveDamage(-healing);
            return;
        }

        // Avoid overheal
        if (health + healing > maxHealth) {
            health = maxHealth;
        } else {
            health += healing;
        }
    }

    // fully heal the object to its current max health
    public virtual void fullyHeal() {
        health = maxHealth;
    }

    // Increment the maximum health of this object
    // A negative increment will result in decrement of max health
    public virtual void increMaxHealth(int increment) {
        if (increment < 0) {
            decreMaxHealth(-increment);
            return;
        }

        maxHealth += increment;
        receiveHealing(increment);
    }

    // Decrement max health. 
    // The result will not be negative.
    // If the health is higher than the new max health, current health 
    // will be set to the new max health.
    // If the current health is set to zero, this object will not be killed.
    // A negative decrement will result in increment of max health
    public virtual void decreMaxHealth(int decrement) {
        if (decrement < 0) {
            increMaxHealth(-decrement);
            return;
        }

        if (maxHealth - decrement < 0) {
            maxHealth = 0;
        } else {
            maxHealth -= decrement;
        }
        
        if (health < maxHealth) {
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