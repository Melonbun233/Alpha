using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suicidal : Enemy
{
    public override void attack() {
        
        base.attack();

        // self destruction
        kill();
    }
}
