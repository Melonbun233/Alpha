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

    [Header("Level Settings")]
    public float ManaRegen;
    public float MaxMana;
    public float StartingMana;
    public float mana;

    [Header("TeamList settings")]
    public float TeamOptionOffset;
    public Vector2 TeamOptionStartingPoint;

    [Header("Placement Settings")]
    public Vector3 wallOffset;
    public Vector3 valleyOffset;


    [Header("References. No need to change")]
    public Camera cam;
    public GameObject UI;

    private GameObject mapGeneratorObject;
    private MapGenerator mapGenerator;
    private TeamList teamList;
    private Placement placement;

    private Text ManaText;

    [Header("Game Status")]
    public Character character;
    public List<GameObject> spawns;
    public List<GameObject> enemyPrefabs;
    [SerializeField]
    public List<Wave> waves = new List<Wave>();
    public float SpawnCoolDown;
    public int count;
    public float preparationTime;

    private int times = 0;
    private bool initialized = false;

    public void SetUpLevel(int level, float ManaRegen, float MaxMana, float StartingMana, 
        List<AllyData> allydatas, int seed = -1)
    {
        this.rows = level / 5 + 3;
        this.colums = level / 5 + 2;
        this.ManaRegen = ManaRegen;
        this.MaxMana = MaxMana;
        this.StartingMana = StartingMana;
        teamList = new TeamList(allydatas);
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
        teamList = GetComponent<TeamList>();
        teamList.UI = UI;
        teamList.offset = TeamOptionOffset;
        teamList.startPoint = TeamOptionStartingPoint;
        teamList.team.Add(DefaultAllyData.TestRangerData);
        teamList.team.Add(DefaultAllyData.defaultBlockerData);

        // Setup placement
        placement = GetComponent<Placement>();  
        placement.wallOffset = wallOffset;
        placement.valleyOffset = valleyOffset;
        placement.Maincamera = cam;
        placement.UIE = UI;
        placement.ManaRegen = this.ManaRegen;
        placement.MaxMana = this.MaxMana;
        placement.startingMana = this.StartingMana;
        Placement.toPlace.Mana = this.StartingMana;

        ManaText = UI.GetComponentInChildren<Text>();
        initialized = true;

        waves.Add(WaveFormation.Melee3());
        //waves.Add(WaveFormation.MeleeRanger32());
        //waves.Add(WaveFormation.MeleeSuicidal32());
        //waves.Add(WaveFormation.RangerWave3());
        //waves.Add(WaveFormation.suicidalWave5());
        //waves.Add(WaveFormation.Boss_General());
    }

    public bool levelEnded() {
        return character.isDead();
    }

    void Awake()
    {
        GameObject mapGeneratorObject = GameObject.Find("MapGenerator");
        if (mapGeneratorObject == null) {
            Debug.LogError("There should be a MapGenerator game object in the level scene");
            return;
        }

        mapGenerator = mapGeneratorObject.GetComponent<MapGenerator>(); 

        if (autoGenerateMap) {
            generateLevel();
        }
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
        
        if (character.isDead())
        {
            UI.transform.Find("GO").gameObject.SetActive(true);
            Placement.toPlace.enabled = false;
        }

        mana = Placement.toPlace.Mana;
        if (Placement.toPlace.Mana <= MaxMana)
        {
            Placement.toPlace.Mana += ManaRegen * Time.deltaTime;
            int intMana = (int)Placement.toPlace.Mana;
            ManaText.text = intMana.ToString();
        }
        else
        {
            int intMana = (int)MaxMana;
            ManaText.text = intMana.ToString();
        }
    }
}
