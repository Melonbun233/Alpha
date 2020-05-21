using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AllyData: UnitData {
    public AllyType allyType;
    public AllyLevelData levelData;

    public AllyData(HealthData healthData, AttackData attackData,
        ResistanceData resistanceData, MoveData moveData, 
        AllyType allyType, AllyLevelData levelData) : base(healthData, attackData,
        resistanceData, moveData) {
            this.allyType = allyType;
            this.levelData = levelData;
        }

    public static GameObject copyData(GameObject obj, AllyData data) {
        if (obj.GetComponent<Ally>() == null) {
            return null;
        }

        UnitData.copyData(obj, (UnitData)data);
        Ally ally = obj.GetComponent<Ally>();

        ally.allyType = data.allyType;
        ally.levelData = data.levelData;
        return obj;
    }
}

[System.Serializable]
public enum AllyType {
    Ranger, // rangers can only be placed on walls and will not block enemies
    Blocker // blockers can only be placed on vallys and will block enemies
}

// Base class for all allies
public class Ally : Unit
{
    [Header("Ally Type")]
    public AllyType allyType;
    public AllyLevelData levelData;

    // Update the attack target
    // Default alg is to find the nearest enemy within range
    // Enemies not within range will not be set as target
    public override void updateAttackTarget() {
        _attackTargets.Clear();

        List<GameObject> _attackTargetsWithinRange = new List<GameObject>();
        Utils.findGameObjectsWithinRange(_attackTargetsWithinRange, transform.position, 
            attackData.attackRange, "Enemy");
        Utils.sortByDistance(_attackTargetsWithinRange, transform.position);

        for (int i = 0; i < attackData.attackNumber; i ++) {
            if (i < _attackTargetsWithinRange.Count) {
                _attackTargets.Add(_attackTargetsWithinRange[i]);
            } else {
                break;
            }
        }

    }

    public override void dealAoeDamage(GameObject initialTarget) {
        List<GameObject> nearbyEnemies = new List<GameObject>();
        Utils.findGameObjectsWithinRange(nearbyEnemies, initialTarget.transform.position,
            attackData.attackAoeRange, "Enemy");

        foreach(GameObject nearbyEnemy in nearbyEnemies) {
            if (initialTarget != nearbyEnemy) {
                nearbyEnemy.GetComponent<Destroyable>().receiveDamage(attackData.attackDamage, gameObject);
            }
        }
    }

    // Typically allies doesn't move
    public override void move() {}

    public override void updateMoveTarget() {}

    // Spawn an object with predefined data
    public static GameObject spawn(GameObject prefab, AllyData data, 
        Vector3 position, Quaternion rotation) {
        if (prefab.GetComponent<Ally>() == null) {
            Debug.Log("Cannot spawn an non-ally object");
            return null;
        }

        GameObject obj = Instantiate(prefab, position, rotation);
        AllyData.copyData(obj, data);
        return obj;
    }
}
