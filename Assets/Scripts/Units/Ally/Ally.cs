using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AllyType {
    Ranger, // rangers can only be placed on walls and will not block enemies
    Blocker // blockers can only be placed on vallys and will block enemies
}

// Base class for all allies
public class Ally : Unit
{
    [Header("Ally Type")]
    public AllyType type;
    public int cost;
    public int level;
    public int exp;

    // Update the attack target
    // Default alg is to find the nearest enemy within range
    // Enemies not within range will not be set as target
    public override void updateAttackTarget() {
        _attackTargets.Clear();

        List<GameObject> _attackTargetsWithinRange = new List<GameObject>();
        Utils.findGameObjectsWithinRange(_attackTargetsWithinRange, transform.position, 
            attackRange, "Enemy");
        Utils.sortByDistance(_attackTargetsWithinRange, transform.position);

        for (int i = 0; i < attackNumber; i ++) {
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
            attackAoeRange, "Enemy");

        foreach(GameObject nearbyEnemy in nearbyEnemies) {
            if (initialTarget != nearbyEnemy) {
                nearbyEnemy.GetComponent<Destroyable>().receiveDamage(attackDamage, gameObject);
            }
        }
    }

    // Typically allies doesn't move
    public override void move() {}

    public override void updateMoveTarget() {}
}
