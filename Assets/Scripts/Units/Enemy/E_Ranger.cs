using System.Collections.Generic;
using UnityEngine;


public class E_Ranger : Enemy
{

    [Header("Ranger Settings")]
    public AnimationCurve curve;
    public GameObject projectile;
    public float projSpeed;
    public void setDefaultData()
    {
        EnemyData data = DefaultEnemyData.defaultRangerData;
        this.type = data.type;
        this.attackData = data.attackData;
        this.resistanceData = data.resistanceData;
        this.healthData = data.healthData;
        this.moveData = data.moveData;
        this.effectData = data.effectData;
        this.visionRange = data.visionRange;
    }

    public override void attack()
    {
        foreach(GameObject x in _attackTargets)
        {
            if (x == null) continue;

            //spawnArrow(x);
            GameObject proj = projectile.GetComponent<E_Projectile>().spawnEnemyProjectile(transform.position, Quaternion.identity, gameObject, x , projSpeed, curve);
            proj.transform.LookAt(x.transform.TransformPoint(x.GetComponent<Unit>().center));
        }
        _attackCoolDown = attackData.attackCoolDown;
        
    }

    protected override void Start()
    {
        setDefaultData();
        base.Start();
    }
}
