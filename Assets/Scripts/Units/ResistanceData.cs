using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResistanceData
{
    [Header("Resistance Settings")]
    [SerializeField]
    private int physicalResistance;
    [SerializeField]
    private int fireResistance;
    [SerializeField]
    private int waterResistance;
    [SerializeField]
    private int windResistance;
    [SerializeField]
    private int thunderResistance;

    [Header("Initial Multiplier Settings")]
    public float physicalResisPercentMultiplier = 0f;
    public int physicalResisFlatMultiplier = 0;
    public float fireResisPercentMultiplier = 0f;
    public int fireResisFlatMultiplier = 0;
    public float waterResisPercentMultiplier = 0f;
    public int waterResisFlatMultiplier = 0;
    public float windResisPercentMultiplier = 0f;
    public int windResisFlatMultiplier = 0;
    public float thunderResisPercentMultiplier = 0f;
    public int thunderResisFlatMultiplier = 0;


    public ResistanceData(int physic, int fire, int water, int wind, int thunder) {
        this.physicalResistance = physic;
        this.fireResistance = fire;
        this.waterResistance = water;
        this.windResistance = wind;
        this.thunderResistance = thunder;
    }

    public int getPhysicalResistance() {
        return Utils.mult(physicalResistance, physicalResisPercentMultiplier, physicalResisFlatMultiplier);
    }

    public int getFireResistance() {
        return Utils.mult(fireResistance, fireResisPercentMultiplier, fireResisFlatMultiplier);
    }

    public int getWaterResistance() {
        return Utils.mult(waterResistance, waterResisPercentMultiplier, waterResisFlatMultiplier);
    }

    public int getWindResistance() {
        return Utils.mult(windResistance, windResisPercentMultiplier, windResisFlatMultiplier);
    }

    public int getThunderResistance() {
        return Utils.mult(thunderResistance, thunderResisPercentMultiplier, thunderResisFlatMultiplier);
    }


}
