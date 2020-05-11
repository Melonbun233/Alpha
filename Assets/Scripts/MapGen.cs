using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System;
using Random = UnityEngine.Random;


public class MapGen : MonoBehaviour
{
    public class MIDpoint
    {
        public int Gridnum;
        public String leadsTo;

        public MIDpoint(int gridnum, String leadsTo)
        {
            Gridnum = gridnum;
            this.leadsTo = leadsTo;
        }


    }

    //grid DataStructure
    public class grid
    {
        public GameObject Grid;
        public int index;
        public grid up;
        public grid down;
        public grid left;
        public grid right;
        public GameObject tile;
        public int exit;
        public String tileType;

        public grid(Vector3 vector, int index) 
        {
            Grid = new GameObject("grid" + index);
            Grid.transform.position = vector;
            this.index = index;
            up = null;
            down = null;
            left = null;
            right = null;
            tile = null;
            exit = 0;
        }

        public grid(GameObject Grid, int index)
        {
            this.Grid = Grid;
            this.index = index;
            up = null;
            down = null;
            left = null;
            right = null;
            tile = null;
            exit = 0;
        }

        public static String checkTileType(grid Grid)
        {
            if(Grid.tile.name.Contains("Base_tile_template01") || Grid.tile.name.Contains("Base_tile_template02") || Grid.tile.name.Contains("Base_tile_template03") || Grid.tile.name.Contains("Base_tile_template04") || Grid.tile.name.Contains("midtile_template01") || Grid.tile.name.Contains("midtile_template02") || Grid.tile.name.Contains("midtile_template03") || Grid.tile.name.Contains("midtile_template04") || Grid.tile.name.Contains("transiTile_template01") || Grid.tile.name.Contains("Enemy"))
            {
                return "mid";
            }
            else
            {
                return "side";
            }
        }


        public static String checkExit(grid Grid)
        {
            if (Grid.tile.name.Contains("Base_tile_template01") || Grid.tile.name.Contains("EnemySpawn_tile_template01") || Grid.tile.name.Contains("midtile_template01"))
            {
                if (Grid.exit == 1 || Grid.exit == 3)
                {
                    return "LR";
                }
                else
                {
                    return "UD";
                }
            }

            if (Grid.tile.name.Contains("Base_tile_template02") || Grid.tile.name.Contains("EnemySpawn_tile_template02") || Grid.tile.name.Contains("midtile_template02") || Grid.tile.name.Contains("sidetile_template02"))
            {
                return "LRUD";
            }

            if (Grid.tile.name.Contains("Base_tile_template03") || Grid.tile.name.Contains("midtile_template03"))
            {
                if (Grid.exit == 0)
                {
                    return "LUD";
                }

                if (Grid.exit == 1)
                {
                    return "LRU";
                }

                if (Grid.exit == 3)
                {
                    return "LRD";
                }

                if (Grid.exit == 2)
                {
                    return "RUD";
                }
            }

            if (Grid.tile.name.Contains("Base_tile_template04") || Grid.tile.name.Contains("midtile_template04"))
            {
                if(Grid.exit == 0)
                {
                    return "LU";
                }

                if(Grid.exit == 1)
                {
                    return "RU";
                }

                if(Grid.exit == 3)
                {
                    return "LD";
                }

                if(Grid.exit == 2)
                {
                    return "RD";
                }
            }

            if(Grid.tile.name.Contains("Base_tile_template05") || Grid.tile.name.Contains("sidetile_template01"))
            {
                if(Grid.exit == 0)
                {
                    return "LUD";
                }

                if (Grid.exit == 1)
                {
                    return "LRU";
                }

                if (Grid.exit == 3)
                {
                    return "LRD";
                }

                if (Grid.exit == 2)
                {
                    return "RUD";
                }
            }

            if (Grid.tile.name.Contains("Base_tile_template06") || Grid.tile.name.Contains("transiTile_template01"))
            {
                return "LRUD";
            }

            return "LRUD";
        }

        //Instantiate maptiles
        public GameObject setMapTile(GameObject tile, Transform holder) {
            this.tile = tile;
            this.tile.transform.position = Grid.transform.position;
            this.tile.transform.rotation = ranRotation();
            this.tile.transform.SetParent(holder);
            return this.tile;
        }

