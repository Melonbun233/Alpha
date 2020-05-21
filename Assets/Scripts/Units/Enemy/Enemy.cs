using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Base class for all enemies
public class Enemy : Unit {


    [Header("Path Finding Settings")]
    // Used to find allies to attack
    public float visionRange;

    // [Header("Priority Settings")]
    // public int priority;


    // Update the attack target of this enemy
    // Default alg is to check if base is within attack range
    // If not, check whether any ally is within range
    // If not, set the attack target as null
    public override void updateAttackTarget() {
        _attackTargets.Clear();

        GameObject _base = GameObject.FindGameObjectWithTag("Base");
        // Check if base is destroyed
        if (_base == null) {
            return;
        }

        int restAttackNumber = attackNumber;

        // Check if the base is within attack range
        Utils.findGameObjectsWithinRange(_attackTargets, transform.position, attackRange, "Base");
        if (_attackTargets.Count != 0 && restAttackNumber > 0) {
            _attackTargets.Add(_base);
            restAttackNumber --;
        }


        // Check if there's an ally within attack range
        List<GameObject> _attackTargetsWithinRange = new List<GameObject>();
        Utils.findGameObjectsWithinRange(_attackTargetsWithinRange, transform.position, 
             attackRange, "Ally");
        Utils.sortByDistance(_attackTargetsWithinRange, transform.position);

        for (int i = 0; i < restAttackNumber; i++) {
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
            attackAoeRange, "Ally");

        foreach(GameObject nearbyEnemy in nearbyEnemies) {
            nearbyEnemy.GetComponent<Destroyable>().receiveDamage(attackDamage, gameObject);
        }
    }

    // Move toward the target until reached the attacking range
    public override void move() {
        // game over
        if (_moveTarget == null) {
            return;
        }

        Transform targetTransform = _moveTarget.transform;
        // Vector3 targetPosition = new Vector3(targetTransform.position.x, transform.position.y, 
        //     targetTransform.position.z);

        Ray oppositeDirection = new Ray(targetTransform.position, 
        transform.position - targetTransform.position);
        
        Vector3 destination = oppositeDirection.GetPoint(attackRange);

        updateNavAgentDestination(destination);
    }

    // Temporarily find blockers within vision range as move target
    // If no target can be found, search for the base
    // If base is destroyed, enemy will continue move to the last destination
    // Later we might replace the alg to a priority queue
    public override void updateMoveTarget() {
        GameObject[] allies = GameObject.FindGameObjectsWithTag("Ally");

        if (allies.Length != 0) {
            float cloestDistance = float.PositiveInfinity;
            foreach (GameObject ally in allies) {
                // only set if the ally is a blocker and within vision range
                if (ally.GetComponent<Ally>().type == AllyType.Blocker && 
                    Utils.isWithinRange(transform.position, ally.transform.position, visionRange)) {
                    // Use the ally that's cloeset
                    float distance = Utils.horizontalDistance(transform, ally.transform);
                    if (distance <= cloestDistance) {
                        _moveTarget = ally;
                        cloestDistance = distance;
                    }
                }
            }

            if (!float.IsPositiveInfinity(cloestDistance)) {
                return;
            }
        }

        // reach here only because all allies are rangers or out of vision
        _moveTarget = GameObject.FindGameObjectWithTag("Base");
    }


    // Draw the vision range of the enemy
    protected override void OnDrawGizmosSelected() {
        base.OnDrawGizmosSelected();
        Utils.drawRange(transform, visionRange, Color.green);
    }

    
}
