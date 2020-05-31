using System;
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


    public static readonly string[] defaultModifyNames = {
        //"InnerRing",
        "Beam",
        //"Trail",
        "Particles"
    };

    public static readonly string[] defaultModifyNamesLevel4 = {
        // "SemiCircles",
        // "Circle",
        // "Ring",
        "Beam",
        "Particles",
    };

    protected override void Start() {
        base.Start();

        for (int i = 0; i < 3; i ++) {
            addLevel(new LevelConfig(defaultPaths[i], defaultModifyNames, 
            fireDefaultModifyAction));
        }

        addLevel(new LevelConfig(defaultPaths[3], defaultModifyNamesLevel4, fireDefaultModifyAction));
    }

    void fireDefaultModifyActionLevel4 (GameObject obj, int mainTypeLevel, 
        int subTypeLevel, Color color) {
            if (color == CustomColors.noviceMainColor) {
                return;
            }
            Color transparentColor = new Color(color.r, color.g, color.b, 80);
            ParticleSystem ps = obj.GetComponent<ParticleSystem>();
            if (ps != null) {
                ParticleSystem.MainModule main = ps.main;
                ParticleSystemRenderer renderer = obj.GetComponent<ParticleSystemRenderer>();

                renderer.material.SetFloat("_Emission", 0.1f);

                if (obj.name == "Beam") {
                    renderer.material.SetColor("_Color", transparentColor * 0.02f);
                    main.startColor = color;
                } else {
                    renderer.material.SetColor("_Color", color * 0.02f);
                    main.startColor = color;
                }
                
            }
        }

    void fireDefaultModifyAction (GameObject obj, int mainTypeLevel, 
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
                    renderer.material.SetColor("_Color", transparentColor * 0.02f);
                    main.startColor = color;
                } else {
                    renderer.material.SetColor("_Color", color * 0.02f);
                    main.startColor = color;
                }
                
            }
        }

}
