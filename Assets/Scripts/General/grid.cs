using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class Grid
{
        public GameObject gridObject;
        public int index;
        public Grid up;
        public Grid down;
        public Grid left;
        public Grid right;
        public int exit;
        public GameObject tile;

        public Grid(Vector3 position, int index)
        {
            gridObject = new GameObject("grid" + index);
            gridObject.transform.position = position;
            this.index = index;
            up = null;
            down = null;
            left = null;
            right = null;
            tile = null;
            exit = 0;
        }

        public static String checkTileType(Grid grid)
        {
            if (grid.tile.name.Contains("Base_tile_template01") || 
                grid.tile.name.Contains("Base_tile_template02") || 
                grid.tile.name.Contains("Base_tile_template03") || 
                grid.tile.name.Contains("Base_tile_template04") || 
                grid.tile.name.Contains("midtile_template01") || 
                grid.tile.name.Contains("midtile_template02") || 
                grid.tile.name.Contains("midtile_template03") || 
                grid.tile.name.Contains("midtile_template04") || 
                grid.tile.name.Contains("transiTile_template01") || grid.tile.name.Contains("Enemy"))
            {
                return "mid";
            }
            else
            {
                return "side";
            }
        }

        public static String checkExit(Grid grid)
        {
            if (grid.tile.name.Contains("Base_tile_template01") || 
                grid.tile.name.Contains("EnemySpawn_tile_template01") || 
                grid.tile.name.Contains("midtile_template01"))
            {
                if (grid.exit == 1 || grid.exit == 3)
                {
                    return "LR";
                }
                else
                {
                    return "UD";
                }
            }

            if (grid.tile.name.Contains("Base_tile_template02") || 
                grid.tile.name.Contains("EnemySpawn_tile_template02") || 
                grid.tile.name.Contains("midtile_template02") || grid.tile.name.Contains("sidetile_template02"))
            {
                return "LRUD";
            }

            if (grid.tile.name.Contains("Base_tile_template03") || grid.tile.name.Contains("midtile_template03"))
            {
                if (grid.exit == 0)
                {
                    return "LUD";
                }

                if (grid.exit == 1)
                {
                    return "LRU";
                }

                if (grid.exit == 3)
                {
                    return "LRD";
                }

                if (grid.exit == 2)
                {
                    return "RUD";
                }
            }

            if (grid.tile.name.Contains("Base_tile_template04") || grid.tile.name.Contains("midtile_template04"))
            {
                if (grid.exit == 0)
                {
                    return "LU";
                }

                if (grid.exit == 1)
                {
                    return "RU";
                }

                if (grid.exit == 3)
                {
                    return "LD";
                }

                if (grid.exit == 2)
                {
                    return "RD";
                }
            }

            if (grid.tile.name.Contains("Base_tile_template05") || grid.tile.name.Contains("sidetile_template01"))
            {
                if (grid.exit == 0)
                {
                    return "LUD";
                }

                if (grid.exit == 1)
                {
                    return "LRU";
                }

                if (grid.exit == 3)
                {
                    return "LRD";
                }

                if (grid.exit == 2)
                {
                    return "RUD";
                }
            }

            if (grid.tile.name.Contains("Base_tile_template06") || grid.tile.name.Contains("transiTile_template01"))
            {
                return "LRUD";
            }

            return "LRUD";
        }

        public GameObject setMapTile(GameObject tile)
        {
            this.tile = tile;
            this.tile.transform.position = gridObject.transform.position;
            this.tile.transform.rotation = ranRotation();
            return this.tile;
        }

        Quaternion ranRotation()
        {
            Quaternion temp = new Quaternion(0, 0, 0, 0);
            this.exit = Random.Range(0, 4);
            temp = Quaternion.Euler(0, 90f * this.exit, 0);
            return temp;
        }

        public void makeConnect(Grid toConnect, String Exit)
        {
            String connectivity = Grid.checkExit(toConnect);
            int i = 0;
            while (!connectivity.Contains(Grid.connectOrient(Exit)))
            {
                toConnect.tile.transform.rotation = ranRotation();
                connectivity = Grid.checkExit(toConnect);
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

        public Grid turnSwitch(String turn)
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

        public static bool isConnected(Grid first, Grid second)
        {

            if (first.up != second && first.down != second && first.left != second && first.right != second)
            {
                return false;
            }
            else
            {
                String firstE = Grid.checkExit(first);
                String secondE = Grid.checkExit(second);
                List<String> temp = new List<String>();
                foreach (char x in firstE)
                {
                    temp.Add(connectOrient(x.ToString()));
                }

                foreach (String x in temp)
                {
                    if (temp.Contains(x)) return true;
                }
                return false;
            }


        }

        public static int distance(Grid first, Grid second)
        {
            if (isConnected(first, second)) return 1;
            int dist = 0;
            bool arrived = false;
            Grid pointer = first;
            while (arrived == false)
            {
                String exit = checkExit(pointer);

            }


            return dist;
        }

    }
