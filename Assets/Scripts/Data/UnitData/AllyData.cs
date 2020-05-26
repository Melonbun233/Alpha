using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AllyData: UnitData {
    public AllyType allyType1;
    public AllyType allyType2;
    public AllyLevelData allyLevelData;

    public AllyData(HealthData healthData, AttackData attackData,
        ResistanceData resistanceData, MoveData moveData, EffectData effectData,
        AllyType allyType1, AllyType allyType2, AllyLevelData allyLevelData) : 
        base(healthData, attackData, resistanceData, moveData, effectData) {
            this.allyType1 = allyType1;
            this.allyType2 = allyType2;
            this.allyLevelData = allyLevelData;
        }

    public static GameObject copyData(GameObject obj, AllyData data) {
        if (obj.GetComponent<Ally>() == null) {
            return null;
        }

        UnitData.copyData(obj, (UnitData)data);
        Ally ally = obj.GetComponent<Ally>();

        ally.allyType1 = data.allyType1;
        ally.allyType2 = data.allyType2;
        ally.allyLevelData = AllyLevelData.deepCopy(data.allyLevelData);
        return obj;
    }

    // One ally can only have up to two types
    // If non or only one type is selected, this returns true
    // Otherwise return false
    public bool hasAllyTypeSlot() {
        return allyType1 == AllyType.None || allyType2 == AllyType.None;
    }

    //return true if contains type.
    public bool isType(AllyType type)
    {
        return allyType1 == type || allyType2 == type;
    }

    // Set the ally type if there's at least one ally type slot free
    // Return true on successful setting
    // If there are already two types, nothing is set and false is returned
    public bool setAllyType(AllyType type) {
        if (!hasAllyTypeSlot()) {
            return false;
        }

        if (allyType1 == AllyType.None) {
            allyType1 = type;
        } else {
            allyType2 = type;
        }

        return true;
    }
}
