using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class Map_Generator : MonoBehaviour
{
    public int rows;
    public int columns;
    public int iterations;
    public GameObject[] Base;
    public GameObject[] midTiles;
    public GameObject[] sideTiles;
    public GameObject[] enemySpawn;
    public GameObject[] fillOuts;
    public GameObject[] Golds;
    public int spawnNumber;
    public int goldNum;

    private int mapLength;
    private List<int> reached;
    private List<int> midPoints;

    public int baselocMark;
    public List<int> spawnlocMark;
    public List<int> goldsMark;
    public int mapCenterMark;

    //Initialize Grids.
    List<grid> SetUpGridSystem()
    {
        int i = 0;
        Transform gridHolder = new GameObject("Grids").transform;
        List<grid> Grid = new List<grid>();

        for (int c = 0; c < columns * 10; c = c + 10)
        {
            for (int r = 0; r < rows * 10; r = r + 10)
            {
                grid aGrid = new grid(new Vector3(c, 0f, r), i);
                Grid.Add(aGrid);
                Grid[i].Grid.transform.SetParent(gridHolder);
                if (r != 0)
                {
                    Grid[i].left = Grid[i - 1];
                }
                i++;
            }
        }

        mapLength = i;

        for (int count = 0; count <= i - 1; count++)
        {

            if (Grid[count].right == null && (count + 1) % rows != 0)
            {
                Grid[count].right = Grid[count + 1];
            }

            if (Grid[count].down == null && count <= i - rows - 1)
            {
                Grid[count].down = Grid[count + rows];
            }

            if (Grid[count].up == null && count >= rows)
            {
                Grid[count].up = Grid[count - rows];
            }

        }
        return Grid;
    }

    int markBase()
    {
        int row = Random.Range(1, rows - 1);
        int colm = Random.Range(1, columns - 1);
        baselocMark = colm * rows + row;
        return baselocMark;
    }

    //Setup the base tiles
    void baseSetup(List<grid> grids, Transform baseHolder)
    {
        grids[markBase()].setMapTile(Instantiate(Base[Random.Range(0, Base.Length)], baseHolder));
        this.reached.Add(baselocMark);
    }

    String ranTurn(grid pointer)
    {
        List<String> temp = new List<String>();

        if (grid.checkExit(pointer).Contains("U"))
        {
            temp.Add("U");
        }
        if (grid.checkExit(pointer).Contains("D"))
        {
            temp.Add("D");
        }
        if (grid.checkExit(pointer).Contains("L"))
        {
            temp.Add("L");
        }
        if (grid.checkExit(pointer).Contains("R"))
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

    bool checkInner(grid grid)
    {
        if (grid.index >= rows && grid.index < (mapLength - rows) && ((grid.index + 1) % rows) != 0 && (grid.index + 1) % rows != 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    List<String> midPointMarker(grid grid, String turn)
    {
        String exit = grid.checkExit(grid);
        int temp = exit.IndexOf(turn);
        exit = exit.Remove(temp);
        List<String> Exits = new List<String>();

        for (int i = 0; i < exit.Length; i++)
        {
            Exits.Add(exit[i].ToString());
        }

        return Exits;
    }


    List<int> innerGrids(List<grid> grids)
    {
        List<int> temp = new List<int>();
        foreach (grid x in grids)
        {
            if (checkInner(x)) temp.Add(x.index);
        }
        return temp;
    }

    List<int> markSpawn(int num, List<grid> grids)
    {
        if (num > 6) throw new System.ArgumentException("Too many enemy spawns");

        for (int i = 0; i < num; i++){
            int Place = reached[Random.Range(0, reached.Count - 1 )];
            while(Place == baselocMark || Place == (baselocMark + 1) || Place == baselocMark -1 || Place == baselocMark + rows || Place == baselocMark + rows + 1|| Place == baselocMark + rows -1 || Place == baselocMark - rows || Place == baselocMark - rows -1 || Place == baselocMark - rows + 1)
            {
                Place = reached[Random.Range(0, reached.Count - 1)];
            }
            spawnlocMark.Add(Place);
        }
        return spawnlocMark;
    }

    void setupSpawn(List<grid> grids, Transform spawnHolder)
    {
        List<int> temp = markSpawn(spawnNumber, grids);
        foreach(int x in temp)
        {
            GameObject spawns = Instantiate(enemySpawn[0], spawnHolder);
            replaceTile(grids, x, spawns, spawnHolder);
        }
    }

    void replaceTile(List<grid> grids, int index, GameObject tile, Transform holder)
    {
        Destroy(grids[index].tile);
        grids[index].setMapTile(tile);
    }

    void setupMap(int startGrid, List<grid> grids, Transform mapHolder)
    {
        grid pointer = grids[startGrid];
        if (pointer.tile == null && !this.reached.Contains(pointer.index))
        {
            pointer.setMapTile(Instantiate(midTiles[4], mapHolder));
            this.reached.Add(pointer.index);
        }

        int i = 0;

        String Exit = grid.checkExit(pointer);
        String Type = grid.checkTileType(pointer);
        while (i < columns * rows * 10)
        {
            Exit = grid.checkExit(pointer);
            Type = grid.checkTileType(pointer);
            String turn = ranTurn(pointer);
            if (Type == "mid")
            {
                if (Exit.Contains(turn) && !this.reached.Contains(pointer.turnSwitch(turn).index))
                {
                    pointer.turnSwitch(turn).setMapTile(Instantiate(midTiles[Random.Range(0, midTiles.Length)], mapHolder));
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
                    pointer.turnSwitch(turn).setMapTile(Instantiate(sideTiles[Random.Range(0, sideTiles.Length)], mapHolder));
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

    List<int> ReturnUnreached(List<grid> grids)
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

    List<int> checkBorder(List<grid> grids)
    {
        List<int> unreached = ReturnUnreached(grids);
        List<int> temp = new List<int>();

        foreach(int x in unreached)
        {
            if(grids[x].up.tile != null || grids[x].down.tile != null || grids[x].left.tile != null || grids[x].right.tile != null)
            {
                temp.Add(x);
            }
        }
        return temp;
    }

    void fillOut(List<grid> grids, Transform mapHolder)
    {
        List<int> unreached = checkBorder(grids);

        for (int i = 0; i < unreached.Count; i++)
        {
            grids[unreached[i]].setMapTile(Instantiate(fillOuts[Random.Range(0, fillOuts.Length)], mapHolder));
            reached.Add(unreached[i]);
        }
    }

    bool checkValid(List<grid> grids)
    {
        int count = 0;
        foreach(grid x in grids)
        {
            if(x.tile != null)
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

    void eraseMap(Transform mapHolder, Transform baseHolder, Transform spawnHolder, List<grid> grids)
    {


        try
        {
            Destroy(mapHolder.gameObject);

            Destroy(baseHolder.gameObject);

            Destroy(spawnHolder.gameObject);

            reached = new List<int>();

            foreach (grid x in grids)
            {
                x.tile = null;
            }
        }
        catch(NullReferenceException e)
        {

        }
    }

    void setupGold(List<grid> grids)
    {
        int Place = reached[Random.Range(0, reached.Count - 1)];
        for (int i = 0; i < goldNum; i++)
        {
            while(Place == baselocMark || spawnlocMark.Contains(Place))
            {
                Place = reached[Random.Range(0, reached.Count - 1)];
            }
        }
        goldsMark.Add(Place);

        foreach(int x in goldsMark)
        {
            GameObject gold = Instantiate(Golds[0]);
            gold.transform.position = grids[x].Grid.transform.position;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        reached = new List<int>();
        midPoints = new List<int>();
        List<grid> grids = SetUpGridSystem();
        Transform mapHolder = new GameObject("MapTiles").transform;
        Transform baseHolder = new GameObject("BaseTiles").transform;
        Transform spawnHolder = new GameObject("SpawnTiles").transform;

        int i = 0;
        while (!checkValid(grids))
        {
            print("times: " + i);
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
        setupSpawn(grids, spawnHolder);
        setupGold(grids);
        fillOut(grids, mapHolder);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
