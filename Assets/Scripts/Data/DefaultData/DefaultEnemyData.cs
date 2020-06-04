public static class DefaultEnemyData
{
    public static EnemyData defaultSuicidalData
    {
        get
        {
            return new EnemyData
            (
                new HealthData(20, 20, 0f, 0),
                new AttackData(2f, 1, new DamageData(0, 10, 0, 0, 0), 1, 1),
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
                new AttackData(8f, 1, new DamageData(8, 0, 0, 0, 0), 1, 0),
                new ResistanceData(0, 0, 0, 0, 0),
                EnemyType.ranger,
                new MoveData(2f),
                new EffectData(),
                10f
                );
        }
    }

    public static EnemyData defaultMeleeData
    {
        get
        {
            return new EnemyData(
                new HealthData(40,40,0f,0),
                new AttackData(2f,1,new DamageData(8,0,0,0,0), 1 ,0),
                new ResistanceData(0,0,0,0,0),
                EnemyType.melee,
                new MoveData(2.5f),
                new EffectData(),
                10f
                );
        }
    }

    public static EnemyData Boss_General
    {
        get
        {
            return new EnemyData(
                new HealthData(200, 200, 0f, 0),
                new AttackData(3f, 1, new DamageData(20, 0, 0, 0, 0), 1, 2),
                new ResistanceData(0, 0, 0, 0, 0),
                EnemyType.boss_general,
                new MoveData(2.5f),
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
