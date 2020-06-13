using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEditor.AI;

public class MapGenerator : MonoBehaviour
{

    [Header("Map parameter")]
    public int rows;
    public int columns;
    public int iterations;
    public int spawnNumber;
    public int goldNum;
    public int baselocMark;
    public List<int> spawnlocMark;
    public List<int> goldsMark;
    public int mapCenterMark;
    public int seed;

    [Header("Map tile prefabs")]
    public GameObject[] baseTiles;
    public GameObject[] midTiles;
    public GameObject[] sideTiles;
    public GameObject[] enemySpawnTiles;
    public GameObject[] fillOutTiles;

    [Header("Some initial prefabs")]
    public GameObject spawnPrefab;
    public GameObject basePrefab;
    public GameObject goldPrefab;

    public List<Grid> spawnGrids = new List<Grid>();
    public List<Grid> baseGrids = new List<Grid>();
    public List<Grid> goldGrids = new List<Grid>();

    private int mapLength;
    private List<int> reached;
    private List<int> midPoints;
    private bool hasBaked;
    private LevelSceneController levelSceneController;

    void Awake() {
        GameObject levelSceneControllerObject = GameObject.Find("LevelSceneController");
        if (levelSceneControllerObject != null) {
            levelSceneController = levelSceneControllerObject.GetComponent<LevelSceneController>();
        }
    }

    //Initialize Grids.
    List<Grid> SetUpGridSystem()
    {
        int i = 0;
        Transform gridHolder = new GameObject("Grids").transform;
        List<Grid> grids = new List<Grid>();

        for (int c = 0; c < columns * 10; c = c + 10)
        {
            for (int r = 0; r < rows * 10; r = r + 10)
            {
                Grid grid = new Grid(new Vector3(c, 0f, r), i);
                grids.Add(grid);
                grids[i].gridObject.transform.SetParent(gridHolder);
                if (r != 0)
                {
                    grids[i].left = grids[i - 1];
                }
                i++;
            }
        }

        mapLength = i;

        for (int count = 0; count <= i - 1; count++)
        {

            if (grids[count].right == null && (count + 1) % rows != 0)
            {
                grids[count].right = grids[count + 1];
            }

            if (grids[count].down == null && count <= i - rows - 1)
            {
                grids[count].down = grids[count + rows];
            }

            if (grids[count].up == null && count >= rows)
            {
                grids[count].up = grids[count - rows];
            }

        }
        return grids;
    }

    private int baseX;
    private int baseZ;
    int markBase()
    {
        baseX = Random.Range(1, rows - 1);
        baseZ = Random.Range(1, columns - 1);
        baselocMark = baseZ * rows + baseX;
        return baselocMark;
    }

    //Setup the base tiles
    void baseSetup(List<Grid> grids, Transform baseHolder)
    {
        int marks = markBase();
        GameObject baseTile = Instantiate(baseTiles[Random.Range(0, baseTiles.Length)], baseHolder);
        grids[marks].setMapTile(baseTile);
        this.reached.Add(baselocMark);
    }

    String ranTurn(Grid pointer)
    {
        List<String> temp = new List<String>();

        if (Grid.checkExit(pointer).Contains("U"))
        {
            temp.Add("U");
        }
        if (Grid.checkExit(pointer).Contains("D"))
        {
            temp.Add("D");
        }
        if (Grid.checkExit(pointer).Contains("L"))
        {
            temp.Add("L");
        }
        if (Grid.checkExit(pointer).Contains("R"))
        {
            temp.Add("R");
        }


        if (!checkInner(pointer.up))
        {
            temp.Remove("U");
        }
        if (!checkInner(pointer.down))
        {
            temp.Remove("D");
        }
        if (!checkInner(pointer.left))
        {
            temp.Remove("L");
        }
        if (!checkInner(pointer.right))
        {
            temp.Remove("R");
        }

        if (temp.Count == 0)
        {
            return "";
        }
        else
        {
            return temp[Random.Range(0, temp.Count)];
        }
    }

