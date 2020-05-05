using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : Destroyable
{
    public override void Update()
    {
        // Check if self is daed
        if (isDead()) {
            kill();
        }
    }

}
