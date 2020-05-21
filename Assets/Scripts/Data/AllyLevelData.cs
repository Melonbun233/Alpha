using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AllyLevelData {
    public int cost;
    public int level;
    public int exp;
    // may add other properties like maxExp

    public AllyLevelData(int cost, int level, int exp) {
        this.cost = cost;
        this.level = level;
        this.exp = exp;
    }
}