        public GameObject setBaseTile(GameObject tile, Transform holder)
        {
            this.tile = tile;
            this.tile.transform.position = Grid.transform.position;
            this.tile.transform.rotation = ranRotation();
            this.tile.transform.SetParent(holder);
            return this.tile;
        }

        public GameObject setSpawnTile(GameObject tile, Transform holder)
        {
            this.tile = tile;
            this.tile.transform.position = Grid.transform.position;
            this.tile.transform.rotation = ranRotation();
            this.tile.transform.SetParent(holder);
            return this.tile;
        }

        //convert unhuamn Quaternion to random Euler rotation.
        Quaternion ranRotation()
        {
            Quaternion temp = new Quaternion(0, 0, 0, 0);
            this.exit = Random.Range(0, 4);
            temp = Quaternion.Euler(0, 90f * this.exit, 0);
            return temp;
        }

        public void makeConnect(grid toConnect, String Exit)
        {
            String connectivity = grid.checkExit(toConnect);
            int i = 0;
            while (!connectivity.Contains(grid.connectOrient(Exit)))
            {
                if (i > 15) break;
                toConnect.tile.transform.rotation = ranRotation();
                connectivity = grid.checkExit(toConnect);
                i++;
            }
        }

        public static String connectOrient(String exit)
        {
            switch (exit)
            {
                case "U":
                    return "D";
                case "D":
                    return "U";
                case "L":
                    return "R";
                case "R":
                    return "L";
            }
            return null;
        }

        public grid turnSwitch(String turn)
        {
            switch (turn)
            {
                case "U":
                    return this.up;
                case "D":
                    return this.down;
                case "L":
                    return this.left;
                case "R":
                    return this.right;
            }
            return null;
        }


        public static List<grid> ReturnUnreached(List<grid> grids, List<int> index)
        {
            return null;
        }

    }



    //public parameters for UI
    public int rows;
    public int columns;
    public GameObject[] Base;
    public GameObject[] midTiles;
    public GameObject[] sideTiles;
    public GameObject[] enemySpawn;
    public int spawnNumber;

    //An empty to hold all maptiles
    private Transform mapHolder;
    private Transform spawnHolder;
    private Transform baseHolder;

    private List<int> reached;
    private List<MIDpoint> midpoints;
    private List<grid> unreached;

    //Mark base location for setting up map reasonablly.
    public int baselocMark;
    public int mapLength;

    public int mapCenterMark;

    public List<Vector3> spawnVectors = new List<Vector3>();
    public List<int> spawnlocMark = new List<int>();

    private bool hasRun = false;



