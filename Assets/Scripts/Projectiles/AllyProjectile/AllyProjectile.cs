using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AllyProjectile : MonoBehaviour
{
    public readonly int availableLevels = 3;

    public abstract string[] defaultPaths {get;}
    public abstract string[] prefabPaths {get; set;}

    public abstract string[] modifyNamesLevel1 {get; set;}
    public abstract Action<GameObject, int, int, Color> modifyLevel1Action {get; set;}

    public abstract string[] modifyNamesLevel2 {get; set;}
    public abstract Action<GameObject, int, int, Color> modifyLevel2Action {get; set;}

    public abstract string[] modifyNamesLevel3 {get; set;}
    public abstract Action<GameObject, int, int, Color> modifyLevel3Action {get; set;}


    protected Action<GameObject, int, int, Color> defaultModifyAction;   
        

    protected string[] _lastPrefabPaths = {"", "", ""};

    protected GameObject[] _lastPrefabs = {null, null, null};

    protected virtual void Start() {
        for (int i = 0; i < availableLevels; i ++) {
            loadPrefab(i + 1);
        }

        defaultModifyAction = new Action<GameObject, int, int, Color>(
            (GameObject obj, int mainTypeLevel, int subTypeLevel, Color color) => {

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

                    renderer.material.SetFloat("_Emission", 0.1f);
                    renderer.material.SetColor("_Color", color * 0.02f);
                    main.startColor = color;
                }
                
            }

        });
    }

    protected virtual string[] getModifyNames(int mainTypeLevel) {
        switch (mainTypeLevel) {
            case 1: 
                return modifyNamesLevel1;
            case 2:
                return modifyNamesLevel2;
            case 3:
                return modifyNamesLevel3;
            default:
                return null; 
        }
    }

    protected virtual void Update() {}

    public virtual GameObject getProjectile(int mainTypeLevel, AllyType subType, int subTypeLevel) {
        if (mainTypeLevel <= 0 || mainTypeLevel > availableLevels) {
            Debug.LogError("Projectile doesn't have level " + mainTypeLevel + " prefab");
            return null;
        }

        GameObject prefab = loadPrefab(mainTypeLevel);
        if (prefab == null) {
            Debug.LogError("Failed to load projectile level " + mainTypeLevel + " prefab");
            return null;
        }
        GameObject obj = GameObject.Instantiate(prefab);

        Action<GameObject, int, int, Color> modifyAction;
        switch (mainTypeLevel) {
            case 1: 
                modifyAction = modifyLevel1Action;
                break;
            case 2:
                modifyAction = modifyLevel2Action;
                break;
            case 3:
                modifyAction = modifyLevel3Action;
                break;
            default:
                return null; 
        }

        modifyPrefab(mainTypeLevel, subType, subTypeLevel, obj, modifyAction);
        return obj;
    } 

    protected GameObject loadPrefab(int mainTypeLevel) {
        if (_lastPrefabPaths[mainTypeLevel - 1] == prefabPaths[mainTypeLevel - 1]) {
            return _lastPrefabs[mainTypeLevel - 1];
        } else {
            _lastPrefabPaths[mainTypeLevel - 1] = prefabPaths[mainTypeLevel - 1];
            _lastPrefabs[mainTypeLevel - 1] = Resources.Load<GameObject>(_lastPrefabPaths[mainTypeLevel - 1]);
            return _lastPrefabs[mainTypeLevel - 1];
        }
    }

    protected void modifyPrefab(int mainTypeLevel, AllyType subType, int subTypeLevel, GameObject obj,
        Action<GameObject, int, int, Color> modifyAction) {
            switch (subType) {
                case AllyType.Blocker:
                case AllyType.Ranger:
                    modifyAction(obj, mainTypeLevel, subTypeLevel, CustomColors.physicalMainColor);
                    break;

                case AllyType.Fire:
                    modifyAction(obj, mainTypeLevel, subTypeLevel, CustomColors.fireMainColor);
                    break;

                case AllyType.Water:
                    modifyAction(obj, mainTypeLevel, subTypeLevel, CustomColors.waterMainColor);
                    break;

                case AllyType.Wind:
                    modifyAction(obj, mainTypeLevel, subTypeLevel, CustomColors.windMainColor);
                    break;

                case AllyType.Thunder:
                    modifyAction(obj, mainTypeLevel, subTypeLevel, CustomColors.thunderMainColor);
                    break;
            }
        }
}
