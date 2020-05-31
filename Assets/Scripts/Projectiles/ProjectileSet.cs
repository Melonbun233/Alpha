using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ProjectileSet : MonoBehaviour
{
    // Definition for each level configuration
    // NB: level is based => there's no level 0
    public class LevelConfig {
        // Default prefab for thie level of projectile
        // This is the prefab for the maintype
        // NB: this prefab should contain a Projectile component at the highest
        // hierarchy
        public string prefabPath;
        // Used to loop the default prefab's children that are needed
        // to be modified when we have a different subtype
        public string[] modifyNames;
        // The callback for each modify GameObject
        // GameObject: the gameobject with a particle system to modify
        // first int: mainTypeLevel
        // second int: subTypeLevel
        // Color: main color for the sub type
        public Action<GameObject, int, int, Color> modifyAction;
        public LevelConfig(string prefabPath, string[] modifyNames, 
            Action<GameObject, int, int, Color> modifyAction) {
                this.prefabPath = prefabPath;
                this.modifyAction = modifyAction;
                this.modifyNames = modifyNames;
            }
    }

    // To modify levelconfigs, use addLevel() or removeLevel()
    protected List<LevelConfig> _levelConfigs = new List<LevelConfig>();
    public List<LevelConfig> levelConfigs{get {return _levelConfigs;}}


    // Default action is used for prefabs using HDR colors
    protected Action<GameObject, int, int, Color> defaultModifyAction;
        

    // Used to store the latest prefab path
    // Only reload the prefab if these paths are different from the path
    // in the level configs
    protected List<string> _lastPrefabPaths = new List<string>();

    // Used to store the latest loaded prefabs
    protected List<GameObject> _lastPrefabs = new List<GameObject>();

    protected virtual void Start() {
        defaultModifyAction = new Action<GameObject, int, int, Color>
        (
            (GameObject obj, int mainTypeLevel, int subTypeLevel, Color color) => {
                ParticleSystem ps = obj.GetComponent<ParticleSystem>();
                if (ps != null) {
                    ParticleSystem.MainModule main = ps.main;
                    ParticleSystemRenderer renderer = 
                        transform.gameObject.GetComponent<ParticleSystemRenderer>();
                    
                    renderer.material.SetFloat("_Emission", 0.1f);
                    renderer.material.SetColor("_Color", color * 0.02f);
                    main.startColor = color;
                }
            }
        );
    }
    protected virtual void Update() {}

    // Add a new level
    public void addLevel(LevelConfig config) {
        _levelConfigs.Add(config);
        _lastPrefabPaths.Add("");
        _lastPrefabs.Add(null);

        loadPrefab(_levelConfigs.Count);
    }

    // Remove a certain level
    // Return true on successful remove, else return false
    public bool removeLevel(int level) {
        if (level <= 0 || level > _levelConfigs.Count) {
            Debug.LogWarning("Removing a invalid projectile level: " + level + "!");
            return false;
        }
        _levelConfigs.RemoveAt(level - 1);
        _lastPrefabPaths.RemoveAt(level - 1);
        _lastPrefabs.RemoveAt(level - 1);
        return true;
    }

    // Load a prefab by corresponding prefab path, and return the loaded prefab
    protected GameObject loadPrefab(int level) {
        // This prefab is already loaded, simply return it
        if (_lastPrefabPaths[level - 1] == _levelConfigs[level - 1].prefabPath) {
            return _lastPrefabs[level - 1];
        // Prefab path has changed since the last load
        // Reload the prefab using the new path
        } else {
            _lastPrefabPaths[level - 1] = _levelConfigs[level - 1].prefabPath;
            _lastPrefabs[level - 1] = Resources.Load<GameObject>(_lastPrefabPaths[level - 1]);
            return _lastPrefabs[level - 1];
        }
    }

    // Get the projectile gameobject that's been instantiated
    // We instantiate the prefab because there's no way to copy a gameobject :(
    // If the level is leq than 0, use the smallest level prefab
    // If the level is gre than highest level, use the highest level prefab
    public GameObject getProjectile(int level, Color subTypeColor, 
        int subTypeLevel) {
            if (_levelConfigs.Count == 0) {
                Debug.LogError("Empty level config!! Cannot instantiate any projectile");
                return null;
            }

            if (level <= 0) {
                Debug.LogWarning("Level too small, use level 1 prefab");
                level = 1;
            } else if (level > _levelConfigs.Count) {
                Debug.LogWarning("Level too high, use level " + _levelConfigs.Count + " prefab");
                level = _levelConfigs.Count;
            }

            GameObject prefab = loadPrefab(level);
            if (prefab == null) {
                Debug.LogError("Failed to load projectile level " + level + " prefab");
                return null;
            }
            GameObject obj = GameObject.Instantiate(prefab);

            modifyPrefab(level, subTypeColor, subTypeLevel, obj);
            return obj;
    } 


    // Modify the projectile based on the subtype level and color
    protected virtual void modifyPrefab(int level, Color subTypeColor, int subTypeLevel, 
        GameObject obj) {
            string[] modifyNames = _levelConfigs[level - 1].modifyNames;
            foreach(string name in modifyNames) {
                Transform transform = obj.transform.Find(name);
                if (transform == null) {
                    Debug.LogWarning("There's no child named: " + name);
                    continue;
                }

                _levelConfigs[level - 1].modifyAction(transform.gameObject, level, 
                    subTypeLevel, subTypeColor);
            }
        }
}