    //Create Grid System with 4 direction's pointer;
    List<grid> SetUpGridSystem()
    {
        int i = 0;
        Transform gridHolder = new GameObject("Grids").transform;
        List<grid> Grid = new List<grid>();

        for (int c = 0; c < columns*10; c = c + 10)
        {
            for (int r = 0; r < rows*10; r = r + 10)
            {
                grid aGrid = new grid(new Vector3(c, 0f, r), i);
                Grid.Add(aGrid);
                Grid[i].Grid.transform.SetParent(gridHolder);
                if (r!= 0)
                {
                    Grid[i].left = Grid[i-1];
                }
                i++;
                }
            }

            for(int count = 0; count <= i-1; count++)
            {

            if (Grid[count].right == null && (count + 1) % columns != 0)
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

    //convert unhuman quaternion to euler rotation.
    Quaternion ranRotation() {
        Quaternion temp = new Quaternion(0, 0, 0, 0);
        int temp2 = Random.Range(0, 4);
        temp = Quaternion.Euler(0, 90 * temp2, 0);
        return temp;
    }


    //
    int markBase()
    {
        int row = Random.Range(1 , rows - 1);
        int colm = Random.Range(1, columns - 1);
        baselocMark = colm * rows + row;
        return baselocMark;
    }

    //Setup the base tiles
    void baseSetup(List<grid> grids)
    {
        baseHolder = new GameObject("BaseTiles").transform;
        grids[markBase()].setBaseTile(Instantiate(Base[Random.Range(0, Base.Length)]), baseHolder);
    }

    //Decide where enemy spawn is
    List<int> markSpawn(int num)
    {
        if (num > mapCenterMark) throw new System.ArgumentException("Too many enemy spawns");
        List<int> locmark = new List<int>();

        if (baselocMark <= mapCenterMark - 1)
        {
            for (int i = 0; i < num; i++)
            {
                int temp = Random.Range(mapCenterMark, rows * columns);
                while (locmark.Contains(temp))
                {
                    temp = Random.Range(mapCenterMark, rows * columns);
                }
                locmark.Add(temp);
            }
        }
        else
        {
            for (int i = 0; i < num; i++)
            {
                int temp = Random.Range(0, mapCenterMark);
                while (locmark.Contains(temp))
                {
                    temp = Random.Range(0, mapCenterMark);
                }
                locmark.Add(temp);
            }
        }

        return locmark;
    }

    //build enemy spawn tiles
    void setupSpawn(List<grid> grids)
    {
        spawnHolder = new GameObject("EnemySpawn").transform;
        spawnlocMark = markSpawn(spawnNumber);
        for(int i = 0; i < spawnNumber ; i++)
        {
            grids[spawnlocMark[i]].setSpawnTile(Instantiate(enemySpawn[Random.Range(0, enemySpawn.Length)]), spawnHolder);
            spawnVectors.Add(grids[spawnlocMark[i]].tile.transform.position);
        }
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
            return null;
        }
        else
        {
            return temp[Random.Range(0, temp.Count)];
        }
    }

    bool checkInner(grid grid)
    {
        
        if(grid.index >= rows && grid.index < (mapLength - rows) && ((grid.index + 1 ) % rows) != 0 && (grid.index + 1) % rows != 1)
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
            Exits.Add(exit[i].ToString()) ;
        }

        return Exits;
    }

    //build fillout tiles
    void setupMap(int startGrid, List<grid> grids, Transform mapHolder)
    {
        grid pointer = grids[startGrid];
        if(pointer.tile == null)
        {
            pointer.setMapTile(Instantiate(midTiles[4]), mapHolder);
        }

        print(pointer.Grid.name);
        int i = 0;

        String Exit = grid.checkExit(pointer);
        String Type = grid.checkTileType(pointer);

        while (i < columns*rows*10)
        {
            Exit = grid.checkExit(pointer);
            Type = grid.checkTileType(pointer);
            String turn = ranTurn(pointer);

            if(Type == "mid")
            {
                if (Exit.Contains(turn) && !this.reached.Contains(pointer.turnSwitch(turn).index))
                {
                    pointer.turnSwitch(turn).setMapTile(Instantiate(midTiles[Random.Range(0, midTiles.Length)]), mapHolder);
                    this.reached.Add(pointer.index);
                    List<String> midPointer = midPointMarker(pointer, turn);

                    if(hasRun == false) 
                    {
                        for (int x = 0; x < midPointer.Count; x++)
                        {
                            if (checkInner(pointer.turnSwitch(midPointer[x])))
                            {
                                midpoints.Add(new MIDpoint(pointer.turnSwitch(midPointer[x]).index, turn));
                            }
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
                    pointer.turnSwitch(turn).setMapTile(Instantiate(sideTiles[Random.Range(0, sideTiles.Length)]), mapHolder);
                    this.reached.Add(pointer.index);
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


    void Start()
    {
        mapHolder = new GameObject("MapTile").transform;
        mapLength = columns * rows;
        mapCenterMark = (int)(Math.Floor((double)columns/2d) * (double)rows + (double)rows/2d);
        reached = new List<int>();
        midpoints = new List<MIDpoint>();
        List<grid> grids = SetUpGridSystem();
        baseSetup(grids);
        //setupSpawn(grids);
        setupMap(baselocMark, grids, mapHolder);
        print("iteration: 0");
        hasRun = true;

        for(int x = 0; x < midpoints.Count; x++)
        {
            setupMap(midpoints[x].Gridnum, grids, mapHolder);
            print("iteration: " + (1 + x));
        }
        
        

    }


    void Update()
    {
        
    }
}
