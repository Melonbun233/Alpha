
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSetThunder : ProjectileSet
{
    // default prefab paths under Resource/ directory
    public static readonly string[] defaultPaths = {
        "Prefabs/Projectile/Projectiles/thunder_level1_projectile",
        "Prefabs/Projectile/Projectiles/thunder_level2_projectile",
        "Prefabs/Projectile/Projectiles/thunder_level3_projectile",
        "Prefabs/Projectile/Projectiles/thunder_level4_projectile",
    };

    public static readonly string[] modifyNamesLevel1 = {
        "MainBeam",
        // "GlowBeam",
        // "Smoke",
        "Particles",
        // "Projectiles",
    };

    public static readonly string[] modifyNamesLevel2 = {
        "Sparks",
        "Beam",
        //"BeamWorld",
    };

    public static readonly string[] modifyNamesLevel3 = {
        "Sparks",
        "Beam",
        //"BeamWorld",
    };

    public static readonly string[] modifyNamesLevel4 = {
        "Sparks",
        "Beam",
        "CenterAura",
    };

    

    protected override void Start() {
        base.Start();

        addLevel(new LevelConfig(defaultPaths[0], modifyNamesLevel1, defaultModifyAction));
        addLevel(new LevelConfig(defaultPaths[1], modifyNamesLevel2, defaultModifyAction));
        addLevel(new LevelConfig(defaultPaths[2], modifyNamesLevel3, defaultModifyAction));
        addLevel(new LevelConfig(defaultPaths[3], modifyNamesLevel4, defaultModifyAction));
    }
}
