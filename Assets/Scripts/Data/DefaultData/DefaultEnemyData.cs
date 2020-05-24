using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DefaultEnemyData
{
    public static EnemyData defaultSuicidalData
    {
        get 
        {
            return new EnemyData
            (
                new HealthData(20, 20, 0f, 0),
                new AttackData(1, 1, new DamageData(0, 10, 0, 0, 0), 1, 1),
                new ResistanceData(0, 10, 0, 0, 0),
                new MoveData(2f),
                new EffectData(),
                10f
            );
        }
    }
}
