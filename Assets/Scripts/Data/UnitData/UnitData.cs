using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class UnitData{
    public HealthData healthData;
    public AttackData attackData;
    public ResistanceData resistanceData;
    public MoveData moveData;
    public EffectData effectData;

    public UnitData(HealthData healthData, AttackData attackData, 
        ResistanceData resistanceData, MoveData moveData, EffectData effectData){
        this.healthData = healthData;
        this.attackData = attackData;
        this.resistanceData = resistanceData;
        this.moveData = moveData;
        this.effectData = effectData;
    }

    public static GameObject copyData(GameObject obj, UnitData data) {
        Unit unit = obj.GetComponent<Unit>();
        if (unit == null) {
            return null;
        }
        unit.healthData = HealthData.deepCopy(data.healthData);
        unit.attackData = AttackData.deepCopy(data.attackData);
        unit.resistanceData = ResistanceData.deepCopy(data.resistanceData);
        unit.moveData = MoveData.deepCopy(data.moveData);
        unit.effectData = EffectData.deepCopy(data.effectData);
        return obj;
    }
}
