﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DefaultAllyData
{
    public static AllyData defaultRangerData 
    {
        get 
        {
            return new AllyData(
                new HealthData(20, 20, 0f, 0),
                new AttackData(10f, 1f, new DamageData(10, 0, 0, 0, 0), 1, 0f),
                new ResistanceData(0, 0, 0, 0, 0),
                new MoveData(0f),
                new EffectData(),
                AllyType.Ranger, AllyType.None, 
                new AllyLevelData(5, 1, 0, 3f)
            );
        }
    }

    public static AllyData defaultBlockerData 
    {
        get 
        {
            return new AllyData (
                new HealthData(200, 200, 0f, 0),
                new AttackData(1f, 1f, new DamageData(5, 0, 0, 0, 0), 1, 1f),
                new ResistanceData(10, 0, 0, 0, 0),
                new MoveData(0f),
                new EffectData(),
                AllyType.Blocker, AllyType.None,
                new AllyLevelData(5, 1, 0, 5f)
            );
        }
    }

    // Blank ally
    public static AllyData defaultBlankData
    {
        get
        {
            return new AllyData (
                new HealthData(10, 10, 0f, 0),
                new AttackData(1f, 1f, new DamageData(5, 0, 0, 0, 0), 1, 0f),
                new ResistanceData(0, 0, 0, 0, 0),
                new MoveData(0f),
                new EffectData(),
                AllyType.None, AllyType.None,
                new AllyLevelData(5, 0, 0, 3f)
            );
        }
    }
}
