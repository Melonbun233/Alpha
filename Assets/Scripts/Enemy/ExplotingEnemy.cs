using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplotingEnemy : Enemy
{
    public override void Start() {
        base.Start();
    }

    public override void attack() {
        if (!isWithinAttackRange())
            return;
        
        _target.GetComponent<Destroyable>().receiveDamage((int)attackDamage);

        _attackCoolDown -= Time.deltaTime;
        
        // self destruction
        kill();
    }
}
