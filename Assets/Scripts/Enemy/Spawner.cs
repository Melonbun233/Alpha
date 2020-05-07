using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Enemy
{
    [Header("Spawn Rate")]
    public float spawnRate;
    private float _spawnCoolDown = 0f;

    [Header("Spawn Enemies")]
    public GameObject meleeEnemy;
    public GameObject rangeEnemy;


    public override void Start()
    {
        
    }

    public override void Update()
    {
        // Check if there's a base
        if (GameObject.FindGameObjectWithTag("Base") == null) {
            return;
        }

        // Simply spawn a range enemy at a spawning rate
        if (_spawnCoolDown <= 0f) {
            Vector3 spawnPosition = new Vector3(transform.position.x, 1, transform.position.y);
            Spawn(getNextSpawnEnemy(), transform.position, transform.rotation);
            _spawnCoolDown = 1f / spawnRate;
        }

        _spawnCoolDown -= Time.deltaTime;
    }

    // Spawner doesnt move nor attack
    public override void attack(){}
    public override void updateAttackTarget(){}
    public override void move(){}
    public override void updateMoveTarget(){}

    private void Spawn(GameObject enemy, Vector3 position, Quaternion rotation) {
        Instantiate(meleeEnemy, position, rotation);
    }

    private GameObject getNextSpawnEnemy() {
        // simply return a range enemy right now
        return meleeEnemy;
    } 
}
