using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float spawnRate;
    private float _spawnCoolDown = 0f;
    public GameObject meleeEnemy;
    public GameObject rangeEnemy;


    void Start()
    {
        
    }

    void Update()
    {
        // Simply spawn a range enemy at a spawning rate
        if (_spawnCoolDown <= 0f) {
            Vector3 spawnPosition = new Vector3(transform.position.x, 1, transform.position.y);
            Spawn(getNextSpawnEnemy(), transform.position, transform.rotation);
            _spawnCoolDown = 1f / spawnRate;
        }

        _spawnCoolDown -= Time.deltaTime;
    }

    private void Spawn(GameObject enemy, Vector3 position, Quaternion rotation) {
        Instantiate(meleeEnemy, position, rotation);
    }

    private GameObject getNextSpawnEnemy() {
        // simply return a range enemy right now
        return meleeEnemy;
    } 
}
