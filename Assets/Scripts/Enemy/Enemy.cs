using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Base class for all enemies
public class Enemy : Destroyable {

    [Header("Attack Settings")]
    public float attackRange;

    public float attackRate;
    protected float _attackCoolDown;

    public float attackDamage;

    [Header("Path Finding Settings")]
    // Used to find allies to attack
    public float visionRange;

    // [Header("Priority Settings")]
    // public int priority;


    // nearest ally, then base
    protected GameObject _attackTarget;
    // used to store possible targets (within attack range)
    protected List<GameObject> _attackTargets;

    protected GameObject _lastMoveTarget;

    protected GameObject _moveTarget;
    protected Vector3 _destination;

    protected NavMeshAgent _navAgent;

    protected bool _baseDestroyed = false;

    


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
        preAttack();
    }

    // Check if the enemy can perform an attack on the attack target
    // If possible, perform the attack action
    public virtual void preAttack() {

        if (_attackCoolDown <= 0 && _attackTarget != null) {
            attack();
        }

        _attackCoolDown -= Time.deltaTime;
    }
    
    public virtual void attack() {
        if (_attackTarget == null) {
            return;
        }
        _attackTarget.GetComponent<Destroyable>().receiveDamage((int)attackDamage, gameObject);
        _attackCoolDown = 1.0f/attackRate;
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
    public virtual void move() {
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

        _navAgent.destination = destination;
    }

    // Temporarily find blockers within vision range as move target
    // If no target can be found, search for the base
    // If base is destroyed, enemy will stall
    // Later we might replace the alg to a priority queue
    public virtual void updateMoveTarget() {
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
                updateNavAgent();
                return;
            }
        }

        // reach here only because all allies are rangers or out of vision
        _moveTarget = GameObject.FindGameObjectWithTag("Base");
        updateNavAgent();
    }

    private void updateNavAgent() {
        if (_moveTarget != _lastMoveTarget) {
            _lastMoveTarget = _moveTarget;
        }

        move();
    }

    public virtual void OnDrawGizmosSelected() {
        Utils.drawRange(transform, attackRange, Color.red);
        Utils.drawRange(transform, visionRange, Color.green);
    }

    
}
