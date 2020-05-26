using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData: UnitData {
    public float visionRange;
    public EnemyData(HealthData healthData, AttackData attackData,
        ResistanceData resistanceData, MoveData moveData, EffectData effectData, float visionRange) :
        base(healthData, attackData, resistanceData, moveData, effectData) {
            this.visionRange = visionRange;
        }

    public static GameObject copyData(GameObject obj, EnemyData data) {
        if (obj.GetComponent<Enemy>() == null) {
            return null;
        }

        UnitData.copyData(obj, (UnitData) data);
        Enemy enemy = obj.GetComponent<Enemy>();

        enemy.visionRange = data.visionRange;
        return obj;
    }
}
