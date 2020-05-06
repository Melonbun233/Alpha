using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Base class for all enemies

public class Enemy : Destroyable {
    public float attackRange;

    public float attackRate;
    protected float _attackCoolDown;

    public float attackDamage;


    // nearest ally, then base
    protected GameObject _attackTarget;
    protected GameObject _moveTarget;

    protected NavMeshAgent _navAgent;


    public override void Start()
    {
        base.Start();

        _navAgent = GetComponent<NavMeshAgent>();

        InvokeRepeating("updateAttackTarget", 0f, 0.5f);
        InvokeRepeating("updateMoveTarget", 0f, 0.5f);
    }

    public override void Update()
    {
        base.Update();
        attack();
    }

    // Attack target
    public virtual void attack() {
        if (!isWithinAttackRange())
            return;
        
        if (_attackCoolDown <= 0) {
            _attackTarget.GetComponent<Destroyable>().receiveDamage((int)attackDamage, gameObject);
            _attackCoolDown = 1.0f/attackRate;
        }

        _attackCoolDown -= Time.deltaTime;
    }

    // Update the attack target of this enemy
    // Default alg is to check if base is within attack range
    // If not, check whether any ally is within range
    // If not, set the attack target as null
    public virtual void updateAttackTarget() {
        
        GameObject _base = GameObject.FindGameObjectWithTag("Base");
        // Check if base is destroyed
        if (_base == null) {
            _attackTarget = null;
            return;
        }

        // Check if the base is within attack range
        if (Utils.horizontalDistance(transform, _base.transform) <= attackRange) {
            _attackTarget = _base;
            return;
        }

        // Check if there's an ally within attack range
        GameObject[] allies = GameObject.FindGameObjectsWithTag("Ally");
        if (allies.Length == 0) {
            // no attack target can be found
            _attackTarget = null;
            return;
        } else {
            // find the nearest ally that is within attack range and set it as target
            float nearestDistance = float.PositiveInfinity;
            foreach (GameObject ally in allies) {
                float distance = Utils.horizontalDistance(transform, ally.transform);

                // check if the ally is within attack range
                if (distance > attackRange) {
                    continue;
                }

                if (distance <= nearestDistance) {
                    nearestDistance = distance;
                    _attackTarget = ally;
                }
            }

            if (float.IsPositiveInfinity(nearestDistance)) {
                _attackTarget = null;
                return;
            }
        }
    }

    // Move toward the target until reached the attacking range
    public virtual void move() {
        // if no target, stall
        if (_moveTarget == null) {
            return;
        }

        Transform targetTransform = _moveTarget.transform;
        Vector3 targetPosition = new Vector3(targetTransform.position.x, transform.position.y, 
            targetTransform.position.z);

        Ray oppositeDirection = new Ray(targetPosition, transform.position - targetPosition);
        
        Vector3 destination = oppositeDirection.GetPoint(attackRange - 0.2f);

        _navAgent.destination = destination;
    }

    // Basically, just find the base
    public virtual void updateMoveTarget() {
        _moveTarget = GameObject.FindGameObjectWithTag("Base");
        move();
    }


    // Check if the target is within attack range
    public virtual bool isWithinAttackRange() {
        if (_attackTarget == null) {
            return false;
        }

        return Utils.horizontalDistance(transform, _attackTarget.transform) <= attackRange;
    }

    public virtual void OnDrawGizmosSelected() {
        Utils.drawAttackRange(transform, attackRange);
    }

    
}
