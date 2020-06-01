using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;


public class E_Ranger : Enemy
{
    public AnimationCurve curve;
    public Projectile projectile;
    public void setDefaultData()
    {
        EnemyData data = DefaultEnemyData.testRangerData;
        this.attackData = data.attackData;
        this.resistanceData = data.resistanceData;
        this.healthData = data.healthData;
        this.moveData = data.moveData;
        this.effectData = data.effectData;
        this.visionRange = data.visionRange;
    }

    protected override void Start()
    {
        setDefaultData();
        base.Start();
        
    }

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
        foreach(GameObject x in _attackTargetsWithinRange)
        {
            if (restAttackNumber == 0) { break; }
            

            if (x.GetComponent<Blocker>() != null)
            {
                if (!FollowAnimationCurve.ifHitWall(FollowAnimationCurve.CurveRayCast(transform.position, x.transform.position, curve, 5)))
                {
                    blockers.Add(x);
                    restAttackNumber--;
                }
            }
            else
            {
                    others.Add(x);
                    restAttackNumber--;
            }
        }

        if(blockers.Count != 0)
        {
            foreach(GameObject x in blockers)
            {
                _attackTargets.Add(x);
            }
            return;
        }
        
        foreach(GameObject x in others)
        {
            _attackTargets.Add(x);
        }

        if (Utils.isWithinRangeObstacle(transform.position, _base.transform.position, attackData.attackRange)) _attackTargets.Add(_base);


        /*
        // Check if the base is within attack range
        Utils.findGameObjectsWithinRange(_attackTargets, transform.position, attackData.attackRange, "Base");
        if (_attackTargets.Count != 0 && restAttackNumber > 0)
        {
            if (!FollowAnimationCurve.ifHitWall(FollowAnimationCurve.CurveRayCast(transform.position, _attackTargets[0].transform.position, curve, 5)))
            {
                //_attackTargets[_attackTargets.Count] = new GameObject(_base);
                _attackTargets.Add(_base);
                restAttackNumber--;
            }
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
                List<RaycastHit> hits = FollowAnimationCurve.CurveRayCast(transform.position, _attackTargetsWithinRange[i].transform.position, curve, 5);
                if (!FollowAnimationCurve.ifHitWall(hits) || FollowAnimationCurve.ifHitRanger(hits))
                {
                    _attackTargets.Add(_attackTargetsWithinRange[i]);
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }
        */
    }

}
