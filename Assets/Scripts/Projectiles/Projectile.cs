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
    public Vector3 muzzleOffset;

    public AudioClip shotSFX;
    public AudioClip hitSFX;
    public List<GameObject> trails;

    public bool useMuzzleVFX;
    public bool useHitVFX;

    protected bool collided;
    protected Rigidbody rb;

    protected Vector3 movePosition;
    protected Quaternion moveRotation;
    protected Vector3 targetCenter;

    protected Transform muzzleParent;
    protected Transform hitParent;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();    
        

        updateMoveTarget();

        GameObject muzzleParentGameObject = GameObject.Find("Muzzles");
        GameObject hitParentGameObject = GameObject.Find("Hits");

        if (muzzleParentGameObject == null) {
            muzzleParent = new GameObject("Muzzles").transform;
        } else {
            muzzleParent = muzzleParentGameObject.transform;
        }

        if (hitParentGameObject == null) {
            hitParent = new GameObject("Hits").transform;
        } else {
            hitParent = hitParentGameObject.transform;
        }

        // Muzzle and shot
        if (muzzlePrefab != null && useMuzzleVFX) {
            GameObject muzzleVFX  = Instantiate(muzzlePrefab, transform.position, 
                Quaternion.identity, muzzleParent);
            muzzleVFX.transform.forward = transform.forward + muzzleOffset;
            ParticleSystem ps = muzzleVFX.GetComponent<ParticleSystem>();
            if (ps != null) {
                Destroy(muzzleVFX, ps.main.duration);
            } else {
                ParticleSystem psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
				Destroy (muzzleVFX, psChild.main.duration);
            }
        }

        if (shotSFX != null && GetComponent<AudioSource>()) {
            GetComponent<AudioSource>().PlayOneShot(shotSFX);
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected virtual void FixedUpdate() {
        // Check if this projectile has been detroyed
        if (gameObject == null) {
            return;
        }

        // Check if the target has been destroyed,
        // If true, destroy this projectile until it is close enough
        // to the last target position
        if (target == null && 
            Vector3.Distance(movePosition, transform.position) <= 0.2f) {
            //Mathf.Abs(Quaternion.Dot(moveRotation, transform.rotation)) >= 0.8f) {
            Destroy(gameObject);
            return;
        }

        float step = speed * Time.deltaTime;

        if (target != null) {
            Destroyable d = target.GetComponent<Destroyable>();
            targetCenter = d.center;
            updateMoveTarget();
        }

        transform.position = Vector3.MoveTowards(transform.position, movePosition, step);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, moveRotation, step);
    }

    // Update the position and rotation this projectile is moving towards
    // By default movePosition is set to target's position, and moveRotation
    // is set to the rotation looking to the target
    protected virtual void updateMoveTarget() {
        if (target == null) {
            return;
        }
        moveRotation = Quaternion.LookRotation(target.transform.position);
        movePosition = target.transform.TransformPoint(targetCenter);
    }

    protected virtual void OnTriggerEnter(Collider collider) {
        // Already dealt projectile effect
        if (collided) {
            return;
        }

        if (!isValidCollision(collider)) {
            return;
        }

        if (isValidTarget(collider)) {
            onHitTarget(collider);
        }

        onHitVFX(collider);
    }

    // Check wheter we want to ignore this collision
    // return false if we want to ignore this, and the projectile
    // will continue move to the target
    // By default, ignore all walls and valley
    protected virtual bool isValidCollision(Collider collider) {
        if (collider.tag == "walls" || collider.tag == "valley") {
            return false;
        }

        return true;
    }

    // Check whether the collision can be treat as an attack to
    // the object associated with the collider
    // Return false if we want to simply destroy this projectile,
    // and doesn't deal damage/effect to the collider
    // By default use the target's type
    protected virtual bool isValidTarget(Collider collider) {
        if (target == null) {
            return false;
        }

        if (target.tag == collider.tag) {
            return true;
        }

        return false;
    }


    protected virtual void onHitTarget(Collider collider) {
        // deal damage to the enemy
        if (attacker == null) return;
        GameObject target = collider.gameObject;
        Unit attackUnit = attacker.GetComponent<Unit>();
        DamageData damage = attackUnit.attackData.attackDamage;
        target.GetComponent<Destroyable>().receiveDamage(damage, attacker);
        attackUnit.dealAoeDamage(target, damage);
        // apply on attack event
        attackUnit.OnAttack(target);
    }

    protected virtual void onHitVFX(Collider collider) {
        collided = true;

        // Play on hit sfx
        AudioSource audioSource = GetComponent<AudioSource>();
        if (hitSFX != null && audioSource != null) {
            audioSource.PlayOneShot(hitSFX);
        }

        // Destroy trails
        foreach (GameObject trail in trails) {
            if (trail == null) {
                continue;
            }
            ParticleSystem ps = trail.GetComponent<ParticleSystem>();
            if (ps != null) {
                ps.Stop();
                Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
            }
        }

        speed = 0;

        // On hit vfx
        // Guess the collision position
        Vector3 collidingPosition = collider.ClosestPointOnBounds(transform.position);
        Quaternion rotation;
        if (!target) {
            rotation = Quaternion.identity;
        } else {
            rotation = Quaternion.LookRotation(collidingPosition -
                target.transform.TransformPoint(targetCenter));
        }
        

        if (hitPrefab != null && useHitVFX) {
            GameObject hitVFX = Instantiate(hitPrefab, collidingPosition, rotation, hitParent);
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

    protected virtual IEnumerator destroyParticle(float waitTime) {
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

    static GameObject getProjectileParent() {
        GameObject projectileParent = GameObject.Find("Projectile Parent");
        if (projectileParent == null) {
            projectileParent = new GameObject("Projectile Parent");
        }

        return projectileParent;
    }


    public GameObject spawnProjectile(Vector3 position, Quaternion rotation, 
        GameObject attacker, GameObject target, float speed) {

            GameObject projectileParent = getProjectileParent();

            GameObject obj = Instantiate(gameObject, position, rotation, 
                projectileParent.transform);
            obj.transform.LookAt(target.transform.position);

            Projectile projectile = obj.GetComponent<Projectile>();
            projectile.target = target;
            projectile.attacker = attacker;
            projectile.speed = speed;
            
            return obj;
    }

    // reset an already spawned projectile
    public void setProjectile(Vector3 position, Quaternion rotation,
        GameObject attacker, GameObject target, float speed) {

            GameObject projectileParent = getProjectileParent();    
            Transform transform = gameObject.transform;

            transform.parent = projectileParent.transform;
            transform.LookAt(target.transform.position);

            transform.position = position;
            transform.rotation = rotation;

            this.target = target;
            this.attacker = attacker;
            this.speed = speed;
        }


}
