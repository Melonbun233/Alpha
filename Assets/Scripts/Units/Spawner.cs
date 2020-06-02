using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave {
    // Enemy prefab spawned
    public List<GameObject> prefab;

    //Multiple Enemy combinations.
    public List<EnemyData> datas;
    
    // Spawn rate in one wave. Enemies are always spawned one by one
    public float spawnCoolDown;

    // Number of enemies spawned in this wave
    public int count;

    // time (seconds) before starting spawning
    public float preparationTime;



    public Wave(List<EnemyData> datas, float spawnCoolDown, float preparationTime)
    {
        this.datas = datas;
        this.spawnCoolDown = spawnCoolDown;
        this.count = datas.Count;
        this.preparationTime = preparationTime;
    }

    public Wave(List<GameObject> prefab, float spawnCoolDown, float preparationTime)
    {
        this.prefab = prefab;
        this.spawnCoolDown = spawnCoolDown;
        this.count = prefab.Count;
        this.preparationTime = preparationTime;
    }
}

public class Spawner : Destroyable
{
    [Tooltip("Prefab: Enemy prefab to be spawned\n" + 
        "Spawn Cool Down: Time between each enemy spawned in one wave\n" +
        "Count: Number of enemies spawned in one wave\n" +
        "Preparation Time: Time before starting spawning the first enemey of this wave")]
    [Header("Wave Settings")]
    public List<Wave> waves;
    public int currentWave = 0;


    [Header("Spawn Settings")]
    public Vector3 localSpawnPosition;
    public List<GameObject> prefabs;

    private float _preparationWaited = 0f;
    private float _spawnWaited = 0f;
    private int _spawnCount = 0;
    private bool _isSpawning = false;

    public GameObject fetchPrefab(EnemyData data)
    {
        switch (data.type)
        {
            case EnemyType.suicidal:
                return prefabs[0];
            case EnemyType.ranger:
                return prefabs[1];
            case EnemyType.melee:
                return prefabs[2];
            case EnemyType.boss_general:
                return prefabs[3];
        }
        return null;
    }

    protected override void Start()
    {
        
    }

    protected override void Update()
    {
        // Check if there's a base
        if (GameObject.FindGameObjectWithTag("Base") == null) {
            return;
        }

        // Check if we have spawned all waves
        if (spawnedAllWaves()) {
            return;
        }

        Wave wave = waves[currentWave];
        // Check whether we are at spwaning or preparing
        if (!_isSpawning) {
            // Preparation state
            if (_preparationWaited >= wave.preparationTime) {
                _isSpawning = true;
                _preparationWaited = 0f;
            } else {
                _preparationWaited += Time.deltaTime;
            }

        } else {
            // Spawning state
            if (_spawnCount >= wave.count) {
                // spawned all enemies, switch back to preparation state
                _spawnCount = 0;
                _spawnWaited = 0f;
                _isSpawning = false;
                currentWave ++;
            } else {
                // check spawn count down and spawn enemy
                if (_spawnWaited >= wave.spawnCoolDown) {

                    Enemy.spawn(fetchPrefab(wave.datas[_spawnCount]), wave.datas[_spawnCount], transform.TransformPoint(localSpawnPosition), Quaternion.identity);
                    _spawnCount ++;
                    _spawnWaited = 0f;
                } else {
                    _spawnWaited += Time.deltaTime;
                }
            }
        }


    }

    private void spawn(List<GameObject> prefab, int countIndex) {
        Instantiate(prefab[countIndex], transform.TransformPoint(localSpawnPosition), Quaternion.identity);
    }

    public bool spawnedAllWaves() {
        // Out of bountry indicating all waves have been spawned
        return currentWave == waves.Count;
    }

    // Draw spawn position with a red sphere
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.TransformPoint(localSpawnPosition), 0.5f);
    }
}
