using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSetPhysic : ProjectileSet
{
    // default prefab paths under Resource/ directory
    public static readonly string[] defaultPaths = {
        "Prefabs/Projectile/Projectiles/physic_level1_projectile",
        "Prefabs/Projectile/Projectiles/physic_level2_projectile",
        "Prefabs/Projectile/Projectiles/physic_level3_projectile",
        "Prefabs/Projectile/Projectiles/physic_level4_projectile",
    };

    // public static readonly string[] defaultModifyNames = {
    //     //"InnerRing",
    //     "Beam",
    //     //"Trail",
    //     "Particles"
    // };

    public static readonly string[] modifyNamesLevel1 = {
        "MainBeam",
        // "GlowBeam",
        // "Smoke",
        "Particles",
    };

    protected override void Start() {
        base.Start();

        addLevel(new LevelConfig(defaultPaths[0], modifyNamesLevel1, defaultModifyAction));
        // addLevel(new LevelConfig(defaultPaths[1], defaultModifyNames, defaultModifyAction));
        // addLevel(new LevelConfig(defaultPaths[2], defaultModifyNames, defaultModifyAction));
        // addLevel(new LevelConfig(defaultPaths[3], defaultModifyNames, defaultModifyAction));
    }
}
