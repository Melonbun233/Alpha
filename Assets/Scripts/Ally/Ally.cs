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
    protected List<GameObject> _attackTargets;
    
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
        preAttack();
    }

    // Check if the ally can perform an attack on the attack target
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

    // Update the attack target
    // Default alg is to find the nearest enemy within range
    // Enemies not within range will not be set as target
    public virtual void updateAttackTarget() {
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
    public virtual void move() {}

    public virtual void updateMoveTarget() {
        move();
    }

    public virtual void OnDrawGizmosSelected() {
        Utils.drawRange(transform, attackRange, Color.red);
    }

}
