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
                AllyType.Ranger, AllyType.None, 
                new AllyLevelData(5, 1, 0)
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
                AllyType.Blocker, AllyType.None,
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
                AllyType.None, AllyType.None,
                new AllyLevelData(5, 0, 0)
            );
        }
    }
}
