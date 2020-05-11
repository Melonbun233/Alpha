using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suicidal : Enemy
{

    // // As a suicidal, ignore attack range and smash on it
    // public override void move() {
    //     if (_moveTarget == null) {
    //         _navAgent.destination = transform.position;
    //         return;
    //     }

    //     _navAgent.destination = _moveTarget.transform.position;
    // }
    
    public override void attack() {
        if (!isWithinAttackRange())
            return;
        
        _attackTarget.GetComponent<Destroyable>().receiveDamage((int)attackDamage, gameObject);

        // self destruction
        kill();
    }
}
