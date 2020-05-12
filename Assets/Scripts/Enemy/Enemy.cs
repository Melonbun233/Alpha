using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Base class for all enemies
public class Enemy : Character {


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
        
        GameObject _base = GameObject.FindGameObjectWithTag("Base");
        // Check if base is destroyed
        if (_base == null) {
            _attackTarget = null;
            return;
        }

        // Check if the base is within attack range
        _attackTargets = Utils.findGameObjectsWithinRange(transform.position, attackRange, "Base");
        if (_attackTargets.Count != 0) {
            _attackTarget = _base;
            return;
        }


        // Check if there's an ally within attack range
        _attackTargets = Utils.findGameObjectsWithinRange(transform.position, attackRange, "Ally");
        if (_attackTargets.Count == 0) {
            // no attack target can be found
            _attackTarget = null;
            return;
        } else {
            // just pick the closest ally
            _attackTarget = Utils.findNearestGameObject(transform.position, _attackTargets);
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
                // updateNavAgent();
                return;
            }
        }

        // reach here only because all allies are rangers or out of vision
        _moveTarget = GameObject.FindGameObjectWithTag("Base");
        // updateNavAgent();
    }

    // private void updateNavAgent() {
    //     if (_moveTarget != _lastMoveTarget) {
    //         _lastMoveTarget = _moveTarget;
    //     }

    //     move();
    // }

    // Draw the vision range of the enemy
    protected override void OnDrawGizmosSelected() {
        base.OnDrawGizmosSelected();
        Utils.drawRange(transform, visionRange, Color.green);
    }

    
}
