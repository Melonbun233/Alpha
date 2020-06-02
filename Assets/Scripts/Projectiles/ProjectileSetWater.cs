using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSetWater : ProjectileSet
{
    // default prefab paths under Resource/ directory
    public static readonly string[] defaultPaths = {
        "Prefabs/Projectile/Projectiles/water_level1_projectile",
        "Prefabs/Projectile/Projectiles/water_level2_projectile",
        "Prefabs/Projectile/Projectiles/water_level3_projectile",
        "Prefabs/Projectile/Projectiles/water_level4_projectile",
    };

    public static readonly string[] modifyNamesLevel1 = {
        "MainBeam",
        // "GlowBeam",
        // "Smoke",
        "Particles",
        "Trail",
    };

    public static readonly string[] modifyNamesLevel2 = {
        // "SemiCircleOuter",
        // "SemiCircleInner",
        // "Center",
        "Beam",
        "Particles"
    };

    public static readonly string[] modifyNamesLevel3 = {
        // "SemiCircleOuter",
        // "SemiCircleInner",
        // "Center",
        "Beam",
        "Particles"
    };

    public static readonly string[] modifyNamesLevel4 = {
        "Beam",
        // "BeamCenter",
        "Particles",
        // "Shockwaves",
        // "SemiCircles",
    };

    protected override void Start() {
        base.Start();

        addLevel(new LevelConfig(defaultPaths[0], modifyNamesLevel1, defaultModifyAction));
        addLevel(new LevelConfig(defaultPaths[1], modifyNamesLevel2, waterDefaultModifyAction));
        addLevel(new LevelConfig(defaultPaths[2], modifyNamesLevel3, waterDefaultModifyAction));
        addLevel(new LevelConfig(defaultPaths[3], modifyNamesLevel4, waterDefaultModifyAction));
    }

    void waterDefaultModifyAction (GameObject obj, int mainTypeLevel, 
        int subTypeLevel, Color color) {
            if (color == CustomColors.noviceMainColor) {
                return;
            }
            Color transparentColor = new Color(color.r, color.g, color.b, 60);
            ParticleSystem ps = obj.GetComponent<ParticleSystem>();
            if (ps != null) {
                ParticleSystem.MainModule main = ps.main;
                ParticleSystemRenderer renderer = obj.GetComponent<ParticleSystemRenderer>();

                renderer.material.SetFloat("_Emission", 0.15f);

                if (obj.name == "Beam") {
                    renderer.material.SetColor("_Color", transparentColor * 0.03f);
                    main.startColor = color;
                } else {
                    renderer.material.SetColor("_Color", color * 0.02f);
                    main.startColor = color;
                }
                
            }
        }

}
