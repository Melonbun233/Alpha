using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave {
    // Enemy prefab spawned
    public GameObject prefab;
    
    // Spawn rate in one wave. Enemies are always spawned one by one
    public float spawnCoolDown;

    // Number of enemies spawned in this wave
    public int count;

    // time (seconds) before starting spawning
    public float preparationTime;
}

public class Spawner : Destroyable
{
    [Header("Wave Settings")]
    public List<Wave> waves;
    public int currentWave = 0;


    [Header("Spawn Settings")]
    public Vector3 localSpawnPosition;

    private float _preparationWaited = 0f;
    private float _spawnWaited = 0f;
    private int _spawnCount = 0;
    private bool _isSpawning = false;


    public override void Start()
    {
        
    }

    public override void Update()
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
                    spawn(wave.prefab);
                    _spawnCount ++;
                    _spawnWaited = 0f;
                } else {
                    _spawnWaited += Time.deltaTime;
                }
            }
        }


    }

    private void spawn(GameObject prefab) {
        Instantiate(prefab, transform.TransformPoint(localSpawnPosition), Quaternion.identity);
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
