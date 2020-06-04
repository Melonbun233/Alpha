using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum EnemyType
{
    ranger,
    suicidal,
    melee,
    boss_general,
    none
}

// Base class for all enemies
public class Enemy : Unit {

    public EnemyType type;
    [Header("Path Finding Settings")]
    // Used to find allies to attack
    public float visionRange;
    // [Header("Priority Settings")]
    // public int priority;


    //updates attack target every .5second. Blocker is first piority.
    public override void updateAttackTarget()
    {
        _attackTargets.Clear();

        GameObject _base = GameObject.FindGameObjectWithTag("Base");
        // Check if base is destroyed
        if (_base == null)
        {
            return;
        }

        List<GameObject> _attackTargetsWithinRange = new List<GameObject>();
        List<GameObject> blockers = new List<GameObject>();
        List<GameObject> others = new List<GameObject>();
        int restAttackNumber = attackData.attackNumber;
        Utils.findGameObjectsWithinRange(_attackTargetsWithinRange, transform.position, attackData.attackRange, "Ally");
        Utils.sortByDistance(_attackTargetsWithinRange, transform.position);
        foreach (GameObject x in _attackTargetsWithinRange)
        {
            if (restAttackNumber == 0) { break; }


            if (x.GetComponent<Blocker>() != null)
            {
                if (!FollowAnimationCurve.ifHitWall(FollowAnimationCurve.CurveRayCast(transform.position, x.transform.position, FollowAnimationCurve.DefaultCurveY, 5)))
                {
                    blockers.Add(x);
                    restAttackNumber--;
                }
            }
        }

        foreach (GameObject x in _attackTargetsWithinRange)
        {
            if (restAttackNumber == 0) { break; }
            others.Add(x);
            restAttackNumber--;
        }

        foreach (GameObject x in blockers)
        {
            _attackTargets.Add(x);
        }

        foreach (GameObject x in others)
        {
            _attackTargets.Add(x);
        }

        if (Utils.isWithinRangeObstacle(transform.position, _base.transform.position, attackData.attackRange) && restAttackNumber != 0) _attackTargets.Add(_base);
    }
    /*
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

        int restAttackNumber = attackData.attackNumber;

        // Check if the base is within attack range
        Utils.findGameObjectsWithinRange(_attackTargets, transform.position, attackData.attackRange, "Base");
        if (_attackTargets.Count != 0 && restAttackNumber > 0)
        {
            //_attackTargets[_attackTargets.Count] = _base;
            _attackTargets.Add(_base);
            restAttackNumber--;
        }

        // Check if there's an ally within attack range
        List<GameObject> _attackTargetsWithinRange = new List<GameObject>();
        Utils.findGameObjectsWithinRange(_attackTargetsWithinRange, transform.position,
             attackData.attackRange, "Ally");
        Utils.sortByDistance(_attackTargetsWithinRange, transform.position);

        for (int i = 0; i < restAttackNumber; i++)
        {
            if (i < _attackTargetsWithinRange.Count)
            {
                _attackTargets.Add(_attackTargetsWithinRange[i]);
            }
            else
            {
                break;
            }
        }
    }*/

    public override void dealAoeDamage(GameObject initialTarget, DamageData damage) {
        List<GameObject> nearbyEnemies = new List<GameObject>();
        Utils.findGameObjectsWithinRange(nearbyEnemies, initialTarget.transform.position,
            attackData.attackAoeRange, "Ally");

        foreach(GameObject nearbyEnemy in nearbyEnemies) {
            nearbyEnemy.GetComponent<Destroyable>().receiveDamage(damage, gameObject);
        }
    }

    // Move toward the target until reached the attacking range
    public override void move() {
        // game over
        if (_moveTarget == null) 
        {
            return;
        }
        Transform targetTransform = _moveTarget.transform;
        if (Utils.isWithinRangeObstacle(transform.position, targetTransform.position, attackData.attackRange))
        {
            if(FollowAnimationCurve.ifHitWall(FollowAnimationCurve.CurveRayCast(transform.position, targetTransform.position, FollowAnimationCurve.DefaultCurveY, 5)))
            {
                _navAgent.speed = moveData.moveSpeed;
                //_navAgent.enabled = true;
                updateNavAgentDestination(_moveTarget.transform.position);
            }
            else
            {
                
                Ray oppositeDirection = new Ray(targetTransform.position,
                transform.position - targetTransform.position);
                Vector3 destination = oppositeDirection.GetPoint(attackData.attackRange);
                updateNavAgentDestination(destination);
                _navAgent.speed = 0;
                //_navAgent.enabled = false;
            }
        }
        else
        {
            _navAgent.speed = moveData.moveSpeed;
            //_navAgent.enabled = true;
            updateNavAgentDestination(_moveTarget.transform.position);
        }


        // Vector3 targetPosition = new Vector3(targetTransform.position.x, transform.position.y, 
        //     targetTransform.position.z);
        /*
        Ray oppositeDirection = new Ray(targetTransform.position, 
        transform.position - targetTransform.position);
        
        Vector3 destination = oppositeDirection.GetPoint(attackData.attackRange);

        updateNavAgentDestination(destination);*/
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
                if (ally.GetComponent<Ally>().hasAllyType(AllyType.Blocker) && 
                    Utils.isWithinRangeObstacle(transform.position, ally.transform.position, visionRange)) {
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

    public static GameObject spawn(GameObject prefab, EnemyData data, 
        Vector3 position, Quaternion rotation) {
        if (prefab.GetComponent<Enemy>() == null) {
            Debug.Log("Cannot spawn an non-enemy object");
            return null;
        }

        GameObject obj = Instantiate(prefab, position, rotation);
        EnemyData.copyData(obj, data);

        // Apply all effects in the data
        Unit unit = obj.GetComponent<Unit>();
        // Need first to clear all existing effects
        EffectData tmp = unit.effectData;
        unit.effectData = new EffectData();
        tmp.applyAllEffects(unit);
        
        return obj;
    }

    
}
