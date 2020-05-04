using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for all enemies

public class Enemy : MonoBehaviour {
    public float attackRange;
    internal float _initialAttackRange;

    public float attackRate;
    internal float _attackCoolDown;
    internal float _initialAttackRate;

    public float attackDamage;
    internal float _initialAttackDamage;

    public float speed;
    internal float _initialSpeed;

    // nearest ally, then base
    internal GameObject _target;

    void Start()
    {
        _initialAttackDamage = attackDamage;
        _initialAttackRange = attackRange;
        _initialAttackRate = attackRate;
        _initialSpeed = speed;

        InvokeRepeating("updateTarget", 0f, 0.5f);
    }

    void Update()
    {
        move();
        attack();
    }

    // Attack target
    public virtual void attack() {
        if (!isWithinAttackRange())
            return;
        
        if (_attackCoolDown <= 0) {
            _target.GetComponent<Destroyable>().hit((int)attackDamage);
            _attackCoolDown = 1.0f/attackRate;
        }

        _attackCoolDown -= Time.deltaTime;
    }

    // Move toward the target until reached the attacking range
    public virtual void move() {
        // if no target, stall
        if (_target == null) {
            return;
        }

        Transform targetTransform = _target.transform;
        Vector3 targetPosition = new Vector3(targetTransform.position.x, transform.position.y, 
            targetTransform.position.z);

        Ray oppositeDirection = new Ray(targetPosition, transform.position - targetPosition);
        
        Vector3 destination = oppositeDirection.GetPoint(attackRange);

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, destination, step);
    }

    // Update the target of this enemy
    // Default alg is to find the nearest ally, then the base
    public virtual void updateTarget() {
        GameObject[] allies = GameObject.FindGameObjectsWithTag("Ally");
        if (allies.Length == 0) {
            // Use the base as its target
            _target = GameObject.FindGameObjectWithTag("Base");
        } else {
            // find the nearest ally and set it as target
            float nearestDistance = float.PositiveInfinity;
            foreach (GameObject ally in allies) {
                float distance = Vector3.Distance(transform.position, ally.transform.position);
                if (distance <= nearestDistance) {
                    nearestDistance = distance;
                    _target = ally;
                }
            }
        }
    }


    // Check if the target is within attack range
    public virtual bool isWithinAttackRange() {
        if (_target == null) {
            return false;
        }
        Transform targetTransform = _target.transform;
        Vector3 targetPos = new Vector3(targetTransform.position.x, transform.position.y, 
            targetTransform.position.z);

        return Vector3.Distance(transform.position, targetPos) <= attackRange; 
    }

    
}
