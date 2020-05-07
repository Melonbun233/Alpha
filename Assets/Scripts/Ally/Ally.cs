using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AllyType {
    Ranger, // rangers can only be placed on walls and will not block enemies
    Blocker // blockers can only be placed on vallys and will block enemies
}
// Base class for all allies
public class Ally : Destroyable
{
    [Header("Ally Type")]
    public AllyType type;

    [Header("Attack Settings")]
    public float attackRange;

    public float attackRate;
    protected float _attackCoolDown;

    public float attackDamage;

    // nearest enemy
    protected GameObject _attackTarget;
    protected GameObject _moveTarget;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        InvokeRepeating("updateAttackTarget", 0f, 0.5f);
        InvokeRepeating("updateMoveTarget", 0f, 0.5f);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        move();
        preAttack();
    }

    // Check if the ally can perform an attack on the attack target
    // If possible, perform the attack action
    public virtual void preAttack() {
        if (!isWithinAttackRange())
            return;
        
        if (_attackCoolDown <= 0) {
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

    // Update the attack target
    // Default alg is to find the nearest enemy within range
    // Enemies not within range will not be set as target
    public virtual void updateAttackTarget() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) {
            // no enemy spawned yet
            _attackTarget = null;
            return;
        } else {
            // find the nearest enemy that is within range and set it as target
            float nearestDistance = float.PositiveInfinity;
            foreach (GameObject enemy in enemies) {
                float distance = Utils.horizontalDistance(transform, enemy.transform);
                if (distance > attackRange) {
                    continue;
                }
                if (distance <= nearestDistance) {
                    nearestDistance = distance;
                    _attackTarget = enemy;
                }
            }
            if (float.IsPositiveInfinity(nearestDistance)) {
                _attackTarget = null;
            }
        }
    }

    // Typically allies doesn't move
    public virtual void move() {}

    public virtual void updateMoveTarget() {}
    


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
