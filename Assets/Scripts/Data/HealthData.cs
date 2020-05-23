using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HealthData {
    public int health;
    public int maxHealth;
    public float healingPercentMultiplier;
    public int healingFlatMultiplier;

    public HealthData(int health, int maxHealth, float healingPercentMultiplier, 
        int healingFlatMultiplier) {
            this.health = health;
            this.maxHealth = maxHealth;
            this.healingPercentMultiplier = healingPercentMultiplier;
            this.healingFlatMultiplier = healingFlatMultiplier;
    }

    public static HealthData deepCopy(HealthData data) {
        return new HealthData(data.health, data.maxHealth, data.healingPercentMultiplier, 
            data.healingFlatMultiplier);
    }
}
