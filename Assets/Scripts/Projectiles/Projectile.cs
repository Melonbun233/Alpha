using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject target;
    public GameObject attacker;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) {
            return;
        }

        float step = speed * Time.deltaTime;
        Transform targetTransform = target.transform;

        transform.position = Vector3.MoveTowards(transform.position, 
            targetTransform.position, step);
        transform.rotation = Quaternion.RotateTowards(transform.rotation,
            targetTransform.rotation, step);
    }

    void OnCollisionEnter(Collision other) {
        string tag = other.collider.tag;
        if (tag == "valley") {
            kill();
            return;
        }

        if (tag != "Enemy") {
            return;
        }

        // deal damage to the enemy
        GameObject target = other.collider.gameObject;
        Unit attackUnit = attacker.GetComponent<Unit>();
        DamageData damage = attackUnit.attackData.attackDamage;
        target.GetComponent<Destroyable>().receiveDamage(damage, attacker);
        attackUnit.dealAoeDamage(target, damage);
        // apply on attack event
        attackUnit.OnAttack(target);

        kill();
    }

    public void kill() {
        Destroy(gameObject);
    }

    public static GameObject spawn(GameObject prefab, Vector3 position, Quaternion rotation, 
        GameObject attacker, GameObject target, float speed) {

            if (prefab.GetComponent<Projectile>() == null) {
                Debug.Log("Cannot spawn non-projectile object");
                return null;
            }

            GameObject projectiles = GameObject.Find("Projectiles");
            if (projectiles == null) {
                projectiles = new GameObject("Projectiles");
                // projectiles.transform.position = new Vector3(0, 0, 0);
                // projectiles.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            GameObject obj = Instantiate(prefab, position, rotation, projectiles.transform);
            Projectile projectile = obj.GetComponent<Projectile>();

            projectile.target = target;
            projectile.attacker = attacker;
            projectile.speed = speed;
            
            return obj;
    }


}