    bool checkInner(Grid grid)
    {
        if (grid.index >= rows && grid.index < (mapLength - rows) && ((grid.index + 1) % rows) != 0 && 
            (grid.index + 1) % rows != 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    List<String> midPointMarker(Grid grid, String turn)
    {
        String exit = Grid.checkExit(grid);
        int temp = exit.IndexOf(turn);
        exit = exit.Remove(temp);
        List<String> Exits = new List<String>();

        for (int i = 0; i < exit.Length; i++)
        {
            Exits.Add(exit[i].ToString());
        }

        return Exits;
    }


    List<int> innerGrids(List<Grid> grids)
    {
        List<int> temp = new List<int>();
        foreach (Grid x in grids)
        {
            if (checkInner(x)) temp.Add(x.index);
        }
        return temp;
    }

    List<int> markSpawn(int num, List<Grid> grids)
    {
        if (num > 6) throw new System.ArgumentException("Too many enemy spawns");

        for (int i = 0; i < num; i++)
        {
            int Place = reached[Random.Range(0, reached.Count - 1)];
            int loop = 0;
            while (Place == baselocMark || Place == (baselocMark + 1) || Place == baselocMark - 1 || 
                Place == baselocMark + rows || Place == baselocMark + rows + 1 || 
                Place == baselocMark + rows - 1 || Place == baselocMark - rows || 
                Place == baselocMark - rows - 1 || Place == baselocMark - rows + 1)
            {
                if (loop > 100) {
                    Debug.LogError("Failed to instantiate a enemy spawn due to too many iterations");
                    break;
                }
                Place = reached[Random.Range(0, reached.Count - 1)];
                loop ++;
            }
            spawnlocMark.Add(Place);
        }
        return spawnlocMark;
    }

    List<GameObject> setupSpawn(List<Grid> grids, Transform spawnHolder)
    {
        List<int> temp = markSpawn(spawnNumber, grids);
        List<GameObject> temp2 = new List<GameObject>();
        foreach (int x in temp)
        {
            GameObject spawnsTile = Instantiate(enemySpawnTiles[0], spawnHolder);
            GameObject spawn = Instantiate(spawnPrefab, grids[x].gridObject.transform.position, 
                Quaternion.identity) as GameObject;
            replaceTile(grids, x, spawnsTile, spawnHolder);
            temp2.Add(spawn);
        }
        return temp2;
    }

    void replaceTile(List<Grid> grids, int index, GameObject tile, Transform holder)
    {
        Destroy(grids[index].tile);
        grids[index].setMapTile(tile);
    }

    void setupMap(int startGrid, List<Grid> grids, Transform mapHolder)
    {
        Grid pointer = grids[startGrid];
        if (pointer.tile == null && !this.reached.Contains(pointer.index))
        {
            GameObject temp = Instantiate(midTiles[4], mapHolder) as GameObject;
            pointer.setMapTile(temp);
            this.reached.Add(pointer.index);

        }

        int i = 0;

        String Exit = Grid.checkExit(pointer);
        String Type = Grid.checkTileType(pointer);
        while (i < columns * rows * 10)
        {
            Exit = Grid.checkExit(pointer);
            Type = Grid.checkTileType(pointer);
            String turn = ranTurn(pointer);
            if (Type == "mid")
            {
                if (Exit.Contains(turn) && !this.reached.Contains(pointer.turnSwitch(turn).index))
                {
                    GameObject temp = Instantiate(midTiles[Random.Range(0, midTiles.Length)], mapHolder);
                    pointer.turnSwitch(turn).setMapTile(temp);
                    this.reached.Add(pointer.turnSwitch(turn).index);
                    List<String> midPointer = midPointMarker(pointer, turn);
                    for (int x = 0; x < midPointer.Count; x++)
                    {
                        if (checkInner(pointer.turnSwitch(midPointer[x])))
                        {
                            midPoints.Add(pointer.turnSwitch(midPointer[x]).index);
                        }
                    }
                    pointer = pointer.turnSwitch(turn);
                    pointer.makeConnect(pointer, turn);
                    i++;
                }
                else
                {
                    i++;
                    continue;
                }
            }
            else
            {
                if (Exit.Contains(turn) && !this.reached.Contains(pointer.turnSwitch(turn).index))
                {
                    GameObject temp = Instantiate(sideTiles[Random.Range(0, sideTiles.Length)], mapHolder);
                    pointer.turnSwitch(turn).setMapTile(temp);
                    this.reached.Add(pointer.turnSwitch(turn).index);
                    pointer = pointer.turnSwitch(turn);
                    pointer.makeConnect(pointer, turn);
                    i++;
                }
                else
                {
                    i++;
                    continue;
                }
            }
        }
    }

    List<int> ReturnUnreached(List<Grid> grids)
    {
        List<int> unreached = new List<int>();
        for (int i = 0; i < grids.Count; i++)
        {
            if (!reached.Contains(grids[i].index) && checkInner(grids[i]))
            {
                unreached.Add(grids[i].index);
            }
        }
        return unreached;
    }

    List<int> checkBorder(List<Grid> grids)
    {
        List<int> unreached = ReturnUnreached(grids);
        List<int> temp = new List<int>();

        foreach (int x in unreached)
        {
            if (grids[x].up.tile != null || grids[x].down.tile != null || grids[x].left.tile != null || 
                grids[x].right.tile != null)
            {
                temp.Add(x);
            }
        }
        return temp;
    }

    void fillOut(List<Grid> grids, Transform mapHolder)
    {
        List<int> unreached = checkBorder(grids);

        for (int i = 0; i < unreached.Count; i++)
        {
            GameObject temp = Instantiate(fillOutTiles[Random.Range(0, fillOutTiles.Length)], mapHolder);
            grids[unreached[i]].setMapTile(temp);
            reached.Add(unreached[i]);
        }
    }

    bool checkValid(List<Grid> grids)
    {
        int count = 0;
        foreach (Grid x in grids)
        {
            if (x.tile != null)
            {
                count++;
            }
        }

        if (count > rows * columns / 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void eraseMap(Transform mapHolder, Transform baseHolder, Transform spawnHolder, List<Grid> grids)
    {


        try
        {
            Destroy(mapHolder.gameObject);

            Destroy(baseHolder.gameObject);

            Destroy(spawnHolder.gameObject);

            reached = new List<int>();

            foreach (Grid x in grids)
            {
                x.tile = null;
            }
        }
        catch (NullReferenceException)
        {

        }
    }

    void setupGold(List<Grid> grids)
    {
        int Place = reached[Random.Range(0, reached.Count - 1)];
        for (int i = 0; i < goldNum; i++)
        {
            int loop = 0;
            while (Place == baselocMark || spawnlocMark.Contains(Place))
            {
                if (loop > 100) {
                    Debug.LogError("Failed to instantiate a gold");
                    break;
                }
                Place = reached[Random.Range(0, reached.Count - 1)];
                loop ++;
            }
        }
        goldsMark.Add(Place);

        foreach (int x in goldsMark)
        {
            GameObject gold = Instantiate(goldPrefab);
            gold.transform.position = grids[x].gridObject.transform.position;
        }

    }

    public void generate() {
        if(seed != -1) { Random.seed = this.seed; }
        reached = new List<int>();
        midPoints = new List<int>();
        List<Grid> grids = SetUpGridSystem();
        Transform mapHolder = new GameObject("MapTiles").transform;
        Transform baseHolder = new GameObject("BaseTiles").transform;
        Transform spawnHolder = new GameObject("SpawnTiles").transform;

        int i = 0;
        while (!checkValid(grids))
        {
            if (i > 50) {
                Debug.LogError("Iterate too many times while generating map, abort");
                break;
            }
            eraseMap(mapHolder, baseHolder, spawnHolder, grids);
            mapHolder = new GameObject("MapTiles" + i).transform;
            baseHolder = new GameObject("BaseTiles" + i).transform;
            spawnHolder = new GameObject("SpawnTiles" + i).transform;
            baseSetup(grids, baseHolder);
            setupMap(baselocMark, grids, mapHolder);
            for (int x = 0; x < iterations; x++)
            {
                if (x >= midPoints.Count) break;
                setupMap(midPoints[x], grids, mapHolder);
            }
            i++;
        }
        List<GameObject> spawns = setupSpawn(grids, spawnHolder);
        
        setupGold(grids);
        fillOut(grids, mapHolder);

        GameObject base_ = Character.spawn(basePrefab, DefaultCharacterData.testCharacterData,
            grids[baselocMark].gridObject.transform.position, Quaternion.identity);
        

        if (levelSceneController != null) {
            levelSceneController.player = base_.GetComponent<Character>();
            levelSceneController.spawns = spawns;
        }
        
        // Setup the camera to look at the base
        setupCamera(base_.transform.position);

        //map built
        hasBaked = false;
    }

    // Setup the camera on top of the base
    void setupCamera(Vector3 basePosition) {
        CameraController camController = Camera.main.gameObject.GetComponent<CameraController>();
        if (camController == null) {
            Debug.LogWarning("Cannot find camera controller");
            return;
        }

        Debug.Log($"Base X: {baseX}");
        Debug.Log($"Base Z: {baseZ}");

        int scale = 5;
        int xPositiveLimit = (rows - baseX + 1) * scale;
        int xNegativeLimit = (baseX + 1) * scale;
        int zPositiveLimit = (columns - baseZ + 1) * scale;
        int znegativeLimit = (baseZ + 1) * scale;

        camController.setupCamera(basePosition, xPositiveLimit, xNegativeLimit, 
            zPositiveLimit, znegativeLimit);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!hasBaked)
        {
            NavMeshBuilder.BuildNavMesh();
            hasBaked = true;
        }
    }
}
