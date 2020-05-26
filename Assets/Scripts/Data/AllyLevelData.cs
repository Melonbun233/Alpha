using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AllyLevelData {
    public int cost;
    public int level;
    public int exp;
    public float cd;
    // may add other properties like maxExp

    public AllyLevelData(int cost, int level, int exp, float cd) {
        this.cost = cost;
        this.level = level;
        this.exp = exp;
        this.cd = cd;
    }

    public static AllyLevelData deepCopy(AllyLevelData data) {
        return new AllyLevelData(data.cost, data.level, data.exp, data.cd);
    }
}
