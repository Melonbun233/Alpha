using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyProjectileFire : AllyProjectile
{
    // default prefab paths under Resource/ directory
    public override string[] defaultPaths {get;} = {
        "Prefabs/Projectile/Projectiles/fire_level1_projectile",
        "Prefabs/Projectile/Projectiles/fire_level2_projectile",
        "Prefabs/Projectile/Projectiles/fire_level3_projectile"
    };

    // Actual paths used for loading. This can be changed to use different
    // prefabs
    public override string[] prefabPaths {get; set;} = {
        "Prefabs/Projectile/Projectiles/fire_level1_projectile",
        "Prefabs/Projectile/Projectiles/fire_level2_projectile",
        "Prefabs/Projectile/Projectiles/fire_level3_projectile"
    };


    public override string[] modifyNamesLevel1 {get; set;} = {
        // "InnerRing",
        "Beam",
        "Trail",
        "Particles"
    };

    public override Action<GameObject, int, int, Color> modifyLevel1Action {get; set;}

    public override string[] modifyNamesLevel2 {get; set;} = {
        // "InnerRing",
        "Beam",
        "Trail",
        "Particles"
    };
    public override Action<GameObject, int, int, Color> modifyLevel2Action {get; set;}


    public override string[] modifyNamesLevel3 {get; set;} = {
        // "InnerRing",
        "Beam",
        "Trail",
        "Particles"
    };

    public override Action<GameObject, int, int, Color> modifyLevel3Action {get; set;}


    protected override void Start() {
        base.Start();

        modifyLevel1Action = new Action<GameObject, int, int, Color>(fireDefaultModifyAction);
        modifyLevel2Action = new Action<GameObject, int, int, Color>(fireDefaultModifyAction);
        modifyLevel3Action = new Action<GameObject, int, int, Color>(fireDefaultModifyAction);
    }

    void fireDefaultModifyAction (GameObject obj, int mainTypeLevel, 
        int subTypeLevel, Color color) {
            
            Color transparentColor = new Color(color.r, color.g, color.b, 50);
            string[] modifyNames = getModifyNames(mainTypeLevel);

            foreach (string gameObjectName in modifyNames) {
                Transform transform = obj.transform.Find(gameObjectName);
                if (transform == null) {
                    continue;
                }
                
                ParticleSystem ps = transform.gameObject.GetComponent<ParticleSystem>();
                if (ps != null) {
                    ParticleSystem.MainModule main = ps.main;
                    ParticleSystemRenderer renderer = transform.gameObject.GetComponent<ParticleSystemRenderer>();

                    renderer.material.SetFloat("_Emission", 0.15f);

                    if (gameObjectName == "Beam") {
                        renderer.material.SetColor("_Color", transparentColor * 0.02f);
                        main.startColor = transparentColor;
                    } else {
                        renderer.material.SetColor("_Color", color * 0.02f);
                        main.startColor = color;
                    }
                    
                }
                
            }

        }


    
}
