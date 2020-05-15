using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class grid
{
        public GameObject Grid;
        public int index;
        public grid up;
        public grid down;
        public grid left;
        public grid right;
        public int exit;
        public GameObject tile;

        public grid(Vector3 position, int index)
        {
            Grid = new GameObject("grid" + index);
            Grid.transform.position = position;
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
            if (Grid.tile.name.Contains("Base_tile_template01") || Grid.tile.name.Contains("Base_tile_template02") || Grid.tile.name.Contains("Base_tile_template03") || Grid.tile.name.Contains("Base_tile_template04") || Grid.tile.name.Contains("midtile_template01") || Grid.tile.name.Contains("midtile_template02") || Grid.tile.name.Contains("midtile_template03") || Grid.tile.name.Contains("midtile_template04") || Grid.tile.name.Contains("transiTile_template01") || Grid.tile.name.Contains("Enemy"))
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
                if (Grid.exit == 0)
                {
                    return "LU";
                }

                if (Grid.exit == 1)
                {
                    return "RU";
                }

                if (Grid.exit == 3)
                {
                    return "LD";
                }

                if (Grid.exit == 2)
                {
                    return "RD";
                }
            }

            if (Grid.tile.name.Contains("Base_tile_template05") || Grid.tile.name.Contains("sidetile_template01"))
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

            if (Grid.tile.name.Contains("Base_tile_template06") || Grid.tile.name.Contains("transiTile_template01"))
            {
                return "LRUD";
            }

            return "LRUD";
        }

        public GameObject setMapTile(GameObject tile)
        {
            this.tile = tile;
            this.tile.transform.position = Grid.transform.position;
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

        public void makeConnect(grid toConnect, String Exit)
        {
            String connectivity = grid.checkExit(toConnect);
            int i = 0;
            while (!connectivity.Contains(grid.connectOrient(Exit)))
            {
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

        public static bool isConnected(grid first, grid second)
        {

            if (first.up != second && first.down != second && first.left != second && first.right != second)
            {
                return false;
            }
            else
            {
                String firstE = checkExit(first);
                String secondE = checkExit(second);
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

        public static int distance(grid first, grid second)
        {
            if (isConnected(first, second)) return 1;
            int dist = 0;
            bool arrived = false;
            grid pointer = first;
            while (arrived == false)
            {
                String exit = checkExit(pointer);

            }


            return dist;
        }

    }
