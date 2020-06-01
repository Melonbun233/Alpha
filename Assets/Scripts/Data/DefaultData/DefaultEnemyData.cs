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
                EnemyType.suicidal,
                new MoveData(3f),
                new EffectData(),
                10f
            );
        }
    }

    public static EnemyData defaultRangerData
    {
        get
        {
            return new EnemyData(
                new HealthData(20, 20, 0f, 0),
                new AttackData(10f, 1, new DamageData(8, 0, 0, 0, 0), 1, 0),
                new ResistanceData(0, 0, 0, 0, 0),
                EnemyType.ranger,
                new MoveData(2f),
                new EffectData(),
                10f
                );
        }
    }

    public static EnemyData testRangerData
    {
        get
        {
            return new EnemyData(
                new HealthData(100, 100, 0f, 0),
                new AttackData(10f, 1, new DamageData(8, 0, 0, 0, 0), 1, 0),
                new ResistanceData(0, 0, 0, 0, 0),
                EnemyType.ranger,
                new MoveData(2f),
                new EffectData(),
                10f
                );
        }
    }
}
