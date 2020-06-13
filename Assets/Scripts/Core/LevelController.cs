using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor.AI;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour
{
    [Header("Map Settings")]
    public int rows;
    public int colums;
    public int iterations;
    public int Levels;
    public int seed;
    public bool autoGenerateMap;

    [Header("References. No need to change")]
    public GameObject UI;

    [Header("Game Status")]
    public Character player;
    public List<GameObject> spawns;
    public List<GameObject> enemyPrefabs;
    [SerializeField]
    public List<Wave> waves = new List<Wave>();
    public float SpawnCoolDown;
    public int count;
    public float preparationTime;

    private static MapGenerator mapGenerator;
    private static bool cannotFindMapGenerator;
    private static TeamList teamList;
    private static bool cannotFindTeamList;
    private static PlacementController placementController;
    private static bool cannotFindPlacementController;
    private static LevelController levelController;
    private static bool cannotFindLevelController;

    private Text manaText;

    private int times = 0;
    private bool initialized = false;

    void Awake()
    {
        mapGenerator = getMapGenerator();
        teamList = getTeamList();
        placementController = getPlacementController();
        levelController = getLevelController();
    }

    void Start() {
        if (autoGenerateMap) {
            generateLevel();
        }
    }

    public void setupLevel(int level, Character player, 
        List<AllyData> hand, int seed = -1)
    {
        this.rows = level / 5 + 3;
        this.colums = level / 5 + 2;
        this.player = player;
        teamList.addTowerOptions(hand);
        this.seed = seed;
        generateLevel();
    }

    private void generateLevel()
    {
        // Setup map generator
        mapGenerator.seed = this.seed;
        mapGenerator.spawnNumber = (int)Mathf.Log(Levels, 2f) + 1;
        mapGenerator.goldNum = Random.Range(0, (int)Mathf.Log(Levels, 2f));
        mapGenerator.rows = this.rows;
        mapGenerator.columns = this.colums;
        mapGenerator.generate();

        // Setup team list UI
        teamList.addTowerOption(DefaultAllyData.TestRangerData);
        teamList.addTowerOption(DefaultAllyData.defaultBlockerData);

        // Setup placement
        placementController.player = player;
        manaText = UI.GetComponentInChildren<Text>();
        
        initialized = true;

        waves.Add(WaveFormation.Melee3());
        //waves.Add(WaveFormation.MeleeRanger32());
        //waves.Add(WaveFormation.MeleeSuicidal32());
        //waves.Add(WaveFormation.RangerWave3());
        //waves.Add(WaveFormation.suicidalWave5());
        //waves.Add(WaveFormation.Boss_General());
    }

    public bool levelEnded() {
        return player.isDead();
    }


    // Update is called once per frame

    void Update()
    {
        if (!initialized) return;

        if(times < 1)
        {
            foreach(GameObject x in spawns)
            {
                x.GetComponent<Spawner>().waves = this.waves;
            }
            times++;
        }
        
        if (levelEnded())
        {
            UI.transform.Find("GameOverText").gameObject.SetActive(true);
            placementController.enabled = false;
        }

        
    }

    public void switchCameraAngle()
    {
        Camera.main.GetComponent<CameraController>().angleSwtich();
    }

    public static MapGenerator getMapGenerator() {
        if (mapGenerator != null) {
            return mapGenerator;
        }

        if (cannotFindMapGenerator) {
            return null;
        }

        GameObject mapGeneratorObject = GameObject.Find("MapGenerator");
        if (mapGeneratorObject == null) {
            Debug.LogError("There should be a MapGenerator game object in the level scene");
            cannotFindMapGenerator = true;
            return null;
        }

        mapGenerator = mapGeneratorObject.GetComponent<MapGenerator>(); 
        if (mapGenerator == null) {
            Debug.LogError("There should be a MapGenerator component in the " + 
                "MapGenerator gameobject");
            cannotFindMapGenerator = true;
            return null;
        }

        return mapGenerator;
    }

    public static LevelController getLevelController() {
        if (levelController != null) {
            return levelController;
        }
        
        if (cannotFindLevelController) {
            return null;
        }
        
        GameObject levelControllerObject = GameObject.Find("LevelController");
        if (levelControllerObject == null) {
            Debug.LogError("There should be a LevelController game object in the " + 
                "level scene");
            cannotFindLevelController = true;
            return null;
        }

        levelController = levelControllerObject.GetComponent<LevelController>();
        if (levelController == null) {
            Debug.LogError("There should be a LevelController component in the " + 
                "LevelController game object");
            cannotFindLevelController = true;
            return null;
        }

        return levelController;
    }

    public static PlacementController getPlacementController() {
        if (placementController != null) {
            return placementController;
        }

        if (cannotFindPlacementController) {
            return null;
        }

        if (levelController == null) {
            levelController = getLevelController();
            if (levelController == null) {
                cannotFindPlacementController = true;
                return null;
            }
        }

        placementController = levelController.gameObject.GetComponent<PlacementController>();
        if (placementController == null) {
            Debug.LogError("There should be a PlacementController component in " +
                "the LevelController game object");
            cannotFindPlacementController = true;
            return null;
        }

        return placementController;
    }

    public static TeamList getTeamList() {
        if (teamList != null) {
            return teamList;
        }

        if (cannotFindTeamList) {
            return null;
        }

        if (levelController == null) {
            levelController = getLevelController();
            if (levelController == null) {
                cannotFindTeamList = true;
                return null;
            }
        }

        teamList = levelController.gameObject.GetComponent<TeamList>();
        if (teamList == null) {
            Debug.LogError("There should be a TeamList component in the " +
                "LevelController game object");
            cannotFindTeamList = true;
            return null;
        }

        return teamList;
    }
}
