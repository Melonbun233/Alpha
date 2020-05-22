using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AllyData: UnitData {
    public AllyType allyType1;
    public AllyType allyType2;
    public AllyLevelData levelData;

    public AllyData(HealthData healthData, AttackData attackData,
        ResistanceData resistanceData, MoveData moveData, 
        AllyType allyType1, AllyType allyType2, AllyLevelData levelData) : 
        base(healthData, attackData, resistanceData, moveData) {
            this.allyType1 = allyType1;
            this.allyType2 = allyType2;
            this.levelData = levelData;
        }

    public static GameObject copyData(GameObject obj, AllyData data) {
        if (obj.GetComponent<Ally>() == null) {
            return null;
        }

        UnitData.copyData(obj, (UnitData)data);
        Ally ally = obj.GetComponent<Ally>();

        ally.allyType1 = data.allyType1;
        ally.allyType2 = data.allyType2;
        ally.levelData = data.levelData;
        return obj;
    }

    // One ally can only have up to two types
    // If non or only one type is selected, this returns true
    // Otherwise return false
    public bool hasAllyTypeSlot() {
        return allyType1 == AllyType.None || allyType2 == AllyType.None;
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

[System.Serializable]
public enum AllyType {
    Ranger, 
    Blocker,
    Fire,
    Water,
    Wind,
    Thunder,
    None
}

// Base class for all allies
public class Ally : Unit
{
    [Header("Ally Type")]
    public AllyType allyType1 = AllyType.None;
    public AllyType allyType2 = AllyType.None;
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

    public bool hasAllyType(AllyType type) {
        return allyType1 == type || allyType2 == type;
    }
}
