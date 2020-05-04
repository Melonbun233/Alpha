using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplotingEnemy : Enemy
{
    public override void attack() {
        if (!isWithinAttackRange())
            return;
        
        if (_attackCoolDown <= 0) {
            _target.GetComponent<Destroyable>().hit((int)attackDamage);
            _attackCoolDown = 1.0f/attackRate;
        }

        _attackCoolDown -= Time.deltaTime;
        // self destruction
        Destroyable destroyable = GetComponent<Destroyable>();
        destroyable.hit(destroyable.health);
    }
}
