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

    // Update the attack target
    // Default alg is to find the nearest enemy within range
    // Enemies not within range will not be set as target
    public override void updateAttackTarget() {
        _attackTargets = Utils.findGameObjectsWithinRange(transform.position, attackRange, "Enemy");

        if (_attackTargets.Count == 0) {
            // no enemy within range
            _attackTarget = null;
            return;
        } else {
            _attackTarget = Utils.findNearestGameObject(transform.position, _attackTargets);
        }
    }

    // Typically allies doesn't move
    public override void move() {}

    public override void updateMoveTarget() {}
}
