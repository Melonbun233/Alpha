using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageData
{
    [Header("Initial Damage Settings")]
    [SerializeField]
    public int physicalDamage;
    [SerializeField]
    public int fireDamage;
    [SerializeField]
    public int waterDamage;
    [SerializeField]
    public int windDamage;
    [SerializeField]
    public int thunderDamage;

    [Header("Initial Muiltiplier Settings")]
    public float physicalDamagePercentMultiplier = 0f;
    public int physicalDamageFlatMultiplier = 0;
    public float fireDamagePercentMultiplier = 0f;
    public int fireDamageFlatMultiplier = 0;
    public float waterDamagePercentMultiplier = 0f;
    public int waterDamageFlatMultiplier = 0;
    public float windDamagePercentMultiplier = 0f;
    public int windDamageFlatMultiplier = 0;
    public float thunderDamagePercentMultiplier = 0f;
    public int thunderDamageFlatMultiplier = 0;

    public DamageData(int physic, int fire, int water, int wind, int thunder) {
        this.physicalDamage = physic;
        this.fireDamage = fire;
        this.waterDamage = water;
        this.windDamage = wind;
        this.thunderDamage = thunder;
    }

    public static DamageData deepCopy(DamageData data) {
        return new DamageData(data.physicalDamage, data.fireDamage, data.waterDamage,
            data.windDamage, data.thunderDamage);
    }

    // Calculte damage with corresponding resistance
    private int calculateRealDamage (int damage, int resistance) {
        float multiplier = resistance >= 0 ? 
            100f / (100f + (float)resistance) :
            2 - (100f / (100f - (float)resistance));

        return (int)((float)damage * multiplier);
    }

    // Calculate the damage dealt to another object with its resistance data
    // This damage can never be negative
    public int getTotalDamage(ResistanceData resis) {
        return calculateRealDamage(this.getPhysicalDamage(), resis.getPhysicalResistance())
            + calculateRealDamage(this.getFireDamage(), resis.getFireResistance())
            + calculateRealDamage(this.getWaterDamage(), resis.getWaterResistance())
            + calculateRealDamage(this.getWindDamage(), resis.getWindResistance())
            + calculateRealDamage(this.getThunderDamage(), resis.getThunderResistance());
    }

    public int getPhysicalDamage() {
        int actual = Utils.mult(physicalDamage, physicalDamagePercentMultiplier, physicalDamageFlatMultiplier);
        return actual < 0 ? 0 : actual;
    }


    public int getFireDamage() {
        int actual = Utils.mult(fireDamage, fireDamagePercentMultiplier, fireDamageFlatMultiplier);
        return actual < 0 ? 0 : actual;
    }

    public int getWaterDamage() {
        int actual = Utils.mult(waterDamage, waterDamagePercentMultiplier, waterDamageFlatMultiplier);
        return actual < 0 ? 0 : actual;
    }

    public int getWindDamage() {
        int actual = Utils.mult(windDamage, windDamagePercentMultiplier, windDamageFlatMultiplier);
        return actual < 0 ? 0 : actual;
    }

    public int getThunderDamage() {
        int actual = Utils.mult(thunderDamage, thunderDamagePercentMultiplier, thunderDamageFlatMultiplier);
        return actual < 0 ? 0 : actual;
    }

}
