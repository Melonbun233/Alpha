using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suicidal : Enemy
{
    public override void attack() {
        if (!isWithinAttackRange())
            return;
        
        _attackTarget.GetComponent<Destroyable>().receiveDamage((int)attackDamage);

        // self destruction
        kill();
    }
}
