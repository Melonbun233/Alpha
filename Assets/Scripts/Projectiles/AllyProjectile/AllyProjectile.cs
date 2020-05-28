using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to spawn ally projectiles
// Projectiles will have color
public class AllyProjectile : Projectile
{   
    // Speed for different ally projectile main type
    public static readonly Dictionary<AllyType, float> allyProjectileSpeeds = 
        new Dictionary<AllyType, float>() {
            {AllyType.Ranger, 30f},
            {AllyType.Blocker, 20f},
            {AllyType.Fire, 10f},
            {AllyType.Water, 10f},
            {AllyType.Wind, 20f},
            {AllyType.Thunder, 30f},
            {AllyType.None, 20f},
        };

    public AllyType mainType;
    public AllyType subType;

    // level of the main type
    public int mainTypeLevel;
    public int subTypeLevel;

    // Set the vfx based on the types
    public void setVFX() {
        switch (mainType) {
            case AllyType.None:
                break;
            case AllyType.Blocker:
            case AllyType.Ranger:
                break;
            case AllyType.Fire:
                break;
            case AllyType.Water:
                break;
            case AllyType.Wind:
                break;
            case AllyType.Thunder:
                break;
        }
    }

    public GameObject spawnAllyProjectile(Vector3 position, Quaternion rotation,
        GameObject attacker, GameObject target) {
            Ally ally = attacker.GetComponent<Ally>();
            if (ally == null) {
                Debug.LogWarning("Non-ally object cannot shot ally projectile");
                return null;
            }

            AllyType mainType = ally.getMainType();
            AllyType subType = ally.getSubType();

            GameObject obj = base.spawnProjectile(position, rotation, attacker, target, 
                AllyProjectile.allyProjectileSpeeds[mainType]);

            AllyProjectile projectile = obj.GetComponent<AllyProjectile>();
            projectile.mainType = mainType;
            projectile.subType = subType;
            projectile.mainTypeLevel = ally.getMainTypeLevel();
            projectile.subTypeLevel = ally.getSubTypeLevel();
            projectile.setVFX();

            return obj;
    }
}
