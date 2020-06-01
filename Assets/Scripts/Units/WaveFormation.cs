using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaveFormation : MonoBehaviour
{

    public static Wave suicidalWave5()
    {
        List<EnemyData> datas = new List<EnemyData>();
        for(int i=0; i < 5; i++)
        {
            datas.Add(DefaultEnemyData.defaultSuicidalData);
        }

        return new Wave(
            datas,
            2f,
            3f
          );
    }

    public static Wave RangerWave3()
    {
        List<EnemyData> datas = new List<EnemyData>();
        for(int i = 0; i < 3; i++)
        {
            datas.Add(DefaultEnemyData.defaultRangerData);
        }

        return new Wave(
            datas,
            1f,
            3f
          );
    }

    public static Wave RangerSuicidal23()
    {
        List<EnemyData> datas = new List<EnemyData>();
        datas.Add(DefaultEnemyData.defaultRangerData);
        datas.Add(DefaultEnemyData.defaultRangerData);
        datas.Add(DefaultEnemyData.defaultSuicidalData);
        datas.Add(DefaultEnemyData.defaultSuicidalData);
        datas.Add(DefaultEnemyData.defaultSuicidalData);

        return new Wave(
            datas,
            1.5f,
            4f
        );
    }
}
