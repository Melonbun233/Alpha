using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public int health;
    internal int _maxHealth;
    public GameObject destroyEffect;
    public float destroyEffectPeriod;

    void Update()
    {

        // Check object's health, if its lower or equal to 0, destroy it.
        // If we have provided the destroy effect, instantiate the effect with a timeout.
        if (health <= 0) {        
            if (destroyEffect) {
                GameObject effect = Instantiate(destroyEffect, transform.position, transform.rotation);
                Destroy(effect, destroyEffectPeriod);
            }
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _maxHealth = health;
    }

    public void hit(int damage) {
        // Avoid negative health
        if (health <= damage) {
            health = 0;
        } else {
            health -= damage;
        }
    }

    public void heal(int healing) {
        // Avoid overheal
        if (health + healing > _maxHealth) {
            health = _maxHealth;
        } else {
            health += healing;
        }
    }

    // fully heal the object to its current max health
    public void fullyheal() {
        health = _maxHealth;
    }

    public void increMaxHealth(int increment) {
        _maxHealth += increment;
           heal(increment);
    }


}
