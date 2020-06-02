using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaveFormation : MonoBehaviour
{

    public static Wave Melee3()
    {
        List<EnemyData> datas = new List<EnemyData>();
        for(int i = 0; i < 3; i++)
        {
            datas.Add(DefaultEnemyData.defaultMeleeData);
        }

        return new Wave(datas, 1f, 5f);
    }

    public static Wave RangerWave3()
    {
        List<EnemyData> datas = new List<EnemyData>();
        for (int i = 0; i < 3; i++)
        {
            datas.Add(DefaultEnemyData.defaultRangerData);
        }

        return new Wave(datas, 1f, 5f);
    }

    public static Wave MeleeRanger32()
    {
        List<EnemyData> datas = new List<EnemyData>();
        datas.Add(DefaultEnemyData.defaultMeleeData);
        datas.Add(DefaultEnemyData.defaultMeleeData);
        datas.Add(DefaultEnemyData.defaultMeleeData);
        datas.Add(DefaultEnemyData.defaultRangerData);
        datas.Add(DefaultEnemyData.defaultRangerData);

        return new Wave(datas, 1f, 7f);
    }

    public static Wave Boss_General()
    {
        List<EnemyData> datas = new List<EnemyData>();
        datas.Add(DefaultEnemyData.Boss_General);

        return new Wave(datas, 1f, 15f);
    }

    public static Wave suicidalWave5()
    {
        List<EnemyData> datas = new List<EnemyData>();
        for(int i=0; i < 5; i++)
        {
            datas.Add(DefaultEnemyData.defaultSuicidalData);
        }

        return new Wave(datas, 2f, 5f);
    }



    public static Wave MeleeSuicidal32()
    {
        List<EnemyData> datas = new List<EnemyData>();
        datas.Add(DefaultEnemyData.defaultMeleeData);
        datas.Add(DefaultEnemyData.defaultMeleeData);
        datas.Add(DefaultEnemyData.defaultMeleeData);
        datas.Add(DefaultEnemyData.defaultSuicidalData);
        datas.Add(DefaultEnemyData.defaultSuicidalData);

        return new Wave(datas,1.5f, 6f);
    }
}
