using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Unit : Destroyable
{
    [Header("Mana Settings")]
    public int mana;
    public int maxMana;
    [Header("Attack Settings")]
    public float attackRange;

    public float attackRate;
    protected float _attackCoolDown;

    public float attackDamage;


    // Attack target this unit will try to attack
    protected GameObject _attackTarget;
    // used to store possible targets (within attack range)
    protected List<GameObject> _attackTargets;

    protected GameObject _moveTarget;
    protected NavMeshAgent _navAgent;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        InvokeRepeating("updateAttackTarget", 0f, 0.5f);
        InvokeRepeating("updateMoveTarget", 0f, 0.5f);

        _navAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (gameObject.activeSelf) {
            preAttack();
            move();
        }
        
    }

    // Check if this unit has at least one attack target and attack is cooled down
    // If all true, perform attack on the attack target
    public virtual void preAttack() {
        if (_attackCoolDown <= 0 && _attackTarget != null) {
            attack();
        }

        _attackCoolDown -= Time.deltaTime;
    }

    // Perform the attack on the attack target
    // This method assumes that the unit has at least one attack target
    // and the attack is cooled down
    public virtual void attack() {
        if (_attackTarget == null) {
            return;
        }
        _attackTarget.GetComponent<Destroyable>().receiveDamage((int)attackDamage, gameObject);
        _attackCoolDown = 1.0f/attackRate;
    }

    // All unit class should implement this method
    // This method update the attack target of this unit every 0.5 second
    public abstract void updateAttackTarget();

    // Try to move by setting the destination of navAgent as the move target
    // This method is called every frame in Update() function
    // If no Unity NavMeshAgent is attached to the unit, no movement will be done
    public virtual void move() {
        if (_moveTarget == null || _navAgent == null) {
            return;
        }

        updateNavAgentDestination(_moveTarget.transform.position);
    }


    // All chracter classes should implement this method
    // This method update the move target of the unit every 0.5 second
    public abstract void updateMoveTarget();

    // Draw attack range of this unit
    protected virtual void OnDrawGizmosSelected() {
         Utils.drawRange(transform, attackRange, Color.red);
    }

    protected virtual void updateNavAgentDestination(Vector3 position) {
        if (_navAgent != null && gameObject != null && !isDead()) {
            _navAgent.destination = position; 
        }
    }


}
