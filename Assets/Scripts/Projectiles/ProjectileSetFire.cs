﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSetFire : ProjectileSet
{
    // default prefab paths under Resource/ directory
    public static readonly string[] defaultPaths = {
        "Prefabs/Projectile/Projectiles/fire_level1_projectile",
        "Prefabs/Projectile/Projectiles/fire_level2_projectile",
        "Prefabs/Projectile/Projectiles/fire_level3_projectile",
        "Prefabs/Projectile/Projectiles/fire_level4_projectile",
    };

    public static readonly string[] modifyNamesLevel1 = {
        "MainBeam",
        "Particles",
        "Trail",
    };

    public static readonly string[] modifyNamesLevel2 = {
        "Beam",
        "Trail",
        "Particles"
    };

    public static readonly string[] modifyNamesLevel3 = {
        "Beam",
        "Trail",
        "Particles"
    };

    public static readonly string[] modifyNamesLevel4 = {
        "Beam",
        "Trail",
        "Particles",
    };

    protected override void Start() {
        base.Start();

        addLevel(new LevelConfig(defaultPaths[0], modifyNamesLevel1, defaultModifyAction));
        addLevel(new LevelConfig(defaultPaths[1], modifyNamesLevel2, fireDefaultModifyAction));
        addLevel(new LevelConfig(defaultPaths[2], modifyNamesLevel3, fireDefaultModifyAction));
        addLevel(new LevelConfig(defaultPaths[3], modifyNamesLevel4, fireDefaultModifyAction));
    }

    void fireDefaultModifyAction (GameObject obj, int mainTypeLevel, 
        int subTypeLevel, Color color) {
            if (color == CustomColors.noviceMainColor) {
                return;
            }
            Color transparentColor = new Color(color.r, color.g, color.b, 60);

            // Particles systems
            ParticleSystem ps = obj.GetComponent<ParticleSystem>();
            if (ps != null) {
                
                ParticleSystem.MainModule main = ps.main;
                ParticleSystemRenderer renderer = obj.GetComponent<ParticleSystemRenderer>();

                renderer.material.SetFloat("_Emission", 0.15f);

                if (obj.name == "Beam") {
                    renderer.material.SetColor("_Color", transparentColor * 0.02f);
                    main.startColor = color;
                } else {
                    renderer.material.SetColor("_Color", color * 0.02f);
                    main.startColor = color;
                }

                if (obj.name == "Particles") {
                    renderer.material.SetFloat("_Emission", 0.7f);
                    renderer.material.SetColor("_Color", color * 0.04f);
                }
                
            }

            // Trail
            TrailRenderer tr = obj.GetComponent<TrailRenderer>();
            if (tr != null) {
                tr.startColor = color;
                tr.material.SetFloat("_Emission", 0.1f);
                tr.material.SetColor("_Color", color * 0.02f);
            }
        }

}
