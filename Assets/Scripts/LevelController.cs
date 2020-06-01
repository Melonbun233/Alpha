using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour
{
    public static LevelController levelCtr;

    [Header("Map Settings")]
    public GameObject levelGenerator;
    public int rows;
    public int colums;
    public int iterations;
    public int Levels;
    public int seed;

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


    private TeamList teamList;
    private placement Placement;
    private Map_Generator MapGenerator;
    private Text ManaText;

    [Header("Game Status")]
    public Character Base;
    public List<GameObject> spawns;
    public List<GameObject> enemyPrefabs;
    [SerializeField]
    public List<Wave> waves = new List<Wave>();
    public float SpawnCoolDown;
    public int count;
    public float preparationTime;

    private int times = 0;

    private void Awake()
    {
        levelCtr = this;
        GameObject LevelGenerator = Instantiate(levelGenerator, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
        teamList = LevelGenerator.GetComponent<TeamList>();
        Placement = LevelGenerator.GetComponent<placement>();
        MapGenerator = LevelGenerator.GetComponent<Map_Generator>();
        MapGenerator.seed = this.seed;

        teamList.UI = UI;
        MapGenerator.spawnNumber = (int)Mathf.Log(Levels, 2f) + 1;
        MapGenerator.goldNum = Random.Range(0, (int)Mathf.Log(Levels, 2f));

        teamList.offset = TeamOptionOffset;
        teamList.startPoint = TeamOptionStartingPoint;
        teamList.team.Add(DefaultAllyData.TestRangerData);
        teamList.team.Add(DefaultAllyData.defaultBlockerData);

        Placement.wallOffset = wallOffset;
        Placement.valleyOffset = valleyOffset;
        Placement.Maincamera = cam;
        Placement.UIE = UI;
        Placement.ManaRegen = this.ManaRegen;
        Placement.MaxMana = this.MaxMana;
        Placement.startingMana = this.StartingMana;

        ManaText = UI.GetComponentInChildren<Text>();

        waves.Add(WaveFormation.RangerSuicidal23());
        waves.Add(WaveFormation.RangerWave3());
    }

    void Start()
    {
        placement.toPlace.Mana = StartingMana;
        
    }

    // Update is called once per frame

    void Update()
    {
        if(times < 1)
        {
            spawns[0].GetComponent<Spawner>().waves = this.waves;
            times++;
        }
        
        if (Base.isDead())
        {
            UI.transform.Find("GO").gameObject.SetActive(true);
            placement.toPlace.enabled = false;
        }

        mana = placement.toPlace.Mana;
        if (placement.toPlace.Mana <= MaxMana)
        {
            placement.toPlace.Mana += ManaRegen * Time.deltaTime;
            int intMana = (int)placement.toPlace.Mana;
            ManaText.text = intMana.ToString();
        }
        else
        {
            int intMana = (int)MaxMana;
            ManaText.text = intMana.ToString();
        }
    }
}
