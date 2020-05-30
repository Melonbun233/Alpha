using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AllyProjectileController : MonoBehaviour
{
    AllyProjectile allyProjectilePhysic;
    AllyProjectile allyProjectileFire;
    AllyProjectile allyProjectileWater;
    AllyProjectile allyProjectileWind;
    AllyProjectile allyProjectileThunder;

    // Speed for different ally projectile main type
    public static readonly Dictionary<AllyType, float> allyProjectileSpeeds = 
        new Dictionary<AllyType, float>() {
            {AllyType.Ranger, 20f},
            {AllyType.Blocker, 10f},
            {AllyType.Fire, 5f},
            {AllyType.Water, 5f},
            {AllyType.Wind, 10f},
            {AllyType.Thunder, 20f},
            {AllyType.None, 10f},
        };

    // Start is called before the first frame update
    void Start()
    {
        allyProjectilePhysic = gameObject.AddComponent<AllyProjectilePhysic>() as AllyProjectile;
        allyProjectileFire = gameObject.AddComponent<AllyProjectileFire>() as AllyProjectile;
        allyProjectileWater = gameObject.AddComponent<AllyProjectileWater>() as AllyProjectile;
        allyProjectileWind = gameObject.AddComponent<AllyProjectileWind>() as AllyProjectile;
        allyProjectileThunder = gameObject.AddComponent<AllyProjectileThunder>() as AllyProjectile;
        //Time.timeScale = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Get the projectile object based on ally's stats
    // Both ranger and blocker main type use physic projectile
    public GameObject getAllyProjectile(Ally ally) {
        AllyType mainType = ally.getMainType();
        switch (mainType) {
            case AllyType.Ranger :
            case AllyType.Blocker :
                return allyProjectilePhysic.getProjectile(ally.getMainTypeLevel(), 
                    ally.getSubType(), ally.getSubTypeLevel());

            case AllyType.Fire :
                return allyProjectileFire.getProjectile(ally.getMainTypeLevel(), 
                    ally.getSubType(), ally.getSubTypeLevel());

            case AllyType.Water :
                return allyProjectileWater.getProjectile(ally.getMainTypeLevel(), 
                    ally.getSubType(), ally.getSubTypeLevel());

            case AllyType.Wind :
                return allyProjectileWind.getProjectile(ally.getMainTypeLevel(), 
                    ally.getSubType(), ally.getSubTypeLevel());
            
            case AllyType.Thunder :
                return allyProjectileThunder.getProjectile(ally.getMainTypeLevel(), 
                    ally.getSubType(), ally.getSubTypeLevel());

            default: 
                Debug.LogError("Ally Type : " + Enum.GetName(typeof(AllyType), mainType) + 
                    " doesn't contain a ally projectile implementation");
                return null;
        }
    }

    // Spawn an ally projectile for the attacker
    public GameObject spawnAllyProjectile(Vector3 position, Quaternion rotation,
        GameObject attacker, GameObject target) {
            Ally ally = attacker.GetComponent<Ally>();
            if (ally == null) {
                Debug.LogWarning("Non-ally object cannot shot ally projectile");
                return null;
            }

            GameObject obj = getAllyProjectile(ally);
            if (obj == null) {
                return null;
            }
            Projectile projectile = obj.GetComponent<Projectile>();
            
            projectile.setProjectile(position, rotation, attacker, target,
                allyProjectileSpeeds[ally.getMainType()]);

            return obj;
    }
}
