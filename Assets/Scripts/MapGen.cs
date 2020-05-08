using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System;
using Random = UnityEngine.Random;


public class MapGen : MonoBehaviour
{
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
        public String exit;
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
            exit = null;
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
            exit = null;
        }

        public static String checkTileType(grid Grid)
        {
            if(Grid.tile.name.Contains("Base_tile_template01")|| Grid.tile.name.Contains("Base_tile_template02") || Grid.tile.name.Contains("Base_tile_template03") || Grid.tile.name.Contains("Base_tile_template04") || Grid.tile.name.Contains("midtile_template01") || Grid.tile.name.Contains("midtile_template02") || Grid.tile.name.Contains("midtile_template03") || Grid.tile.name.Contains("midtile_template04") || Grid.tile.name.Contains("transiTile_template01") || Grid.tile.name.Contains("Enemy"))
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
                if (Grid.tile.transform.eulerAngles.y == 90f || Grid.tile.transform.eulerAngles.y == -90f )
                {
                    return "LR";
                }
                else
                {
                    return "UD";
                }
            }

            if (Grid.tile.name.Contains("Base_tile_template02") || Grid.tile.name.Contains("EnemySpawn_tile_template02") || Grid.tile.name.Contains("midtile_template02"))
            {
                return "LRUD";
            }

            if (Grid.tile.name.Contains("Base_tile_template03") || Grid.tile.name.Contains("midtile_template03"))
            {
                if (Grid.tile.transform.eulerAngles.y == 0f)
                {
                    return "LUD";
                }

                if (Grid.tile.transform.eulerAngles.y == 90f)
                {
                    return "LRU";
                }

                if (Grid.tile.transform.eulerAngles.y == -90f)
                {
                    return "LRD";
                }

                if(Grid.tile.transform.eulerAngles.y == 180)
                {
                    return "RUD";
                }
            }

            if (Grid.tile.name.Contains("Base_tile_template04") || Grid.tile.name.Contains("midtile_template04"))
            {
                if(Grid.tile.transform.eulerAngles.y == 0f)
                {
                    return "LU";
                }

                if(Grid.tile.transform.eulerAngles.y == 90f)
                {
                    return "RU";
                }

                if(Grid.tile.transform.eulerAngles.y == -90f)
                {
                    return "LD";
                }

                if(Grid.tile.transform.eulerAngles.y == 180f)
                {
                    return "RD";
                }
            }

            if(Grid.tile.name.Contains("Base_tile_template05") || Grid.tile.name.Contains("sidetile_template01"))
            {
                if(Grid.tile.transform.eulerAngles.y == 0f)
                {
                    return "LUD";
                }

                if (Grid.tile.transform.eulerAngles.y == 90f)
                {
                    return "LRU";
                }

                if (Grid.tile.transform.eulerAngles.y == -90f)
                {
                    return "LRD";
                }

                if (Grid.tile.transform.eulerAngles.y == 180f)
                {
                    return "RUD";
                }
            }

            if (Grid.tile.name.Contains("Base_Tile_template06") || Grid.tile.name.Contains("transiTile_template01"))
            {
                return "LRUD";
            }

            return "";
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
            int temp2 = Random.Range(0, 4);
            temp = Quaternion.Euler(0, 90 * temp2, 0);
            return temp;
        }




        //check if contains a index
        public static bool Contains(int index, List<grid> grids)
        {
            bool temp = false;
            for(int i = 0; i < grids.Count; i++)
            {
                if (grids[i].index == index) temp = true;
            }
            return temp;
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

    private List<Vector3> gridPositions = new List<Vector3>();

    //Mark base location for setting up map reasonablly.
    public int baselocMark;

    private Vector3 mapCenter;
    public int mapCenterMark;

    public List<Vector3> spawnVectors = new List<Vector3>();



    //get center Vector3;
    public Vector3 getCenter() {
        return mapCenter;
    }


    void InitialiseList()
    {
        gridPositions.Clear();
        int row = rows * 10;
        int colum = columns * 10;

        for (int c = 0; c < colum; c=c+10)
        {
            for (int r = 0; r < row; r=r+10)
            {
                gridPositions.Add(new Vector3(c, 0f, r));
            }
        }
    }

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
                    Grid[i].left = new grid(GameObject.Find("grid" + (i - 1)), i - 1);
                }

                i++;
                }
            }

            for(int count = 0; count < i; count++)
            {
                Grid[count].right = new grid(GameObject.Find("grid" + (count + 1)), count + 1);

            if (Grid[count].down == null && count <= i-rows) 
            { 
                Grid[count].down = new grid(GameObject.Find("grid" + (count + rows)), count + rows);
            }

            if (Grid[count].up == null && count >= rows) 
            {
                Grid[count].up = new grid(GameObject.Find("grid" + (count - rows)), count - rows);
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

    /*
    //Mark base location for setting up map reasonablly.
    int markBase() {
        int tempC = Random.Range(0, columns);
        int tempR = Random.Range(0, rows);
        baseVector = new Vector3(tempC * 10, 0f, tempR * 10);
        return ((tempC) * rows) + tempR + 1;
    }

    //set up base tiles.
    void baseSetup() {

        baseHolder = new GameObject("Base").transform;
        int row = rows * 10;
        int colum = columns * 10;
        baselocMark = markBase();

        for (int c = 0; c < colum; c = c + 10)
        {
            for (int r = 0; r < row; r = r + 10)
            {
                if (new Vector3(c,0f,r).Equals(baseVector))
                {
                    GameObject toInstantiate = Base[Random.Range(0, Base.Length)];
                    GameObject instance = Instantiate(toInstantiate, new Vector3(c, 0f, r), ranRotation()) as GameObject;
                    instance.transform.SetParent(baseHolder);
                    break;
                }
            }
        }
    }

    //decide spawn locations according to base location
    List<int> markSpawn(int num)
    {
        if (num > mapCenterMark) throw new System.ArgumentException("Too many enemy spawns");
        List<int> locmark = new List<int>();

        if (baselocMark <= mapCenterMark-1) {
            for(int i = 0; i < num; i++)
            {
                int temp = Random.Range(mapCenterMark, rows*columns);
                while (locmark.Contains(temp))
                {
                    temp = Random.Range(mapCenterMark, rows * columns);
                }
                locmark.Add(temp);
            }
        }
        else
        {
            for(int i = 0; i < num; i++)
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

    //set up enemy spawn tiles.
    void spawnSetup() {
        spawnHolder = new GameObject("Spawns").transform;
        List<int> locmark = markSpawn(spawnNumber);
        int row = rows * 10;
        int colum = columns * 10;

        int i = 0;
        for (int c = 0; c < colum; c = c + 10)
        {
            for (int r = 0; r < row; r = r + 10)
            {
                if (locmark.Contains(i))
                {
                    spawnVectors.Add(new Vector3(c, 0f, r));
                    GameObject toInstant = enemySpawn[Random.Range(0, enemySpawn.Length)];
                    GameObject instant = Instantiate(toInstant, new Vector3(c, 0f, r), ranRotation());
                    instant.transform.SetParent(spawnHolder);
                }
                i++;
            }
        }
    }


    //fill out the rest of the map tiles.
    void mapSetup()
    {
        mapHolder = new GameObject("Map").transform;
        int row = rows * 10;
        int colum = columns * 10;

        for (int c = 0; c < colum; c = c+10)
        {
            for (int r = 0; r < row; r = r+10)
            {
                Vector3 loc = new Vector3(c, 0f, r);
                if (!loc.Equals(baseVector) && !spawnVectors.Contains(loc))
                {
                    GameObject toInstantiate = midTiles[Random.Range(0, midTiles.Length)];
                    GameObject instance = Instantiate(toInstantiate, new Vector3(c, 0f, r), ranRotation()) as GameObject;
                    instance.transform.SetParent(mapHolder);
                }

                int i = 0;
                if (i == mapCenterMark) mapCenter = loc;
                i++;
            }
        }
    }
    */

    //
    int markBase()
    {
        int tempC = Random.Range(0, columns);
        int tempR = Random.Range(0, rows);
        baselocMark = ((tempC) * rows) + tempR + 1;
        return ((tempC) * rows) + tempR + 1;
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
        List<int> temp = markSpawn(spawnNumber);
        for(int i = 0; i < spawnNumber ; i++)
        {
            grids[temp[i]].setSpawnTile(Instantiate(enemySpawn[Random.Range(0, enemySpawn.Length)]), spawnHolder);
            spawnVectors.Add(grids[temp[i]].tile.transform.position);
        }
    }

    //build fillout tiles
    void setupMap(List<grid> grids)
    {
        mapHolder = new GameObject("MapTile").transform;
        for(int i = 0; i < grids.Count; i++)
        {
            if(grids[i].tile == null)
            {
                grids[i].setMapTile(Instantiate(midTiles[Random.Range(0, midTiles.Length)]), mapHolder);
            }
        }
    }

    void Start()
    {
        mapCenterMark = (int)(Math.Floor((double)columns/2d) * (double)rows + (double)rows/2d);

        List<grid> grids = SetUpGridSystem();
        baseSetup(grids);
        setupSpawn(grids);
        setupMap(grids);
        print(grid.checkExit(grids[baselocMark]));
    }


    void Update()
    {
        
    }
}
