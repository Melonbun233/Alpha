using System.Collections;
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
                AllyType.Ranger, 1,
                AllyType.None, 0,
                new AllyLevelData(5, 1, 0)
            );
        }
    }

    public static AllyData defaultBlockerData 
    {
        get 
        {
            return new AllyData (
                new HealthData(50, 50, 0f, 0),
                new AttackData(1f, 1f, new DamageData(5, 0, 0, 0, 0), 1, 1f),
                new ResistanceData(10, 0, 0, 0, 0),
                new MoveData(0f),
                new EffectData(),
                AllyType.Blocker, 1,
                AllyType.None, 0,
                new AllyLevelData(5, 1, 0)
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
                AllyType.None, 0,
                AllyType.None, 0,
                new AllyLevelData(5, 0, 0)
            );
        }
    }
}
