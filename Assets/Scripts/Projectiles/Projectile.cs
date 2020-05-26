using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject target;
    public GameObject attacker;
    public float speed;

    public GameObject muzzlePrefab;
    public GameObject hitPrefab;

    public AudioClip shotSFX;
    public AudioClip hitSFX;
    public List<GameObject> trails;

    private bool collided;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
        if (target == null) {
            Destroy(gameObject);
            return;
        }

        float step = speed * Time.deltaTime;
        Transform targetTransform = target.transform;

        transform.position = Vector3.MoveTowards(transform.position, 
            targetTransform.position, step);
        
        Quaternion rotation = Quaternion.LookRotation(targetTransform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, step);
    }

    void OnTriggerEnter(Collider collider) {
        if (collided) {
            return;
        }

        string tag = collider.tag;

        // if (tag == "walls" || tag == "valley") {
        //     OnHitVFX(collision);
        //     return;
        // }

        if (tag != "Enemy") {    
            return;
        }


        // deal damage to the enemy
        GameObject target = collider.gameObject;
        Unit attackUnit = attacker.GetComponent<Unit>();
        DamageData damage = attackUnit.attackData.attackDamage;
        target.GetComponent<Destroyable>().receiveDamage(damage, attacker);
        attackUnit.dealAoeDamage(target, damage);
        // apply on attack event
        attackUnit.OnAttack(target);

        // Play on hit vfx
        OnHitVFX(collider);
    }

    private void OnHitVFX(Collider collider) {
        collided = true;

        // Play on hit sfx
        AudioSource audioSource = GetComponent<AudioSource>();
        if (hitSFX != null && audioSource != null) {
            audioSource.PlayOneShot(hitSFX);
        }

        // Destroy trails
        foreach (GameObject trail in trails) {
            ParticleSystem ps = trail.GetComponent<ParticleSystem>();
            if (ps != null) {
                ps.Stop();
                Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
            }
        }

        speed = 0;

        // On hit vfx
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, 
            transform.position - collider.transform.position);
        Vector3 position = transform.position;

        if (hitPrefab != null) {
            GameObject hitVFX = Instantiate(hitPrefab, position, rotation);
            ParticleSystem ps = hitVFX.GetComponent<ParticleSystem>();

            if (ps == null) {
                // particle systems are in the child
                var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitVFX, psChild.main.duration);
            } else {
                Destroy(hitVFX, ps.main.duration);
            }
        }

        StartCoroutine(destroyParticle(0f));
    }

    private IEnumerator destroyParticle(float waitTime) {
        if (transform.childCount > 0 && waitTime != 0) {
			List<Transform> tList = new List<Transform> ();

			foreach (Transform t in transform.GetChild(0).transform) {
				tList.Add (t);
			}		

			while (transform.GetChild(0).localScale.x > 0) {
				yield return new WaitForSeconds (0.01f);
				transform.GetChild(0).localScale -= new Vector3 (0.1f, 0.1f, 0.1f);
				for (int i = 0; i < tList.Count; i++) {
					tList[i].localScale -= new Vector3 (0.1f, 0.1f, 0.1f);
				}
			}
		}
		
		yield return new WaitForSeconds (waitTime);
		Destroy (gameObject);
    }

    public static GameObject spawn(GameObject vfx, Vector3 position, Quaternion rotation, 
        GameObject attacker, GameObject target, float speed) {

            if (vfx.GetComponent<Projectile>() == null) {
                Debug.Log("Cannot spawn non-projectile object");
                return null;
            }

            GameObject projectiles = GameObject.Find("Projectiles");
            if (projectiles == null) {
                projectiles = new GameObject("Projectiles");
            }
            GameObject obj = Instantiate(vfx, position, rotation, projectiles.transform);
            obj.transform.LookAt(target.transform.position);

            Projectile projectile = obj.GetComponent<Projectile>();
            projectile.target = target;
            projectile.attacker = attacker;
            projectile.speed = speed;
            
            return obj;
    }


}
