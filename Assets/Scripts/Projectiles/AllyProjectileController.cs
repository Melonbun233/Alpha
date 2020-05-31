using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AllyProjectileController : MonoBehaviour
{
    ProjectileSet projectileSetPhysic;
    ProjectileSet projectileSetFire;
    ProjectileSet projectileSetWater;
    ProjectileSet projectileSetWind;
    ProjectileSet projectileSetThunder;

    // Speed for different ally projectile main type
    public static readonly Dictionary<AllyType, float> allyProjectileSpeeds = 
        new Dictionary<AllyType, float>() {
            {AllyType.Ranger, 15f},
            {AllyType.Blocker, 5f},
            {AllyType.Fire, 5f},
            {AllyType.Water, 10f},
            {AllyType.Wind, 10f},
            {AllyType.Thunder, 15f},
            {AllyType.None, 10f},
        };

    public static readonly Dictionary<AllyType, Color> allyProjectileColors = 
        new Dictionary<AllyType, Color>() {
            {AllyType.Ranger, CustomColors.physicalMainColor},
            {AllyType.Blocker, CustomColors.physicalMainColor},
            {AllyType.Fire, CustomColors.fireMainColor},
            {AllyType.Water, CustomColors.waterMainColor},
            {AllyType.Wind, CustomColors.windMainColor},
            {AllyType.Thunder, CustomColors.thunderMainColor},
            {AllyType.None, CustomColors.noviceMainColor},
        };

    // Start is called before the first frame update
    void Start()
    {
        projectileSetPhysic = gameObject.AddComponent<ProjectileSetPhysic>() as ProjectileSet;
        projectileSetFire = gameObject.AddComponent<ProjectileSetFire>() as ProjectileSet;
        projectileSetWater = gameObject.AddComponent<ProjectileSetWater>() as ProjectileSet;
        projectileSetWind = gameObject.AddComponent<ProjectileSetWind>() as ProjectileSet;
        projectileSetThunder = gameObject.AddComponent<ProjectileSetThunder>() as ProjectileSet;
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
                return projectileSetPhysic.getProjectile(ally.getMainTypeLevel(), 
                    allyProjectileColors[ally.getSubType()], ally.getSubTypeLevel());

            case AllyType.Fire :
                return projectileSetFire.getProjectile(ally.getMainTypeLevel(), 
                    allyProjectileColors[ally.getSubType()], ally.getSubTypeLevel());

            case AllyType.Water :
                return projectileSetWater.getProjectile(ally.getMainTypeLevel(), 
                    allyProjectileColors[ally.getSubType()], ally.getSubTypeLevel());

            case AllyType.Wind :
                return projectileSetWind.getProjectile(ally.getMainTypeLevel(), 
                    allyProjectileColors[ally.getSubType()], ally.getSubTypeLevel());
            
            case AllyType.Thunder :
                return projectileSetThunder.getProjectile(ally.getMainTypeLevel(), 
                    allyProjectileColors[ally.getSubType()], ally.getSubTypeLevel());

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
