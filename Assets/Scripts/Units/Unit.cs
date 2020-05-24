using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class UnitData{
    public HealthData healthData;
    public AttackData attackData;
    public ResistanceData resistanceData;
    public MoveData moveData;
    public EffectData effectData;

    public UnitData(HealthData healthData, AttackData attackData, 
        ResistanceData resistanceData, MoveData moveData, EffectData effectData){
        this.healthData = healthData;
        this.attackData = attackData;
        this.resistanceData = resistanceData;
        this.moveData = moveData;
        this.effectData = effectData;
    }

    public static GameObject copyData(GameObject obj, UnitData data) {
        Unit unit = obj.GetComponent<Unit>();
        if (unit == null) {
            return null;
        }
        unit.healthData = HealthData.deepCopy(data.healthData);
        unit.attackData = AttackData.deepCopy(data.attackData);
        unit.resistanceData = ResistanceData.deepCopy(data.resistanceData);
        unit.moveData = MoveData.deepCopy(data.moveData);
        unit.effectData = EffectData.deepCopy(data.effectData);
        return obj;
    }
}

public abstract class Unit : Destroyable
{
    [Header("Attack Settings")]
    public AttackData attackData;
    protected float _attackCoolDown;
        
    public GameObject attackIndicator;

    public MoveData moveData;

    // All attack targets this unit will attack if the attack is cooled down
    // The size of this list should not large than attackNumber
    protected List<GameObject> _attackTargets = new List<GameObject>();
    

    protected GameObject _moveTarget;
    protected NavMeshAgent _navAgent;

    // Effects
    public EffectData effectData;

    // Effect Events & Delegates
    // Called on each attack
    public event Action<Unit, Unit> OnAttackEvent;
    // Called on each update
    public event Action<Unit, float> OnUpdateEvent;

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

        if (OnUpdateEvent != null) {
            OnUpdateEvent(this, Time.deltaTime);
        }
        
    }

    // Check if this unit has at least one attack target and attack is cooled down
    // If all true, perform attack on the attack target
    public virtual void preAttack() {
        if (_attackCoolDown <= 0 && _attackTargets.Count != 0) {
            attack();
        }

        _attackCoolDown -= Time.deltaTime;
    }

    // Perform the attack on the attack target
    // This method assumes that the unit has at least one attack target
    // and the attack is cooled down
    public virtual void attack() {
        foreach(GameObject target in _attackTargets) {
            target.GetComponent<Destroyable>().receiveDamage(attackData.attackDamage, gameObject);
            
            if (OnAttackEvent != null) {
                OnAttackEvent(this, target.GetComponent<Unit>());
            }   
            

            dealAoeDamage(target);
        }

        _attackCoolDown = attackData.attackCoolDown;
    }

    public virtual void dealAoeDamage(GameObject initialTarget){}

    // All unit class should implement this method
    // This method update the attack target of this unit every 0.5 second
    // Set the all attack targets for the next attack
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
    // Set move target to move
    public abstract void updateMoveTarget();

    // Draw attack range of this unit
    protected virtual void OnDrawGizmosSelected() {
         Utils.drawRange(transform, attackData.attackRange, Color.red);
    }

    protected virtual void updateNavAgentDestination(Vector3 position) {
        if (_navAgent != null && gameObject != null && !isDead()) {
            _navAgent.destination = position; 
        }
    }

    // Spawn a unit with the predefined data
    // Effect data inside the data will be applied to the object by default
    public static GameObject spawn(GameObject prefab, UnitData data, 
        Vector3 position, Quaternion rotation) {
        if (prefab.GetComponent<Unit>() == null) {
            Debug.Log("Cannot instantiate an non-unit object");
            return null;
        }

        GameObject obj = Instantiate(prefab, position, rotation);
        UnitData.copyData(obj, data);

        // Apply all effects in the data
        Unit unit = obj.GetComponent<Unit>();
        // Need first to clear all existing effects
        EffectData tmp = unit.effectData;
        unit.effectData = new EffectData();
        tmp.applyAllEffects(unit);

        return obj;
    }


}
