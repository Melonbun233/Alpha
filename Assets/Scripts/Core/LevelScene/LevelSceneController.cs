using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor.AI;
using Random = UnityEngine.Random;

public class LevelSceneController : MonoBehaviour
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
    private static LevelSceneController levelSceneController;
    private static bool cannotFindLevelSceneController;

    private Text manaText;

    private int times = 0;
    private bool initialized = false;

    void Awake()
    {
        mapGenerator = getMapGenerator();
        teamList = getTeamList();
        placementController = getPlacementController();
        levelSceneController = getLevelSceneController();
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

    public static bool isBossLevel(int level) {
        if (level > 0 && level % 5 == 0) {
            return true;
        }
        return false;
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

    public static LevelSceneController getLevelSceneController() {
        if (levelSceneController != null) {
            return levelSceneController;
        }
        
        if (cannotFindLevelSceneController) {
            return null;
        }
        
        GameObject levelSceneControllerObject = GameObject.Find("LevelSceneController");
        if (levelSceneControllerObject == null) {
            Debug.LogError("There should be a LevelSceneController game object in the " + 
                "level scene");
            cannotFindLevelSceneController = true;
            return null;
        }

        levelSceneController = levelSceneControllerObject.GetComponent<LevelSceneController>();
        if (levelSceneController == null) {
            Debug.LogError("There should be a LevelSceneController component in the " + 
                "LevelSceneController game object");
            cannotFindLevelSceneController = true;
            return null;
        }

        return levelSceneController;
    }

    public static PlacementController getPlacementController() {
        if (placementController != null) {
            return placementController;
        }

        if (cannotFindPlacementController) {
            return null;
        }

        if (levelSceneController == null) {
            levelSceneController = getLevelSceneController();
            if (levelSceneController == null) {
                cannotFindPlacementController = true;
                return null;
            }
        }

        placementController = levelSceneController.gameObject.GetComponent<PlacementController>();
        if (placementController == null) {
            Debug.LogError("There should be a PlacementController component in " +
                "the LevelSceneController game object");
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

        if (levelSceneController == null) {
            levelSceneController = getLevelSceneController();
            if (levelSceneController == null) {
                cannotFindTeamList = true;
                return null;
            }
        }

        teamList = levelSceneController.gameObject.GetComponent<TeamList>();
        if (teamList == null) {
            Debug.LogError("There should be a TeamList component in the " +
                "LevelSceneController game object");
            cannotFindTeamList = true;
            return null;
        }

        return teamList;
    }
}
