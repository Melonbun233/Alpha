﻿using System.Collections.Generic;
using UnityEngine;


public class E_Ranger : Enemy
{
    public AnimationCurve curve;
    public Projectile projectile;
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

    protected override void Start()
    {
        setDefaultData();
        base.Start();

    }
}
