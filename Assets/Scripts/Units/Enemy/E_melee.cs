using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_melee : Enemy
{
    public void setDefaultData()
    {
        EnemyData data = DefaultEnemyData.defaultMeleeData;
        this.type = data.type;
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
}
