﻿using System.Collections;
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

            return "This has problem";
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
    public List<int> spawnlocMark = new List<int>();



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
                    Grid[i].left = Grid[i-1];
                    print("set " + i + " left to " + (i - 1));
                }
                i++;
                }
            }

            for(int count = 0; count <= i-1; count++)
            {

            if (Grid[count].right == null && (count+1) % columns != 0)
            {
                Grid[count].right = Grid[count + 1];
                print("set " + count + " right to " + (count + 1)); 
            }

            if (Grid[count].down == null && count <= i - rows - 1) 
            {
                Grid[count].down = Grid[count + rows];
                print("set " + count + " down to " + (count + rows));
            }

            if (Grid[count].up == null && count >= rows) 
            {
                Grid[count].up = Grid[count - rows];
                print("set " + count + " up to " + (count - rows));
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
        spawnlocMark = markSpawn(spawnNumber);
        for(int i = 0; i < spawnNumber ; i++)
        {
            grids[spawnlocMark[i]].setSpawnTile(Instantiate(enemySpawn[Random.Range(0, enemySpawn.Length)]), spawnHolder);
            spawnVectors.Add(grids[spawnlocMark[i]].tile.transform.position);
        }
    }

    String ranTurn()
    {
        String[] temp = { "U", "D", "L", "R" };
        return temp[Random.Range(0,4)];
    }

    bool checkCorner(grid grid)
    {
        
    }

    //build fillout tiles
    void setupMap(List<grid> grids)
    {
        mapHolder = new GameObject("MapTile").transform;
        grid pointer = grids[baselocMark];
        int i = 0;

        String Exit = grid.checkExit(pointer);
        String Type = grid.checkTileType(pointer);

        while (checkCorner(pointer))
        {
            if (i >= 500) { break; }

            Exit = grid.checkExit(pointer);
            Type = grid.checkTileType(pointer);
            String turn = ranTurn();
            if(Type == "mid")
            {
                if (Exit.Contains(turn))
                {
                    pointer.turnSwitch(turn).setMapTile(Instantiate(midTiles[Random.Range(0,midTiles.Length)]), mapHolder);
                    pointer = pointer.turnSwitch(turn);
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
                if (Exit.Contains(turn))
                {
                    pointer.turnSwitch(turn).setMapTile(Instantiate(sideTiles[Random.Range(0, sideTiles.Length)]), mapHolder);
                    pointer = pointer.turnSwitch(turn);
                    i++;
                }
                else
                {
                    i++;
                    continue;
                }
            }
        }


        /*
        while (pointer.up != null)
        {

            if (spawnlocMark.Contains(pointer.up.index))
            {
                pointer = pointer.up.up;
                continue;
            }

            String Exit = grid.checkExit(pointer);
            String Type = grid.checkTileType(pointer);

            if (Exit.Contains("U"))
            {
                if (Type.Equals("mid"))
                {
                    pointer.up.setMapTile(Instantiate(midTiles[Random.Range(0, midTiles.Length)]), mapHolder);
                    while (!grid.checkExit(pointer.up).Contains("D"))
                    {
                        pointer.up.tile.transform.rotation = ranRotation();
                    }
                    pointer = pointer.up;
                }
                else
                {
                    pointer.up.setMapTile(Instantiate(sideTiles[Random.Range(0, sideTiles.Length)]), mapHolder);
                    print(grid.checkExit(pointer.up));
                    while (!grid.checkExit(pointer.up).Contains("D"))
                    {
                        pointer.up.tile.transform.rotation = ranRotation();
                    }
                    pointer = pointer.up;
                }
            }
            else
            {
                break;
            }
        */
    }

    void Start()
    {
        mapCenterMark = (int)(Math.Floor((double)columns/2d) * (double)rows + (double)rows/2d);

        List<grid> grids = SetUpGridSystem();
        baseSetup(grids);
        //setupSpawn(grids);
        setupMap(grids);
    }


    void Update()
    {
        
    }
}
